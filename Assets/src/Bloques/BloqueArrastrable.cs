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
        detectarBloque.enabled = false;
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
        transform.position = position;
    }
    public void MoveChilds(Vector2 delta)
    {
        List<Bloque> list = bloque.GetBloquesConectados();
        if (list.Count != 0)
        {
            BloqueArrastrable bloque_aux = list[0].GetComponent<BloqueArrastrable>();
            bloque_aux.Move(delta);
        }
    }

    public void OnEndDrag()
    {   
        var targetGameObject = detectarBloque.GetBloqueEnContacto();

        if(detectarBloque.IsInBasura()){

            foreach(Bloque bloqueConectado in bloque.GetBloquesConectados()){
                Destroy(bloqueConectado.gameObject);
            }
            Destroy(this.gameObject);
            targetGameObject.GetComponent<Animator>().SetTrigger("use");
            return;
        }
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
                    Move(targetGameObject.transform.position - new Vector3(0,0,0.05f));
                    this.transform.parent = targetBloque.transform;
                    this.GetBloque().SetParent(targetBloque);
                }   
            }
            else if(detectarBloque.GetTipoContacto() == TipoContacto.ContactoInterno){
                Bloque targetBloque = targetGameObject.transform.parent.GetComponent<Bloque>();
                if(targetBloque is not BloqueControl){Debug.LogError("EL tipo de bloque no es control");}
                BloqueControl bloqueControl = (BloqueControl) targetBloque;
                if(bloque.ConnectTo(bloqueControl)){
                    Move(targetGameObject.transform.position);
                    MoveConnectedBlocks(this.transform.localPosition);
                    foreach(Bloque bloque in bloque.GetBloquesConectados()){
                        bloque.transform.parent = bloqueControl.transform;
                    }
                }
            }
        }
    }
    public void MoveConnectedBlocks(Vector3 parentPosition){
        Bloque bloque = GetComponent<Bloque>(); 
        
        if(!bloque.HasChild()){return;}

        float offsetX = GetComponent<Transform>().localScale.x * 1.1f;
        if(PrimerHijoControl(bloque.GetBloquesConectados()[0])){
            BloqueControl bloqueControl = bloque as BloqueControl; 
            int numBloques = bloqueControl.GetBloquesDentro().Count + bloqueControl.GetExtra();
            if(numBloques == 0){
                offsetX += 0.3f;
            }else{
                offsetX = offsetX + 0.55f + (offsetX * 1.02f) * (numBloques- 1);
            }
        }
        Bloque bloqueHijo = bloque.GetBloquesConectados()[0];
        Transform transformHijo = bloqueHijo.transform;
        if(transformHijo.parent.GetComponent<Bloque>() != null){
            offsetX = -offsetX;    
        }
        Vector3 nuevaPosicion = new Vector3(parentPosition.x + offsetX, parentPosition.y , parentPosition.z );
        transformHijo.localPosition = nuevaPosicion;
        transformHijo.gameObject.GetComponent<BloqueArrastrable>().MoveConnectedBlocks(nuevaPosicion);
    }

    private bool PrimerHijoControl(Bloque hijo){
        return !hijo.HasBloqueControl() && hijo.GetParent() is BloqueControl;
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
