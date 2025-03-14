using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public abstract class Bloque : MonoBehaviour
{
    public List<Bloque> bloquesConectados = new List<Bloque>();
    public Bloque parent;
    protected Nivel nivel;
    protected ProgramableObject programableObject;

    [SerializeField] protected BloqueControl bloqueControl;
    public abstract IEnumerator Action();

    public abstract bool isConectable(Bloque other);

    public virtual bool isConectable(Bloque other, TipoContacto tipoContacto){
        return isConectable(other);
    }

    protected void Start(){
        nivel = GameObject.Find("LevelHandler").GetComponent<Nivel>();
        gameObject.AddComponent<BloqueArrastrable>();
    }
    public List<Bloque> GetBloquesConectados(){
        return bloquesConectados;
    }
    public Bloque GetParent(){
        return parent;
    }
    public void SetParent(Bloque value){
        parent = value;
    }
    public void SetProgramableObject(ProgramableObject programableObject){
        this.programableObject = programableObject;
    }

    public void SetBloqueControl(BloqueControl bloqueControl){
        this.bloqueControl = bloqueControl;
    }
    public bool HasBloqueControl(){
        return bloqueControl != null;
    }

    public BloqueControl GetBloqueControl(){
        return bloqueControl;
    }

    public ZonaProgramacion GetZonaProgramacion(){
        return programableObject.GetZonaProgramacion();
    }

    private void CheckBloqueControl(Bloque parent){
        if(parent.HasBloqueControl()){
            this.SetBloqueControl(parent.GetBloqueControl());
            
        }
    }
    public bool ConnectTo(Bloque parent){
        if(isConectable(parent) && parent.bloqueControl != this){
            List<Bloque> bloquesHijos = new List<Bloque>(parent.GetBloquesConectados());
            List<Bloque> bloquesConectar = new List<Bloque>(bloquesConectados);
            bloquesConectar.Insert(0, this);
            if (!parent.AddBloques(0, bloquesConectar)){return false;} 
            this.parent = parent;
            CheckBloqueControl(parent);
            if(bloquesHijos.Count != 0){
                foreach (Bloque bloque in this.bloquesConectados){
                    bloque.GetBloquesConectados().AddRange(bloquesHijos);
                }
                this.bloquesConectados.AddRange(bloquesHijos);
                if(this.HasChild()){
                    bloquesHijos[0].parent = bloquesConectar.Last();  
                }else{
                    bloquesHijos[0].parent = this;
                }
            }
            return true;
        }
        return false;       
    }

    public bool ConnectTo(BloqueControl bloqueControl){
        if(isConectable(bloqueControl)){
            List<Bloque> bloquesDentro = new List<Bloque>(bloqueControl.GetBloquesDentro());
            List<Bloque> bloquesConectar = new List<Bloque>(bloquesConectados);
            bloquesConectar.Insert(0, this);
            this.SetBloqueControl(bloqueControl);
            if (!bloqueControl.AddBloques(0, bloquesConectar)){return false;} 
            this.parent = bloqueControl;
            foreach(Bloque bloque in bloquesConectados){
                bloque.SetBloqueControl(bloqueControl);
            }
            if(bloquesDentro.Count != 0){
                foreach (Bloque bloque in this.bloquesConectados){
                    bloque.GetBloquesConectados().AddRange(bloquesDentro);
                }
                this.bloquesConectados.AddRange(bloquesDentro);
                if(this.HasChild()){
                    bloquesDentro[0].parent = bloquesConectar.Last();  
                }else{
                    bloquesDentro[0].parent = this;
                }
            }
            return true;
        }
        return false;   
    }

    public virtual void DisConnectTo(Bloque parent){  
        if (parent.GetBloquesConectados().Contains(this)){
            parent.RemoveBloque(this);
            this.SetParent(null);
        }
        if(HasBloqueControl()){
            bloqueControl.RemoveBloqueDentro(this);
            foreach(Bloque bloque in bloquesConectados){
                bloque.transform.parent = bloqueControl.transform.parent;
                bloque.SetBloqueControl(null);
            }
            transform.parent = bloqueControl.transform.parent;
            this.SetParent(null);
            BloqueArrastrable bloqueControlArrastrable = bloqueControl.GetComponent<BloqueArrastrable>();
            bloqueControlArrastrable.MoveConnectedBlocks(bloqueControlArrastrable.transform.localPosition);
            SetBloqueControl(null);
        }
    }

    public virtual bool AddBloques(int index, List<Bloque> childs){
        foreach (Bloque bloque in childs)
        {
            if (bloque == this)
            {
                Debug.LogWarning($"El bloque {bloque.name} es el mismo que el bloque actual, no se agregará.");
                return false;
            }

            if (bloquesConectados.Contains(bloque))
            {
                Debug.LogWarning($"El bloque {bloque.name} ya está conectado, no se agregará.");
                return false;
            }
        }
        bloquesConectados.InsertRange(index, childs);
        if (this.HasParent()){
            if(parent == bloqueControl){
                return bloqueControl.AddBloques(index + 1, childs);
            }else{
                return this.parent.AddBloques(index + 1, childs);
            }
        }else{
            return true;
        }
    }
    public virtual void RemoveBloque(Bloque child){
        List<Bloque> list = child.GetBloquesConectados();
        this.GetBloquesConectados().Remove(child);
        foreach(Bloque bloque_aux in list){
            this.GetBloquesConectados().Remove(bloque_aux);
        }
        if(this.HasParent()){
            this.parent.RemoveBloque(child);
        }
    }
    public bool HasParent(){
        return parent != null;
    }

    public bool HasChild(){
        return bloquesConectados.Count != 0;
    }
    public void Visibility(bool visible){
        MeshRenderer meshRenderer = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null){
            meshRenderer.enabled = visible;
        }
    }
}

public interface IConnectable
{
    int Index { get; set; }
    bool ConnectTo(Bloque parent, int index);
    void DisConnectTo(Bloque parent);
}
