using UnityEngine;
using Vuforia; 
using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class Nivel : MonoBehaviour
{
    private string nombre;
    public int id;
    private string objetivo;
    private List<DatosBloque> bloques;
    
    private BloqueManager bloqueManager;
    /*---VUFORIA DATA---*/
    public Transform groundPlane;   
    public GameObject PlaneFinder;   
    private GameObject airFinder;   
    private GameObject airPlane;   

    /*---UI DATA---*/
    public GameObject startCanvas;
    public GameObject winCanvas;
    public GameObject playCanvas;
    public GameObject loseCanvas;
    private GameObject areaTrabajo;
    /*-------------*/
    
    private void Awake() {
        startCanvas = GameObject.Find("StartCanvas");
        winCanvas = GameObject.Find("WinCanvas");
        playCanvas = GameObject.Find("PrincipalCanvas");
        areaTrabajo = GameObject.Find("AreaTrabajo");
        loseCanvas = GameObject.Find("LoseCanvas");
        groundPlane = GameObject.Find("Ground Plane Stage").transform;
        PlaneFinder = GameObject.Find("Plane Finder");
        airFinder = GameObject.Find("Mid Air Positioner");
        airPlane = GameObject.Find("Mid Air Stage(Spawner)");
        bloqueManager = GetComponent<BloqueManager>();
    }

    public GameObject GetAirFinder(){
        return airFinder;
    }
    public GameObject GetAirPlane(){
        return airPlane;
    }

    public List<DatosBloque> GetBloques(){
        return bloques;
    }

    public void AsignarNivel(string nombre, string objetivo, int id, List<DatosBloque> bloques)
    {
        this.nombre = nombre;
        this.objetivo = objetivo;
        this.id = id;
        this.bloques = bloques;
        InicializarNivel();  
    }

    public void InicializarNivel(){
        PlaneFinder.SetActive(true);
        airFinder.SetActive(false);
        LimpiarAreaTrabajo();
        foreach (Transform child in groundPlane){
            child.gameObject.SetActive(false);
        }
        startCanvas.SetActive(true);
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        playCanvas.SetActive(false);
        LimpiarAreaTrabajo();
        bloqueManager.GenerarBloques(bloques, playCanvas);
    }
    private void MostrarNivel(){
        foreach (Transform child in groundPlane){
            child.gameObject.SetActive(false);
        }
        groundPlane.GetChild(id-1).gameObject.SetActive(true);
        GameObject actionableObject = GameObject.FindObjectOfType<ActionableObject>()?.gameObject;
        if (actionableObject != null){
            actionableObject.GetComponent<ActionableObject>().ResetObject();
        }
    }

    public void NivelInstanciado(){
        startCanvas.SetActive(false);
        playCanvas.SetActive(true);   
        MostrarNivel();
        PlaneFinder.SetActive(false);
    }



    public void Win(){
        playCanvas.SetActive(false);
        winCanvas.SetActive(true);
    }

    public void Lose(){
        playCanvas.SetActive(false);
        loseCanvas.SetActive(true);
    }
    public void Play(){
        List<BloqueRaiz> bloquesRaiz = GetBloquesRaiz(areaTrabajo);
        foreach (Bloque bloque in bloquesRaiz){
            StartCoroutine(bloque.Action());
        }
    }
    private List<BloqueRaiz> GetBloquesRaiz(GameObject parent)
    {
        return GetAllChildren(parent).OfType<BloqueRaiz>().ToList();
    }


    public void LimpiarAreaTrabajo(){
        List<Bloque> list = GetAllChildren(areaTrabajo);
        if(list.Count <= 1) return;
        foreach (Bloque bloque in list){
            bloque.DeleteBloque();
        }
    }

    public List<Bloque> GetAllChildren(GameObject parent){
        List<Bloque> children = new List<Bloque>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject.GetComponent<Bloque>());
        }
        return children;
    }
}
