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
        transform.parent.GetComponent<MeshRenderer>().enabled = false;
        HideBloques();
    }

    public void SelectedObject(){
        transform.parent.GetComponent<MeshRenderer>().enabled = true;
        ShowBloques();
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
    private void HideBloques()
    {
        SetChildrenVisibility(false);
    }

    private void ShowBloques()
    {
        SetChildrenVisibility(true);
    }
    private void SetChildrenVisibility(bool visible)
    {
        foreach (Transform child in this.transform)
        {
            Bloque bloque = child.GetComponent<Bloque>();
            if (bloque != null)
            {
                bloque.Visibility(visible);
            }
        }
    }
}
