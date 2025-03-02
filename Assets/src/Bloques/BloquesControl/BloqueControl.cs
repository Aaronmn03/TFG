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
    [SerializeField] private BoxCollider collider;
    private Vector3 normalSize;

    private void Start() {
        base.Start();
        bloquesDentro = new List<Bloque>();
        normalSize = centro.localScale; 
        collider.enabled = true;
    }
    private void Update() {
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
            return this.parent.AddBloques(index + 1, childs);
        }else{
            return true;
        }
    }

    public void RemoveBloqueDentro(Bloque child){
        List<Bloque> list = child.getListConectados();
        bloquesDentro.Remove(child);
        foreach(Bloque bloque_aux in list){
            Debug.Log($"Eliminamos {bloque_aux.gameObject.name}");
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
    private void ActualizarTamaño(){
        int numBloques = bloquesDentro.Count; 
        if(numBloques == 0){
            centro.localScale = normalSize;
            collider.enabled = true;
        }
        else {
            collider.enabled = false;
            float ancho = 300;
            float nuevoAncho = (ancho * numBloques);
            centro.localScale = new Vector3(nuevoAncho, normalSize.y, normalSize.z);
        }
        Bounds centroBounds = centro.GetComponent<Renderer>().bounds;
        Vector3 nuevaPosicionLateral = centroBounds.max; 
        lateral.position = new Vector3(nuevaPosicionLateral.x, lateral.position.y, lateral.position.z);
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
