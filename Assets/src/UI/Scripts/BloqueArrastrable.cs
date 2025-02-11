using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BloqueArrastrable : MonoBehaviour, IBeginDragHandler,  IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Bloque bloque;
    private bool hasBeenPut;
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

    private void Awake() {
        hasBeenPut = false;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        bloque = GetComponent<Bloque>();
        //shadow = transform.GetChild(1).gameObject;
        objectManipulator = GameObject.Find("ARManipulator").GetComponent<ObjectManipulator>();
    }

    public void Move(Vector2 delta){
        transform.position = transform.position - new Vector3(delta.x * movementSpeed, delta.y * movementSpeed, 0);
        MoveChilds(delta);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!objectManipulator.GetIsARObjectSelected()){
            Debug.Log("No se ha seleccionado ningun objeto");
            return;
        }
        if (!hasBeenPut)
        {
            GameObject duplicatedObject = Instantiate(gameObject, gameObject.transform.parent.transform.parent);
            duplicatedObject.name = gameObject.name; 
            RectTransform duplicatedRectTransform = duplicatedObject.GetComponent<RectTransform>();
            duplicatedRectTransform.position = gameObject.transform.position;
            duplicatedObject.transform.localScale = gameObject.transform.localScale * 2;
            eventData.pointerDrag = duplicatedObject;
            duplicatedObject.GetComponent<Bloque>().SetProgramableObject(objectManipulator.getARObject());
            BloqueArrastrable duplicatedScript = duplicatedObject.GetComponent<BloqueArrastrable>();
            duplicatedScript.canvasGroup = duplicatedObject.GetComponent<CanvasGroup>();
            duplicatedScript.canvasGroup.alpha = 0.75f;
            duplicatedScript.canvasGroup.blocksRaycasts = false;
            return;
        }
        canvasGroup.alpha = 0.75f;
        canvasGroup.blocksRaycasts = false;

        if (bloque.HasParent())
        {
            bloque.GetParent().UnConnectTo(bloque);
        }
    }


    public void MoveChilds(Vector2 delta)
    {
        List<Bloque> list = bloque.getListConectados();
        if (list.Count != 0)
        {
            BloqueArrastrable bloque_aux = list[0].GetComponent<BloqueArrastrable>();
            bloque_aux.MoveChilds(delta);
        }

        //Y esto debera hacerse con un collider
        /*var targetBloque = eventData.pointerCurrentRaycast.gameObject.GetComponent<Bloque>();
        if (targetBloque != null && bloque.isConectable(targetBloque))
        {
            //targetBloque.GetComponent<BloqueArrastrable>().shadow.SetActive(true);
            //Pon aqui ps que cambie un poco el color del material o algo.
        }*/
    }

    /*public void OnPointerExit(PointerEventData eventData)
    {
        if (shadow != null) {
            shadow.SetActive(false);
        }
    }*/

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
    public void OnEndDrag(PointerEventData eventData)
    {
        hasBeenPut = true;
        //HAY QUE HACER ESTO MISMO PERO CON UN COLLIDER
        /*
        var targetBloque = eventData.pointerCurrentRaycast.gameObject.GetComponent<Bloque>();
        if (targetBloque != null && targetBloque != bloque)
        {
            if(bloque.ConnectTo(targetBloque)){
                RectTransform targetRectTransform = targetBloque.GetComponent<RectTransform>();
                Vector2 targetPosition = new Vector2(targetRectTransform.anchoredPosition.x, targetRectTransform.anchoredPosition.y);
                MoveTargetBlocks(targetBloque, targetPosition);
            }
        }*/
        var targetBloque = BloqueInContact.GetComponent<Bloque>();
        if(targetBloque != null){
            if(bloque.ConnectTo(targetBloque)){
                Vector3 targetTransform = targetBloque.GetComponent<Transform>().position;
                MoveTargetBlocks(targetBloque, targetTransform);
            }
        }
    }
    public void MoveTargetBlocks(Bloque targetbloque,Vector3 position){
        float offsetY = GetComponent<Transform>().localScale.y * 2;
        List<Bloque> bloquesConectados = targetbloque.getListConectados();
        for (int i = 0; i < bloquesConectados.Count; i++)
        {
            Bloque bloqueHijo = bloquesConectados[i];
            Transform transformHijo = bloqueHijo.GetComponent<Transform>();
            Vector3 nuevaPosicion = new Vector3(position.x, position.y - (offsetY * (i + 1)), position.z);
            transformHijo.position = nuevaPosicion;
            bloqueHijo.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(nuevaPosicion);
        }
    }

    //Con esto movemos el bloque que hayamos conectado a la posicion esa asi como si ese objeto tiene hijos a su posicion
    public void MoveConnectedBlocks(Vector3 parentPosition){
        float offsetY = GetComponent<Transform>().localScale.y * 2;
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
