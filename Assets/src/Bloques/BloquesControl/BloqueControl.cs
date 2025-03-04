using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueControl : Bloque
{
    [SerializeField] protected BloqueCondicion condicion;
    public List<Bloque> bloquesDentro; 
    [SerializeField] private Transform lateral;
    [SerializeField] private Transform centro;
    [SerializeField] private BoxCollider colliderCentral;

    [SerializeField] private BoxCollider[] colliders;
    [SerializeField] private BoxCollider colliderLateral;
    private Vector3 normalSize;
    private int incrementoExtra;

    private void Start() {
        base.Start();
        bloquesDentro = new List<Bloque>();
        normalSize = centro.localScale; 
        colliderCentral.enabled = true;
        incrementoExtra = 0;
        colliders = GetComponents<BoxCollider>();
        colliderLateral = colliders[1];
    }

    public void IncrementarTamano(int value)
    {
        incrementoExtra = value;
        ActualizarTamaño();
    }

    public bool AddBloques(int index, List<Bloque> childs){
        foreach (Bloque bloque in childs)
        {
            if (bloque == this)
            {
                Debug.LogWarning($"El bloque {bloque.name} es el mismo que el bloque actual, no se agregará.");
                return false;
            }

            if (bloquesDentro.Contains(bloque))
            {
                Debug.LogWarning($"El bloque {bloque.name} ya está dentro, no se agregará.");
                return false;
            }
        }
        Debug.Log($"Intentando añadir {childs.Count} bloques en el índice {index}. Total de bloques antes de la inserción: {bloquesDentro.Count}, el bloque al que nos queremos conectar es: {this.gameObject.name}");
        bloquesDentro.InsertRange(index, childs);
        if (this.HasParent()){
            ActualizarTamaño();
            return this.parent.AddBloques(index + 1, childs);
        }else{
            ActualizarTamaño();
            return true;
        }
    }

    public void RemoveBloqueDentro(Bloque child){
        List<Bloque> list = child.GetListConectados();
        bloquesDentro.Remove(child);
        foreach(Bloque bloque_aux in list){
            bloquesDentro.Remove(bloque_aux);
        }
    }
    public List<Bloque> GetBloquesDentro(){
        return bloquesDentro;
    } 
    public override bool isConectable(Bloque other)
    {
        return other is BloqueControl || other is BloqueAccion || other is BloqueRaiz; 
    }
    private void ActualizarTamaño()
    {
        int numBloques = bloquesDentro.Count + incrementoExtra;

        if (numBloques == 0)
        {
            centro.localScale = normalSize;
            colliderCentral.enabled = true;
        }
        else
        {
            if (bloquesDentro.Count != 0)
            {
                colliderCentral.enabled = false;
            }
            float ancho = 275;
            float nuevoAncho = (ancho * numBloques);
            centro.localScale = new Vector3(nuevoAncho, normalSize.y, normalSize.z);
        }

        Bounds centroBounds = centro.GetComponent<Renderer>().bounds;
        Vector3 nuevaPosicionLateral = centroBounds.max;
        Vector3 nuevaPosicionLocal = lateral.parent.InverseTransformPoint(nuevaPosicionLateral);

        Vector3 posicionAnteriorLateral = lateral.localPosition;

        lateral.localPosition = new Vector3(nuevaPosicionLocal.x, lateral.localPosition.y, lateral.localPosition.z);
        if (colliderLateral != null)
        {
            Vector3 desplazamiento = lateral.localPosition - posicionAnteriorLateral;
            colliderLateral.center += new Vector3(desplazamiento.x * 0.5f, 0, 0);
        }
    }
    public bool SetBloqueCondicion(BloqueCondicion bloqueCondicion){
        if(condicion){return false;}
        condicion = bloqueCondicion;
        return true;
    }
    public void SetNullBloqueCondicion(){
        condicion = null;
    }
}
