using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgramableObject : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material originalMaterial;
    [SerializeField] private Material selectedMaterial;

    [SerializeField] private List<Bloque> bloquesRaiz;

    public ZonaProgramacion zonaProgramacion;
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

    public void SelectObject()
    {
        objectRenderer.material = selectedMaterial;
        if(zonaProgramacion == null){
            nivel.ActivateAirFinder();
        }else{
            nivel.ActivateZonaBloques();
            zonaProgramacion.SelectedObject();
        }
        ShowBloques();
    }
    public void DeselectObject()
    {
        objectRenderer.material = originalMaterial;
        zonaProgramacion.NonSelectedObject();
        nivel.UnActivateZonaBloques();
        HideBloques();
    }
    
    private void ShowBloques(){
        foreach (Bloque bloqueRaiz in bloquesRaiz){
            bloqueRaiz.Show();
            foreach (Bloque bloque in bloqueRaiz.getListConectados()){
                bloque.Show();
            }
        }
    }

    private void HideBloques(){
        foreach (Bloque bloqueRaiz in bloquesRaiz){
            bloqueRaiz.Hide();
            foreach (Bloque bloque in bloqueRaiz.getListConectados()){
                bloque.Hide();
            }
        }
    }


    public void PutBloqueRaiz(Bloque bloqueRaiz){
        if (!bloquesRaiz.Contains(bloqueRaiz))
        {
            bloquesRaiz.Add(bloqueRaiz);
        }
    }
    public void RemoveBloqueRaiz(Bloque bloqueRaiz){
        bloquesRaiz.Remove(bloqueRaiz);
    }
}
