using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MessageManager : MonoBehaviour
{
    public DatosTutorial datosTutorial;
    public TextMeshPro texto; 
    public Button boton;
    private int currentMessageIndex = 0;
    private Coroutine typingCoroutine;
    private bool tutorialFinalizado;
    private LevelAudioManager levelAudioManager;

    private HashSet<int> pistasMostradas;
    private void Awake() {
        levelAudioManager = gameObject.AddComponent<LevelAudioManager>();
    }

    public void Empezar(DatosTutorial datosTutorial)
    {
        pistasMostradas = new HashSet<int>();
        tutorialFinalizado = false;
        if(datosTutorial == null){
            Debug.LogError("No has agregado mensajes");
            return;
        }
        this.datosTutorial = datosTutorial;
        List<string> mensajesList = new List<string>(datosTutorial.mensajesBasicos);
        mensajesList.AddRange(datosTutorial.pistas);
        levelAudioManager.CheckAndDownloadLevelAudio(datosTutorial.id, mensajesList);
        boton.onClick.AddListener(ShowNextMessage);
        ShowNextMessage();
        GameObject.Find("info_Button").GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(ShowPista()); });                                                                                                                                                            
    }

    private void ShowNextMessage()
    {
        if (currentMessageIndex < datosTutorial.mensajesBasicos.Length)
        {
            levelAudioManager.PlayAudio(datosTutorial.id, currentMessageIndex);
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            
            typingCoroutine = StartCoroutine(TypeText(datosTutorial.mensajesBasicos[currentMessageIndex]));
            currentMessageIndex++;
        }
        else
        {
            tutorialFinalizado = true;
            texto.text = ""; 
            boton.onClick.RemoveListener(ShowNextMessage);
            boton.gameObject.SetActive(false);
        }
    }

    private IEnumerator TypeText(string message)
    {
        texto.text = "";
        foreach (char letter in message.ToCharArray())
        {
            texto.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator ShowPista(){
        if (!tutorialFinalizado) yield break;
        boton.gameObject.SetActive(true);
        
        if(pistasMostradas.Count >= datosTutorial.pistas.Length){
            pistasMostradas.Clear();
        }
        int randomIndex = Random.Range(0, datosTutorial.pistas.Length);
        while (pistasMostradas.Contains(randomIndex)){
            randomIndex = Random.Range(0, datosTutorial.pistas.Length);
        }
        pistasMostradas.Add(randomIndex);

        levelAudioManager.PlayAudio(datosTutorial.id, currentMessageIndex + randomIndex);
        string mensaje = datosTutorial.pistas[randomIndex];

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(mensaje));

        yield return null;
    }
    
}
