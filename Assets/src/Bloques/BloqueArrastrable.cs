using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BloqueArrastrable : MonoBehaviour
{
    private Bloque bloque;
    private float movementSpeed = 2.5f;
    public ObjectManipulator objectManipulator;

    private DetectarBloque detectarBloque;

    public Bloque GetBloque(){
        return bloque;
    }

    private void Awake() {
        detectarBloque = gameObject.AddComponent<DetectarBloque>();
        bloque = GetComponent<Bloque>();
        objectManipulator = GameObject.Find("ARManipulator").GetComponent<ObjectManipulator>();
        bloque.SetProgramableObject(objectManipulator.GetProgramableObject());
        if(bloque is BloqueRaiz){
            BloqueRaiz bloqueRaiz = (BloqueRaiz) bloque;
            bloqueRaiz.PutBloque();
        }
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

    public void OnEndDrag()
    {        
        var targetBloque = detectarBloque.GetBloqueEnContacto();
        if(targetBloque != null){
            if(bloque.ConnectTo(targetBloque)){
                Vector3 targetTransform = targetBloque.GetComponent<Transform>().localPosition;
                targetBloque.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(targetTransform);
            }
        }
    }
    public void MoveConnectedBlocks(Vector3 parentPosition){
        float offsetX = GetComponent<Transform>().localScale.x * 1.1f;
        List<Bloque> bloquesConectados = this.GetComponent<Bloque>().getListConectados();
        for (int i = 0; i < bloquesConectados.Count; i++)
        {
            Bloque bloqueHijo = bloquesConectados[i];
            Transform transformHijo = bloqueHijo.GetComponent<Transform>();
            Vector3 nuevaPosicion = new Vector3(parentPosition.x + (offsetX * (i + 1)), parentPosition.y , parentPosition.z );
            transformHijo.localPosition = nuevaPosicion;
            transformHijo.transform.parent.rotation = transform.parent.rotation;
            bloqueHijo.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(nuevaPosicion);
        }
    }
}
