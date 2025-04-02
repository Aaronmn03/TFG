using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

public class LevelAudioManager : MonoBehaviour
{
    private string apiKey;
    private string voiceId = "yiWEefwu5z3DQCM79clN";
    
    private bool apiKeyLoaded = false;
    private AudioSource currentAudioSource = null;

    private IEnumerator LoadApiKey()
    {
        string filePath = Path.Combine("C:\\Users\\aaron\\Documents\\URJC II\\TFGInfo\\Keys", "api_config.json");

        string jsonText;
        if (filePath.Contains("://") || filePath.Contains("jar:"))
        {
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            yield return request.SendWebRequest();
            jsonText = request.downloadHandler.text;
        }
        else
        {
            jsonText = File.ReadAllText(filePath);
        }

        ApiConfig config = JsonUtility.FromJson<ApiConfig>(jsonText);
        apiKey = config.apiKey;
        apiKeyLoaded = true;
        Debug.Log("API Key cargada correctamente.");
    }

    public void CheckAndDownloadLevelAudio(int levelNumber, List<string> tutorialText)
    {
        #if UNITY_EDITOR
            StartCoroutine(CheckAndDownloadLevelAudioCoroutine(levelNumber, tutorialText));
        #endif
    }
    private IEnumerator CheckAndDownloadLevelAudioCoroutine(int levelNumber, List<string> tutorialTexts)
    {
        yield return StartCoroutine(LoadApiKey());
        string levelFolderPath = Path.Combine(Application.streamingAssetsPath, "Tutorial", $"Nivel_{levelNumber+1}");
        if (!Directory.Exists(levelFolderPath))
        {
            Directory.CreateDirectory(levelFolderPath);
            Debug.Log("📁 Carpeta creada: " + levelFolderPath);
        }
         for (int i = 0; i < tutorialTexts.Count; i++)
        {
            string audioFilePath = Path.Combine(levelFolderPath, "tutorial_" + i + ".mp3"); 
            if (!File.Exists(audioFilePath))
            {
                Debug.Log($"🎵 Audio {i} no encontrado, descargando...");
                yield return StartCoroutine(DownloadAudio(tutorialTexts[i], audioFilePath));
            }
            else
            {
                Debug.Log($"✅ El audio {i} ya existe: " + audioFilePath);
            }
        }
    }

    private IEnumerator DownloadAudio(string text, string filePath)
    {
        string url = "https://api.elevenlabs.io/v1/text-to-speech/" + voiceId;
        string jsonData = "{\"text\":\"" + text + "\",\"model_id\":\"eleven_multilingual_v2\",\"voice_settings\":{\"stability\":0.5,\"similarity_boost\":0.5}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("xi-api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            byte[] audioData = request.downloadHandler.data;
            if (audioData != null && audioData.Length > 0)
            {
                File.WriteAllBytes(filePath, audioData);
                Debug.Log("✅ Audio descargado en: " + filePath);
            }
            else
            {
                Debug.LogError("❌ Error: El audio descargado está vacío.");
            }
        }
        else
        {
            Debug.LogError("❌ Error en la descarga: " + request.error);
            Debug.LogError($"Respuesta del servidor: {request.downloadHandler.text}");
        }
    }
    public void PlayAudio(int levelNumber, int currentMessageIndex)
    {
        Debug.Log($"▶️ Iniciando PlayAudio con levelNumber: {levelNumber + 1}, currentMessageIndex: {currentMessageIndex}");
        AudioClip audioClip = Resources.Load<AudioClip>($"Tutorial/Nivel_{levelNumber + 1}/tutorial_{currentMessageIndex}");

        if (audioClip != null)
        {
            Debug.Log("✅ Archivo de audio encontrado, procediendo a reproducir.");

            if (currentAudioSource != null && currentAudioSource.isPlaying)
            {
                Debug.Log("⏹️ Deteniendo y eliminando el audio anterior.");
                currentAudioSource.Stop();
                Destroy(currentAudioSource);
                currentAudioSource = null;
            }
            float volumenVoz = PlayerPrefs.GetFloat("VolumenVoz");
            currentAudioSource = gameObject.AddComponent<AudioSource>();
            currentAudioSource.clip = audioClip;
            currentAudioSource.volume = volumenVoz;
            currentAudioSource.Play();
        }
        else
        {
            Debug.LogError($"❌ El archivo de audio no existe: {$"Resources/Tutorial/Nivel_{levelNumber + 1}/tutorial_{currentMessageIndex}"}");
        }
    }

    private class ApiConfig
    {
        public string apiKey;
    }
}
