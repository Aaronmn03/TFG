using UnityEngine;
using Vuforia; 
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Nivel : MonoBehaviour
{
    private DatosNivel datosNivel;
    private ZonaBloques zonaBloques;
    public List<ProgramableObject> programableObjects;
    private bool win;

    [SerializeField] private IUNivelController iUNivelController;

    /*---VUFORIA DATA---*/
    public Transform groundPlane;   
    [SerializeField] public GameObject PlaneFinder;   
    private GameObject airFinder;   

    
    private void Awake() {
        groundPlane = GameObject.Find("Ground Plane Stage").transform;
        PlaneFinder = GameObject.Find("Plane Finder");
        airFinder = GameObject.Find("AreaTrabajoFinder");
        zonaBloques = GameObject.Find("Zona_programacion").GetComponent<ZonaBloques>();
        iUNivelController = GetComponent<IUNivelController>();
    }

    public GameObject GetAirFinder(){
        return airFinder;
    }

    public void ActivateAirFinder(){
        airFinder.SetActive(true);
    }

    public void AsignarNivel(DatosNivel datosNivel){
        this.datosNivel = datosNivel;
        InicializarNivel();  
    }

    public void InicializarNivel(){
        foreach (ProgramableObject programableObject in programableObjects){
            programableObject.LimpiarBloques();
        }
        win = false;
        PlaneFinder.SetActive(true);
        airFinder.SetActive(false);
        foreach (Transform child in groundPlane){
            child.gameObject.SetActive(false);
        }
        iUNivelController.IniciarNivel(datosNivel);
    }

    public void NivelInstanciado(){
        iUNivelController.NivelInstanciado();
        MostrarNivel();
        PlaneFinder.SetActive(false);
    }

    private void MostrarNivel(){
        foreach (Transform child in groundPlane){
            child.gameObject.SetActive(false);
        }
        groundPlane.GetChild(datosNivel.id-1).gameObject.SetActive(true);
        ObtenerProgramableObjects();
        ReiniciarPosiciones();        
    }

    private void ObtenerProgramableObjects(){
        programableObjects = GameObject.FindObjectsOfType<ProgramableObject>().ToList();
    }

    private void ReiniciarPosiciones(){
        GameObject actionableObject = GameObject.FindObjectOfType<ActionableObject>()?.gameObject;
        foreach (ProgramableObject programableObject in programableObjects) {
            ActionableObject actionable = programableObject.GetComponent<ActionableObject>();
            if (actionable != null) {
                actionable.ResetObject();
            }
        }
    }
    public void Win(){
        win = true;
        iUNivelController.Win();
    }

    public void Lose(){
        if(!win){
            iUNivelController.Lose();
        }
    }
    public void Play(){
        foreach (ProgramableObject programableObject in programableObjects){
            programableObject.ExecuteBloques();
        }
    }
    public void ActivateZonaBloques(){
        zonaBloques.SelectedObject();
    }
    public void UnActivateZonaBloques(){
        zonaBloques.NonSelectedObject();
    }
}
