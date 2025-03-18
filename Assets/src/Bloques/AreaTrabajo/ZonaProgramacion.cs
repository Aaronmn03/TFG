using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class ZonaProgramacion : MonoBehaviour
{
    public List<BloqueRaiz> bloquesRaiz;
    private Animator anim;
    private bool estaEjecutando = false;
    private void Start() {
        anim = transform.parent.gameObject.GetComponent<Animator>();
        SelectedObject();
        bloquesRaiz = new List<BloqueRaiz>();
    }

    private void Update() {
        Transform cameraTransform = Camera.main.transform;
        Vector3 direction = cameraTransform.position - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Vector3 currentRotation = transform.parent.transform.rotation.eulerAngles;
        targetRotation = Quaternion.Euler(currentRotation.x, targetRotation.eulerAngles.y, currentRotation.z);
        transform.parent.transform.rotation = targetRotation;
    }

    public void TerminarEjecucion(){
        estaEjecutando = false;
        FindObjectOfType<Nivel>().Lose();
    }

    public bool GetEstaEjecutando(){
        return estaEjecutando;
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
        estaEjecutando = true;
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
