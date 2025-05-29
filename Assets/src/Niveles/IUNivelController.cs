using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;
using UnityEngine.SceneManagement;

public class IUNivelController : MonoBehaviour
{
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject playCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject playButton;
    private BloqueManager bloqueManager;

    public void IniciarNivel(DatosNivel datosNivel)
    {
        playButton.GetComponent<Button>().interactable = true;
        playButton.GetComponent<Image>().color = new Color(144f / 255f, 144f / 255f, 144f / 255f);
        playButton.GetComponentInChildren<TextMeshProUGUI>().text = "PLAY";
        bloqueManager = GetComponent<BloqueManager>();
        startCanvas.SetActive(true);
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        playCanvas.SetActive(false);
        bloqueManager.GenerarBloques(datosNivel.bloquesDisponibles, playCanvas);
        SetLevelInfo(datosNivel);
    }

    public void Play()
    {
        playButton.GetComponent<Button>().interactable = false;
        playButton.GetComponent<Image>().color = new Color(56f / 255f, 241f / 255f, 18f / 255f);
        playButton.GetComponentInChildren<TextMeshProUGUI>().text = "PLAYING...";
    }

    public void Stop()
    {
        playButton.GetComponent<Button>().interactable = true;
        playButton.GetComponent<Image>().color = new Color(144f / 255f, 144f / 255f, 144f / 255f);
        playButton.GetComponentInChildren<TextMeshProUGUI>().text = "PLAY";
    }

    public void ReiniciarNivel()
    {
        playCanvas.SetActive(true);
        loseCanvas.SetActive(false);
        Stop();
    }

    public void NivelInstanciado(){
        startCanvas.SetActive(false);
        playCanvas.SetActive(true);
    }

    public void Win(){
        winCanvas.SetActive(true);
        playCanvas.SetActive(false);
    }

    public void Lose(){
        loseCanvas.SetActive(true);
        winCanvas.SetActive(false);
        loseCanvas.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "¡Error! Vamos, tú puedes.";
        playCanvas.SetActive(false);
    }

    public void SetLoseMessage(string message){
        loseCanvas.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "¡Error!\n"+ message +"\nTú puedes.";
    }

    private void SetLevelInfo(DatosNivel datosNivel){
        int numeroNivel;
        string objetivo;
        if(datosNivel.id <= 0){
            numeroNivel = 0;
        }else{
            numeroNivel = datosNivel.id;
        }
        if(datosNivel.objetivo == null){
            objetivo = "El nivel no tiene objetivo";
        }else{
            objetivo = datosNivel.objetivo;
        }
        playCanvas.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Nivel" + numeroNivel + " - " + objetivo;
    }

    public void CargarMenuPrincipal(){
        SceneManager.LoadScene("MenuPrincipal");
    }
}