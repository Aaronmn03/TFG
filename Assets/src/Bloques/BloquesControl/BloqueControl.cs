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
    private Vector3 normalSize;

    private void Start() {
        base.Start();
        bloquesDentro = new List<Bloque>();
        normalSize = centro.localScale; 
    }
    private void Update() {
        ActualizarTamaño();
    }
    public override bool isConectable(Bloque other)
    {
        return other is BloqueControl || other is BloqueAccion || other is BloqueRaiz; 
    }
    private void ActualizarTamaño(){
        int ancho = 100;
        int numBloques = bloquesDentro.Count; 
        if(numBloques == 0){
            centro.localScale = normalSize;
        }
        else {
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
