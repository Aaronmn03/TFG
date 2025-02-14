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
    private bool win;
    private List<DatosBloque> bloques;
    private BloqueManager bloqueManager;
    private ZonaBloques zonaBloques;

    /*---VUFORIA DATA---*/
    public Transform groundPlane;   
    public GameObject PlaneFinder;   
    private GameObject airFinder;   

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
        airFinder = GameObject.Find("AreaTrabajoFinder");
        bloqueManager = GetComponent<BloqueManager>();
        zonaBloques = GameObject.Find("Zona_programacion").GetComponent<ZonaBloques>();
        win = false;
    }

    public void ActivateZonaBloques(){
        zonaBloques.SelectedObject();
    }
    public void UnActivateZonaBloques(){
        zonaBloques.NonSelectedObject();
    }

    public GameObject GetAirFinder(){
        return airFinder;
    }

    public void ActivateAirFinder(){
        airFinder.SetActive(true);
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
        win = true;
        playCanvas.SetActive(false);
        winCanvas.SetActive(true);
    }

    public void Lose(){
        if(!win){
            playCanvas.SetActive(false);
            loseCanvas.SetActive(true);
        }
        
    }
    public void Play(){
        //Tambien habra que comprobar todas las areas de trabajo y que todas ellas ejecuten sus codigos
        areaTrabajo = GameObject.Find("AreaTrabajo");
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
        if(areaTrabajo != null){
            List<Bloque> list = GetAllChildren(areaTrabajo);
            if(list.Count <= 1) return;
            foreach (Bloque bloque in list){
                bloque.DeleteBloque();
            }
            Destroy(areaTrabajo.transform.parent.gameObject);
        }
    }

    private List<Bloque> GetAllChildren(GameObject parent){
        List<Bloque> children = new List<Bloque>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject.GetComponent<Bloque>());
        }
        return children;
    }
}
