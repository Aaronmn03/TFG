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
    public abstract bool isConectable(Bloque other);
    public abstract IEnumerator Action();

    protected void Start(){
        nivel = GameObject.Find("LevelHandler").GetComponent<Nivel>();
        gameObject.AddComponent<BloqueArrastrable>();
        Debug.Log(GetComponent<BloqueArrastrable>());
    }
    public List<Bloque> getListConectados(){
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

    private void CheckBloqueControl(Bloque parent){
        if(parent.HasBloqueControl()){
            this.SetBloqueControl(parent.GetBloqueControl());
            
        }
    }
    public bool ConnectTo(Bloque parent){
        if(isConectable(parent) && parent.bloqueControl != this){
            List<Bloque> bloquesHijos = new List<Bloque>(parent.getListConectados());
            List<Bloque> bloquesConectar = new List<Bloque>(bloquesConectados);
            bloquesConectar.Insert(0, this);
            if (!parent.AddBloques(0, bloquesConectar)){return false;} 
            this.parent = parent;
            CheckBloqueControl(parent);
            if(bloquesHijos.Count != 0){
                foreach (Bloque bloque in this.bloquesConectados){
                    bloque.getListConectados().AddRange(bloquesHijos);
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
            if(bloquesDentro.Count != 0){
                foreach (Bloque bloque in this.bloquesConectados){
                    bloque.getListConectados().AddRange(bloquesDentro);
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
        if (parent.getListConectados().Contains(this)){
            parent.RemoveBloque(this);
            this.SetParent(null);
        }
        if(HasBloqueControl()){
            Debug.Log("Tiene bloque Control vamos a sacarlo");
            bloqueControl.RemoveBloqueDentro(this);
            transform.parent = bloqueControl.transform.parent;
            SetBloqueControl(null);
            this.SetParent(null);
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
            return this.parent.AddBloques(index + 1, childs);
        }else{
            return true;
        }
    }
    public virtual void RemoveBloque(Bloque child){
        List<Bloque> list = child.getListConectados();
        this.getListConectados().Remove(child);
        foreach(Bloque bloque_aux in list){
            this.getListConectados().Remove(bloque_aux);
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
