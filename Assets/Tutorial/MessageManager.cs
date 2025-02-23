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

    private bool tutorialFinalizado;

    public void Empezar(DatosTutorial datosTutorial)
    {
        tutorialFinalizado = false;
        if(datosTutorial == null){
            Debug.Log("No has agregado mensajes");
            return;
        }
        this.datosTutorial = datosTutorial;
        boton.onClick.AddListener(ShowNextMessage);
        ShowNextMessage();
        GameObject.Find("info_Button").GetComponent<Button>().onClick.AddListener(ShowPista);
    }

    private void ShowNextMessage()
    {
        if (currentMessageIndex < datosTutorial.mensajesBasicos.Length)
        {
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

    public void ShowPista(){
        if(!tutorialFinalizado){return;}
        
        boton.gameObject.SetActive(true);
        int randomIndex = Random.Range(0, datosTutorial.pistas.Length);
        string mensaje = datosTutorial.pistas[randomIndex];

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(mensaje));

    }
}
