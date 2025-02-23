using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public DatosTutorial datosTutorial;
    public TextMeshPro texto; 
    public Button boton;
    private int currentMessageIndex = 0;
    private Coroutine typingCoroutine;

    public void Empezar(DatosTutorial datosTutorial)
    {
        this.datosTutorial = datosTutorial;
        boton.onClick.AddListener(ShowNextMessage);
        ShowNextMessage();
    }

    private void ShowNextMessage()
    {
        if (currentMessageIndex < datosTutorial.mensajesBasicos.Length)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine); // Detener la animación anterior si aún está en curso
            
            typingCoroutine = StartCoroutine(TypeText(datosTutorial.mensajesBasicos[currentMessageIndex]));
            currentMessageIndex++;
        }
        else
        {
            texto.text = ""; 
            boton.onClick.RemoveListener(ShowNextMessage);
            boton.gameObject.SetActive(false);
        }
    }

    private IEnumerator TypeText(string message)
    {
        texto.text = ""; // Limpiar el texto antes de escribir
        foreach (char letter in message.ToCharArray())
        {
            texto.text += letter;
            yield return new WaitForSeconds(0.05f); // Velocidad de escritura
        }
    }
}
