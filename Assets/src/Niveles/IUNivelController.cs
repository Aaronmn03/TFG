using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
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
}