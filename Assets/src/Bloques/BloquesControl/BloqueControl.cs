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
        foreach (Bloque bloque in childs)
        {
            bloque.transform.parent = this.transform;
            bloque.GetComponent<BoxCollider>().center += new Vector3(0, 0.2f, 0);
            bloque.GetComponent<BoxCollider>().size -= new Vector3(0.4f, 0, 0);
        }
        ActualizarTamaño();
        return true;
    }

    public void RemoveBloqueDentro(Bloque child){
        List<Bloque> list = child.GetBloquesConectados();
        child.GetComponent<BoxCollider>().center -= new Vector3(0, 0.2f, 0);
        child.GetComponent<BoxCollider>().size += new Vector3(0.4f, 0, 0);
        bloquesDentro.Remove(child);
        foreach(Bloque bloque in list){
            bloque.GetComponent<BoxCollider>().center -= new Vector3(0, 0.2f, 0);
            bloque.GetComponent<BoxCollider>().size += new Vector3(0.4f, 0, 0);
            bloquesDentro.Remove(bloque);
            ActualizarTamaño();
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
            colliderInterno.enabled = true;
        }
        else
        {
            if (bloquesDentro.Count != 0)
            {
                colliderInterno.enabled = false;
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

        BloqueArrastrable bloqueArrastrable = GetComponent<BloqueArrastrable>();
        bloqueArrastrable.MoveConnectedBlocks(bloqueArrastrable.transform.localPosition);
        CentrarCondicion();
        ControlarTextura();
    }

    private void CentrarCondicion(){
        float centroRealX = centro.GetComponent<Renderer>().bounds.center.x;
        Debug.Log("el centro es :" +  centroRealX);
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
    public bool SetBloqueCondicion(BloqueCondicion bloqueCondicion){
        if(condicion){return false;}
        condicion = bloqueCondicion;
        return true;
    }
    public void SetNullBloqueCondicion(){
        condicion = null;
    }
}
