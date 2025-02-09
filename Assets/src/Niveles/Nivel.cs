using UnityEngine;
using Vuforia; 
using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class Nivel : MonoBehaviour
{
    private string nombre;
    private GameObject prefabToSpawn;
    private string objetivo;
    private List<DatosBloque> bloques;
    private GameObject spawnedObject;

    private bool hasSpawned = false;
    

    /*---UI DATA---*/
    public GameObject startCanvas;
    public GameObject winCanvas;
    public GameObject playCanvas;
    private GameObject areaTrabajo;
    /*-------------*/
    
    private void Awake() {
        startCanvas = GameObject.Find("StartCanvas");
        winCanvas = GameObject.Find("WinCanvas");
        playCanvas = GameObject.Find("PrincipalCanvas");
        areaTrabajo = GameObject.Find("AreaTrabajo");

    }
    public void AsignarNivel(string nombre, string objetivo, GameObject nuevoPrefab, List<DatosBloque> bloques)
    {
        this.nombre = nombre;
        this.objetivo = objetivo;
        prefabToSpawn = nuevoPrefab;
        this.bloques = bloques;
        InicializarNivel();  
    }



    public void InicializarNivel(){
        LimpiarAreaTrabajo();
        if (hasSpawned){
            Destroy(spawnedObject);
            spawnedObject = null;
            hasSpawned = false;
        }
        startCanvas.SetActive(true);
        winCanvas.SetActive(false);
        playCanvas.SetActive(false);
        LimpiarAreaTrabajo();
    }
    public void OnInteractiveHitTest(HitTestResult hitTestResult)
    {
        if (!hasSpawned && prefabToSpawn != null)
        {
            startCanvas.SetActive(false);
            playCanvas.SetActive(true);
            GenerarBloques();
            spawnedObject = Instantiate(prefabToSpawn, hitTestResult.Position, hitTestResult.Rotation);
            hasSpawned = true;
        }
    }

    private void GenerarBloques()
    {
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
        }
    }

    public void Win(){
        playCanvas.SetActive(false);
        winCanvas.SetActive(true);
    }
    public void Play(){
        List<BloqueRaiz> bloquesRaiz = GetBloquesRaiz(areaTrabajo);
        foreach (Bloque bloque in bloquesRaiz){
            bloque.Action();
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
