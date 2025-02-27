using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BloqueArrastrable : MonoBehaviour
{
    private Bloque bloque;
    private float movementSpeed = 2.5f;
    public ObjectManipulator objectManipulator;
    private DetectarBloque detectarBloque;
    private Color emissionColor; 
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
        emissionColor = GetComponentInChildren<Renderer>().material.GetColor("_EmissionColor");
    }
    public void Move(Vector2 delta){
        transform.localPosition = transform.localPosition - new Vector3(delta.x * movementSpeed,0 , delta.y * movementSpeed);
        MoveChilds(delta);
    }

    public void Move(Vector3 position){
        transform.position = position - new Vector3(0,0,0.0075f);
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
        var targetGameObject = detectarBloque.GetBloqueEnContacto();
        if(targetGameObject != null){
            if(detectarBloque.GetTipoContacto() == TipoContacto.ContactoNormal){   
                Bloque targetBloque = targetGameObject.GetComponent<Bloque>();
                if(bloque.ConnectTo(targetBloque)){
                    Vector3 targetTransform = targetBloque.GetComponent<Transform>().localPosition;
                    BloqueArrastrable targetArrastrable = targetBloque.GetComponent<BloqueArrastrable>();
                    targetArrastrable.MoveConnectedBlocks(targetTransform);
                    targetArrastrable.NoBrillar();
                    this.NoBrillar();

                }
            }else if(detectarBloque.GetTipoContacto() == TipoContacto.ContactoPosicional){   
                Bloque targetBloque = targetGameObject.transform.parent.GetComponent<Bloque>();
                IConnectable connectableBloque = bloque as IConnectable;
                if(connectableBloque.ConnectTo(targetBloque, detectarBloque.GetIndex())){
                    Move(targetGameObject.transform.position);
                    this.transform.parent = targetBloque.transform;
                    this.GetBloque().SetParent(targetBloque);
                }   
            }
        }
    }
    public void MoveConnectedBlocks(Vector3 parentPosition){
        Bloque bloque = this.GetComponent<Bloque>(); 
        if(!bloque.HasChild()){return;}
        float offsetX = GetComponent<Transform>().localScale.x * 1.1f;
        Transform transformHijo = bloque.getListConectados()[0].transform;
        Vector3 nuevaPosicion = new Vector3(parentPosition.x + offsetX, parentPosition.y , parentPosition.z );
        transformHijo.localPosition = nuevaPosicion;
        transformHijo.transform.parent.rotation = transform.parent.rotation;
        transformHijo.gameObject.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(nuevaPosicion);
    }

    public void Brillar(){
        Color newEmissionColor = emissionColor * 2;
        Material material = GetComponentInChildren<Renderer>().material;
        material.SetColor("_EmissionColor", newEmissionColor);
    }

    public void NoBrillar(){
        Color newEmissionColor = emissionColor;
        Material material = GetComponentInChildren<Renderer>().material;
        material.SetColor("_EmissionColor", newEmissionColor);
    }
}
