using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class ZonaProgramacion : MonoBehaviour
{
    public List<BloqueRaiz> bloquesRaiz;
    private Animator anim;
    private void Start() {
        anim = transform.parent.gameObject.GetComponent<Animator>();
        SelectedObject();
        bloquesRaiz = new List<BloqueRaiz>();
        transform.parent.LookAt(Camera.main.transform.position);
        Vector3 euler = transform.parent.eulerAngles;
        euler.z = -90;
        transform.parent.eulerAngles = euler;

    }

    public List<BloqueRaiz> GetBloquesRaiz(){
        return bloquesRaiz;
    }
    public void NonSelectedObject(){
        anim.SetBool("active",false);
        HideBloques();
    }

    public void SelectedObject(){
        anim.SetBool("active",true);
        ShowBloques();
    }

    public void LimpiarBloques(){
        Destroy(this.transform.parent.transform.parent.gameObject);
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
