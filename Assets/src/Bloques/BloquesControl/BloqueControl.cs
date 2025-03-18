using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueControl : Bloque
{
    [SerializeField] protected BloqueCondicion condicion;
    [SerializeField] protected List<Bloque> bloquesDentro; 
    [SerializeField] private Transform lateral;
    [SerializeField] private Transform centro;
    [SerializeField] private BoxCollider colliderInterno;
    [SerializeField] private BoxCollider colliderCondicion;
    private BoxCollider[] colliders;
    private BoxCollider colliderLateral;
    private Vector3 normalSize;
    private int incrementoExtra;
    protected List<Coroutine> coroutines = new List<Coroutine>();

    private void Start() {
        base.Start();
        bloquesDentro = new List<Bloque>();
        normalSize = centro.localScale; 
        colliderInterno.enabled = true;
        incrementoExtra = 0;
        colliders = GetComponents<BoxCollider>();
        colliderLateral = colliders[1];
    }

    public int GetExtra(){
        return incrementoExtra;
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
        bloquesDentro.InsertRange(index, childs);
        foreach (Bloque bloque in childs)
        {
            bloque.transform.parent = this.transform;
            AjustarCollider(bloque, true);
        }
        ActualizarTamaño();
        return true;
    }
    public void RemoveBloqueDentro(Bloque child){
        List<Bloque> list = child.GetBloquesConectados();
        AjustarCollider(child, false);
        bloquesDentro.Remove(child);
        foreach(Bloque bloque in list){
            AjustarCollider(bloque, false);
            bloquesDentro.Remove(bloque);
            ActualizarTamaño();
        }
    }
    
    public List<Bloque> GetBloquesDentro(){
        return bloquesDentro;
    } 
    public override bool isConectable(Bloque other){
        return other is BloqueControl || other is BloqueAccion || other is BloqueRaiz; 
    }
    public void IncrementarTamano(int value){
        incrementoExtra = value;
        ActualizarTamaño();
    }
    private void ActualizarTamaño(){
        int numBloques = bloquesDentro.Count + incrementoExtra;
        if (numBloques == 0){
            centro.localScale = normalSize;
            colliderInterno.enabled = true;
        }else{
            const float ancho = 275;
            float nuevoAncho = (ancho * numBloques);
            centro.localScale = new Vector3(nuevoAncho, normalSize.y, normalSize.z);
        }
        colliderInterno.enabled = bloquesDentro.Count == 0;

        if (colliderLateral != null){
            Vector3 desplazamiento = lateral.localPosition;
            lateral.localPosition = new Vector3(CalcularNuevaPosicionLateral(), lateral.localPosition.y, lateral.localPosition.z);
            colliderLateral.center += new Vector3((lateral.localPosition - desplazamiento).x * 0.5f, 0, 0);
        }
        
        GetComponent<BloqueArrastrable>().MoveConnectedBlocks(this.transform.localPosition);
        CentrarCondicion();
        ControlarTextura();
    }
    private float CalcularNuevaPosicionLateral(){
        return lateral.parent.InverseTransformPoint(centro.GetComponent<Renderer>().bounds.max).x;
    }
    private void CentrarCondicion(){
        float centroRealX = centro.GetComponent<Renderer>().bounds.center.x;
        Transform transformCollider = colliderCondicion.transform;
        transformCollider.position = new Vector3(centroRealX, transformCollider.position.y, transformCollider.position.z);
        if(condicion != null){
            condicion.GetComponent<BloqueArrastrable>().Move(transformCollider.position - new Vector3(0,0,0.05f));
        }
    }

    private void ControlarTextura(){
        Renderer renderer = centro.GetComponent<Renderer>();
        Material material = renderer.material;
        Vector2 tiling = material.mainTextureScale;
        tiling.x = centro.localScale.x / normalSize.x;
        material.mainTextureScale = tiling;
        material.mainTexture.wrapMode = TextureWrapMode.Clamp;
        const float C = 0.67f;
        float offsetX = -C * tiling.x + C; 
        material.mainTextureOffset = new Vector2(offsetX, 0f);
    }

    private void AjustarCollider(Bloque bloque, bool agregar){
        float ajusteX = agregar ? -0.4f : 0.4f;
        float ajusteY = agregar ? 0.2f : -0.2f;
        
        var collider = bloque.GetComponent<BoxCollider>();
        collider.center += new Vector3(0, ajusteY, 0);
        collider.size += new Vector3(ajusteX, 0, 0);
    }

    public bool SetBloqueCondicion(BloqueCondicion bloqueCondicion){
        if(condicion){return false;}
        condicion = bloqueCondicion;
        return true;
    }
    public void SetNullBloqueCondicion(){
        condicion = null;
    }

    public void StopAllActions()
    {
        foreach (Coroutine c in coroutines)
        {
            StopCoroutine(c);
        }
        coroutines.Clear();
    }
}
