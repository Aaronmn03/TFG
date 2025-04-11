using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgramableObject : MonoBehaviour
{
    private Renderer objectRenderer;
    [SerializeField] private ZonaProgramacion zonaProgramacion;
    private Nivel nivel;
    [SerializeField] private GameObject luz;
    private ActionableObject actionableObject;
    void Start()
    {
        actionableObject = gameObject.AddComponent<ActionableObject>();        
        objectRenderer = GetComponent<Renderer>();
        nivel = GameObject.Find("LevelHandler").GetComponent<Nivel>();
        luz.SetActive(false);
    }

    public void SetZonaProgramacion(ZonaProgramacion zonaProgramacion){
        this.zonaProgramacion = zonaProgramacion;
    }

    public ZonaProgramacion GetZonaProgramacion(){
        return zonaProgramacion;
    }

    public void SelectObject()
    {
        luz.SetActive(true);
        nivel.SelectedObject(this);
        if(zonaProgramacion == null){
            nivel.ActivateAirFinder();
        }else{
            nivel.ActivateZonaBloques();
            zonaProgramacion.SelectedObject();
        }
    }
    public void DeselectObject()
    {
        luz.SetActive(false);
        if(zonaProgramacion == null){
            nivel.GetAirFinder().SetActive(false);
        }else{
            zonaProgramacion.NonSelectedObject();
            nivel.UnActivateZonaBloques();
        }
    }

    public void PutBloqueRaiz(BloqueRaiz bloqueRaiz){
        if (!zonaProgramacion.GetBloquesRaiz().Contains(bloqueRaiz))
        {
            zonaProgramacion.GetBloquesRaiz().Add(bloqueRaiz);
        }
    }
    public void RemoveBloqueRaiz(BloqueRaiz bloqueRaiz){
        zonaProgramacion.GetBloquesRaiz().Remove(bloqueRaiz);
    }

    public void ExecuteBloques(){
        zonaProgramacion.Play();
    }
    public void StopExecution(){
        zonaProgramacion.Stop();
        actionableObject.Stop();
    }
    public void LimpiarBloques(){
        zonaProgramacion.LimpiarBloques();
    }
}
