using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgramableObject : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material originalMaterial;
    [SerializeField] private Material selectedMaterial;

    private ZonaProgramacion zonaProgramacion;
    private Nivel nivel;

    void Start()
    {
        gameObject.AddComponent<ActionableObject>();        
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
        nivel = GameObject.Find("LevelHandler").GetComponent<Nivel>();
    }

    public void SetZonaProgramacion(ZonaProgramacion zonaProgramacion){
        this.zonaProgramacion = zonaProgramacion;
    }

    public GameObject GetZonaProgramacion(){
        return zonaProgramacion.gameObject;
    }

    public void SelectObject()
    {
        objectRenderer.material = selectedMaterial;
        if(zonaProgramacion == null){
            nivel.ActivateAirFinder();
        }else{
            nivel.ActivateZonaBloques();
            zonaProgramacion.SelectedObject();
        }
    }
    public void DeselectObject()
    {
        if(zonaProgramacion == null){
            nivel.GetAirFinder().SetActive(false);
        }else{
            zonaProgramacion.NonSelectedObject();
            nivel.UnActivateZonaBloques();
        }
        objectRenderer.material = originalMaterial;

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

    public void LimpiarBloques(){
        zonaProgramacion.LimpiarBloques();
    }
}
