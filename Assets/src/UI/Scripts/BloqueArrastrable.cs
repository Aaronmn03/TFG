using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BloqueArrastrable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Bloque bloque;
    private bool hasBeenPut;
    private GameObject shadow;


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
        shadow = transform.GetChild(1).gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!hasBeenPut){
            GameObject duplicatedObject = Instantiate(gameObject);
            duplicatedObject.transform.SetParent(this.transform.parent);
            duplicatedObject.transform.localScale = Vector3.one;
            this.transform.SetParent(this.transform.parent.parent);
        }
        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
        if(bloque.HasParent()){
            bloque.GetParent().UnConnectTo(bloque);
        }
    }

public void OnDrag(PointerEventData eventData)
{
    Canvas canvas = GetComponentInParent<Canvas>();
    if (canvas == null) return; 
    Vector2 adjustedDelta = eventData.delta / canvas.scaleFactor;
    rectTransform.anchoredPosition += adjustedDelta;

    List<Bloque> list = bloque.getListConectados();
    if (list.Count != 0)
    {
        foreach (Bloque bloque_aux in list)
        {
            RectTransform rt = bloque_aux.GetComponent<RectTransform>();
            rt.anchoredPosition += adjustedDelta;
        }
    }
    var targetBloque = eventData.pointerCurrentRaycast.gameObject.GetComponent<Bloque>();
    if (targetBloque != null && bloque.isConectable(targetBloque))
    {
        targetBloque.GetComponent<BloqueArrastrable>().shadow.SetActive(true);
    }
}

    public void OnPointerExit(PointerEventData eventData)
    {
        if (shadow != null) {
            shadow.SetActive(false);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        hasBeenPut = true;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        var targetBloque = eventData.pointerCurrentRaycast.gameObject.GetComponent<Bloque>();
        if (targetBloque != null && targetBloque != bloque)
        {

            //Esto luego cambialo y pon primero la comprobacion de COnnectTo es solo para probar
            if(bloque.ConnectTo(targetBloque)){
                RectTransform targetRectTransform = targetBloque.GetComponent<RectTransform>();
                Vector2 targetPosition = new Vector2(targetRectTransform.anchoredPosition.x, targetRectTransform.anchoredPosition.y);
                MoveTargetBlocks(targetBloque, targetPosition);
            }
        }
    }
    public void MoveTargetBlocks(Bloque targetbloque,Vector2 position){
        float offsetY = GetComponent<RectTransform>().rect.height * 2;
        List<Bloque> bloquesConectados = targetbloque.GetComponent<Bloque>().getListConectados();
        for (int i = 0; i < bloquesConectados.Count; i++)
        {
            Bloque bloqueHijo = bloquesConectados[i];
            RectTransform rectTransformHijo = bloqueHijo.GetComponent<RectTransform>();
            Vector2 nuevaPosicion = new Vector2(position.x, position.y - (offsetY * (i + 1)));
            rectTransformHijo.anchoredPosition = nuevaPosicion;
            bloqueHijo.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(nuevaPosicion);
        }
    }

    //Con esto movemos el bloque que hayamos conectado a la posicion esa asi como si ese objeto tiene hijos a su posicion
    public void MoveConnectedBlocks(Vector2 parentPosition){
        float offsetY = GetComponent<RectTransform>().rect.height * 2;
        List<Bloque> bloquesConectados = this.GetComponent<Bloque>().getListConectados();
        for (int i = 0; i < bloquesConectados.Count; i++)
        {
            Bloque bloqueHijo = bloquesConectados[i];
            RectTransform rectTransformHijo = bloqueHijo.GetComponent<RectTransform>();

            Vector2 nuevaPosicion = new Vector2(parentPosition.x, parentPosition.y - (offsetY * (i + 1)));
            rectTransformHijo.anchoredPosition = nuevaPosicion;

            bloqueHijo.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(nuevaPosicion);
        }
    }




}
