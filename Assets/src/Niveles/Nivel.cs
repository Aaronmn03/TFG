using UnityEngine;
using Vuforia; 
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Nivel : MonoBehaviour
{
    public delegate void OnPlayEvent();
    public event OnPlayEvent PlayEvent;
    public delegate void OnResetEvent();
    public event OnResetEvent ResetEvent;
    private DatosNivel datosNivel;
    private ZonaBloques zonaBloques;
    public List<ProgramableObject> programableObjects;
    private bool win;
    private bool lose;
    private bool ifUsed;
    private bool bucleUsed;
    private HashSet<Bloque> bloquesUsados;
    [SerializeField] private IUNivelController iUNivelController;
    private AudioController audioController;

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
        audioController = FindObjectOfType<AudioController>();
    }

    public DatosNivel GetData(){
        return datosNivel;
    }
    public GameObject GetAirFinder(){
        return airFinder;
    }

    public void IfUsed(){
        this.ifUsed = true;
    }

    public void BucleUsed(){
        this.bucleUsed = true;
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
        lose = false;
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
        audioController.AdjustAudioVolumes();
    }

    private void MostrarNivel(){
        foreach (Transform child in groundPlane){
            child.gameObject.SetActive(false);
        }
        groundPlane.GetChild(datosNivel.id-1).gameObject.SetActive(true);
        groundPlane.GetChild(datosNivel.id-1).transform.GetComponentInChildren<MessageManager>().Empezar(datosNivel.datosTutorial);
        UnActivateZonaBloques();
        ObtenerProgramableObjects();
        ReiniciarPosiciones();        
    }

    public void Reiniciar(){
        ifUsed = false;
        bucleUsed = false;
        bloquesUsados = new HashSet<Bloque>();
        ResetEvent?.Invoke();
        win = false;
        lose = false;
        foreach (ProgramableObject programableObject in programableObjects){
            programableObject.DeselectObject();
        }
        iUNivelController.ReiniciarNivel();
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
        if(!lose && !win){
            if(!ComprobarCondiciones()){
                return;
            }
            win = true;
            iUNivelController.Win();
            int nivelesPasados = PlayerPrefs.GetInt("MaxLevel");
            if(nivelesPasados <= datosNivel.id){
                int nuevoNivel = datosNivel.id + 1;
                PlayerPrefs.SetInt("MaxLevel", nuevoNivel);
                PlayerPrefs.Save();
            }
        }
    }

    private bool ComprobarCondiciones(){
        if(datosNivel.requireIF && !ifUsed){
            Lose("Para este nivel se requiere al menos un IF");
            return false;
        }
        if(datosNivel.requireBucle && !bucleUsed){
            Lose("Para este nivel se requiere al menos un Bucle");
            return false;
        }
        if(datosNivel.maxBloques != 0 && datosNivel.maxBloques < GetBloquesUsados()){
            Lose("Has usado más bloques de los permitidos");
            return false;
        }
        return true;
    }

    private int GetBloquesUsados(){
        return bloquesUsados.Count;
    }
    public void AñadirBloqueUsado(Bloque bloque){
        if(bloquesUsados == null){
            bloquesUsados = new HashSet<Bloque>();
        }
        if(!bloquesUsados.Contains(bloque)){
            bloquesUsados.Add(bloque);
            Debug.Log("Añadimos un nuevo bloque, cantidad actual: " + bloquesUsados.Count + "nombre del bloque añadido: " + bloque.name);
        }
    }

    public void Lose()
    {
        HandleLose();
    }

    public void Lose(string message)
    {
        if(HandleLose()){
            iUNivelController.SetLoseMessage(message);
        }
    }
    private bool HandleLose()
    {
        if (!win && !lose)
        {
            lose = true;
            iUNivelController.Lose();
            StopAllCoroutines();
            foreach (ProgramableObject programableObject in programableObjects){
                programableObject.StopExecution();
            }
            return true;
        }
        return false;
    }
    public void Play(){
        foreach (ProgramableObject programableObject in programableObjects){
            programableObject.ExecuteBloques();
        }
        PlayEvent?.Invoke();
    }
    public void ActivateZonaBloques(){
        zonaBloques.SelectedObject();
    }
    public void UnActivateZonaBloques(){
        zonaBloques.NonSelectedObject();
    }
}
