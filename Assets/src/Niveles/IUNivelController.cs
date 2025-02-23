using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
using TMPro;

public class IUNivelController : MonoBehaviour
{
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject playCanvas;
    [SerializeField] private GameObject loseCanvas;
    private BloqueManager bloqueManager;

    public void IniciarNivel(DatosNivel datosNivel){
        bloqueManager = GetComponent<BloqueManager>();
        startCanvas.SetActive(true);
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        playCanvas.SetActive(false);
        bloqueManager.GenerarBloques(datosNivel.bloquesDisponibles, playCanvas);
        SetLevelInfo(datosNivel);
    }

    public void ReiniciarNivel(){
        playCanvas.SetActive(true);
        loseCanvas.SetActive(false);
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
        playCanvas.SetActive(false);
    }

    private void SetLevelInfo(DatosNivel datosNivel){
        int numeroNivel;
        string objetivo;
        if(datosNivel.id == null || datosNivel.id <= 0){
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
}