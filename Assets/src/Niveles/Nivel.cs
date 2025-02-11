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
    public Transform groundPlane;   
    public GameObject PlaneFinder;   
    public GameObject airFinder;   

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
        GenerarBloques();
    }
    private void MostrarPrefab(){
        foreach (Transform child in groundPlane){
            child.gameObject.SetActive(false);
        }
        Debug.Log($"Mostrando prefab {id - 1}");
        groundPlane.GetChild(id-1).gameObject.SetActive(true);
        GameObject actionableObject = GameObject.FindObjectOfType<ActionableObject>()?.gameObject;
        if (actionableObject != null){
            actionableObject.GetComponent<ActionableObject>().ResetObject();
        }
    }

    public void NivelInstanciado(){
        startCanvas.SetActive(false);
        playCanvas.SetActive(true);   
        MostrarPrefab();
        PlaneFinder.SetActive(false);
        airFinder.SetActive(true);
    }

    public void OnObjectPositioned(){
        airFinder.SetActive(false);
    }

    private void GenerarBloques(){
        Transform contenedorBloques = playCanvas.transform.GetChild(2).GetChild(0).GetChild(0);
        LimpiarBloques(contenedorBloques);

        foreach (DatosBloque datos in this.bloques)
        {
            if (datos == null) continue;
            GameObject nuevoBloque = Instantiate(datos.prefab, contenedorBloques);
            Bloque bloque = nuevoBloque.GetComponent<Bloque>();
            Debug.Log($"Bloque instanciado: {bloque.name}");
            bloque.SetColor(datos.color);
            bloque.SetText(datos.texto, datos.colorTexto);
            bloque.SetName(datos.nombre);
            bloque.gameObject.SetActive(false);
        }
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

    private void LimpiarBloques(Transform contenedorBloques){
        foreach (Transform child in contenedorBloques)
        {
            Destroy(child.gameObject);
        }
    }

    public void LimpiarAreaTrabajo(){
        List<Bloque> list = GetAllChildren(areaTrabajo);
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
