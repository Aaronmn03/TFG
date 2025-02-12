using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BloqueArrastrable : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Bloque bloque;
    [SerializeField] private bool hasBeenPut;
    private GameObject shadow;
    private GameObject BloqueInContact;
    private float movementSpeed = 2.5f;

    public ObjectManipulator objectManipulator;


    public GameObject GetShadow(){
        return shadow;
    }

    public bool GetHasBeenPut(){
        return hasBeenPut;
    }

    public void SetHasBeenPut(bool hasBeenPut){
        this.hasBeenPut = hasBeenPut;
    }

    private void Awake() {
        hasBeenPut = true;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        bloque = GetComponent<Bloque>();
        //shadow = transform.GetChild(1).gameObject;
        objectManipulator = GameObject.Find("ARManipulator").GetComponent<ObjectManipulator>();
    }

    private void Update() {
        Vector3 targetPosition = Camera.main.transform.position;
        targetPosition.y = transform.position.y; 
        transform.position = new Vector3(transform.position.x,transform.position.y,targetPosition.z + 0.5f);
        transform.LookAt(targetPosition);
    }

    public void Move(Vector2 delta){
        transform.position = transform.position - new Vector3(delta.x * movementSpeed, delta.y * movementSpeed, 0);
        MoveChilds(delta);
    }


    public void MoveChilds(Vector2 delta)
    {
        List<Bloque> list = bloque.getListConectados();
        if (list.Count != 0)
        {
            BloqueArrastrable bloque_aux = list[0].GetComponent<BloqueArrastrable>();
            bloque_aux.MoveChilds(delta);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
        {
            BloqueInContact = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
        {
            if(BloqueInContact == other.gameObject){
                BloqueInContact = null;
            }
        }
    }
    public void OnEndDrag()
    {        
        if(BloqueInContact == null){ return; }
        var targetBloque = BloqueInContact.GetComponent<Bloque>();
        if(targetBloque != null){
            if(bloque.ConnectTo(targetBloque)){
                Vector3 targetTransform = targetBloque.GetComponent<Transform>().position;
                MoveTargetBlocks(targetBloque, targetTransform);
            }
        }
    }
    public void MoveTargetBlocks(Bloque targetbloque,Vector3 position){
        float offsetY = GetComponent<Transform>().localScale.y * 1.2f;
        List<Bloque> bloquesConectados = targetbloque.getListConectados();
        for (int i = 0; i < bloquesConectados.Count; i++)
        {
            Bloque bloqueHijo = bloquesConectados[i];
            Transform transformHijo = bloqueHijo.GetComponent<Transform>();
            Vector3 nuevaPosicion = new Vector3(position.x, position.y - (offsetY * (i + 1)), position.z);
            transformHijo.position = nuevaPosicion;
            transformHijo.transform.parent.rotation = transform.parent.rotation;
            bloqueHijo.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(nuevaPosicion);
        }
    }

    //Con esto movemos el bloque que hayamos conectado a la posicion esa asi como si ese objeto tiene hijos a su posicion
    public void MoveConnectedBlocks(Vector3 parentPosition){
        float offsetY = GetComponent<Transform>().localScale.y * 1.2f;
        List<Bloque> bloquesConectados = this.GetComponent<Bloque>().getListConectados();
        for (int i = 0; i < bloquesConectados.Count; i++)
        {
            Bloque bloqueHijo = bloquesConectados[i];
            Transform transformHijo = bloqueHijo.GetComponent<Transform>();
            Vector3 nuevaPosicion = new Vector3(parentPosition.x, parentPosition.y - (offsetY * (i + 1)), parentPosition.z);
            transformHijo.position = nuevaPosicion;
            bloqueHijo.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(nuevaPosicion);
        }
    }
}
