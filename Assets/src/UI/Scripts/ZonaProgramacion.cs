using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class ZonaProgramacion : MonoBehaviour
{
    public List<BloqueRaiz> bloquesRaiz;
    private void Start() {
        NonSelectedObject();
        bloquesRaiz = new List<BloqueRaiz>();
    }

    public List<BloqueRaiz> GetBloquesRaiz(){
        return bloquesRaiz;
    }
    public void NonSelectedObject(){
        GetComponent<MeshRenderer>().enabled = false;
        HideBloques(gameObject);
    }

    public void SelectedObject(){
        GetComponent<MeshRenderer>().enabled = true;
        ShowBloques(gameObject);
    }

    public void LimpiarBloques(){
        Destroy(this.transform.parent.gameObject);
    }

    public void Play(){
        if(bloquesRaiz.Count <= 0) return;
        foreach (BloqueRaiz bloque in bloquesRaiz){
            StartCoroutine(bloque.Action());
        }
    }


    private void HideBloques(GameObject parent)
    {
        SetChildrenVisibility(parent, false);
    }

    private void ShowBloques(GameObject parent)
    {
        SetChildrenVisibility(parent, true);
    }
    private void SetChildrenVisibility(GameObject parent, bool visible)
    {
        if (parent == null)
        {
            Debug.LogWarning("SetChildrenVisibility: parent is null.");
            return;
        }
        
        foreach (Transform child in parent.transform)
        {
            Bloque bloque = child.GetComponent<Bloque>();
            if (bloque != null)
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = visible;
                }
            }
        }
    }
}
