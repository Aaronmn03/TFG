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

    public Bloque GetBloque(){
        return bloque;
    }
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
        objectManipulator = GameObject.Find("ARManipulator").GetComponent<ObjectManipulator>();
        bloque.SetProgramableObject(objectManipulator.GetProgramableObject());
    }
    public void Move(Vector2 delta){
        transform.localPosition = transform.localPosition - new Vector3(delta.x * movementSpeed,0 , delta.y * movementSpeed);
        MoveChilds(delta);
    }
    public void MoveChilds(Vector2 delta)
    {
        List<Bloque> list = bloque.getListConectados();
        if (list.Count != 0)
        {
            BloqueArrastrable bloque_aux = list[0].GetComponent<BloqueArrastrable>();
            Debug.Log(bloque_aux);
            bloque_aux.Move(delta);
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
                Vector3 targetTransform = targetBloque.GetComponent<Transform>().localPosition;
                MoveTargetBlocks(targetBloque, targetTransform);
            }
        }
    }
    public void MoveTargetBlocks(Bloque targetbloque,Vector3 localPosition){
        float offsetZ = GetComponent<Transform>().localScale.y * 1.1f;
        List<Bloque> bloquesConectados = targetbloque.getListConectados();
        for (int i = 0; i < bloquesConectados.Count; i++)
        {
            Bloque bloqueHijo = bloquesConectados[i];
            Transform transformHijo = bloqueHijo.GetComponent<Transform>();
            Vector3 nuevaPosicion = new Vector3(localPosition.x, localPosition.y , localPosition.z - (offsetZ * (i + 1)));
            transformHijo.localPosition = nuevaPosicion;
            transformHijo.transform.parent.rotation = transform.parent.rotation;
            bloqueHijo.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(nuevaPosicion);
        }
    }

    //Con esto movemos el bloque que hayamos conectado a la posicion esa asi como si ese objeto tiene hijos a su posicion
    public void MoveConnectedBlocks(Vector3 parentPosition){
        float offsetZ = GetComponent<Transform>().localScale.y * 1.1f;
        List<Bloque> bloquesConectados = this.GetComponent<Bloque>().getListConectados();
        for (int i = 0; i < bloquesConectados.Count; i++)
        {
            Bloque bloqueHijo = bloquesConectados[i];
            Transform transformHijo = bloqueHijo.GetComponent<Transform>();
            Vector3 nuevaPosicion = new Vector3(parentPosition.x, parentPosition.y , parentPosition.z - (offsetZ * (i + 1)));
            transformHijo.localPosition = nuevaPosicion;
            bloqueHijo.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(nuevaPosicion);
        }
    }
}
