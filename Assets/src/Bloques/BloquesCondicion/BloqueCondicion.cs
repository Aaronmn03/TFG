using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueCondicion : Bloque, IConnectable
{
    private int index;
    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    [SerializeField] protected BloqueVariable bloque1;
    [SerializeField] protected BloqueVariable bloque2;
    [SerializeField] protected bool resultado = false;
    public override bool isConectable(Bloque other)
    {
        return other is BloqueControl || other is BloqueAccion || other is BloqueRaiz; 
    }

    public bool ObtenerResultado()
    {
        return resultado; 
    }

    public bool SetBloque1(BloqueVariable bloqueVariable){
        if(bloque1){return false;}
        bloque1 = bloqueVariable;
        return true;
    }

    public void NullBloque1(){
        bloque1 = null;
    }

    public bool SetBloque2(BloqueVariable bloqueVariable){
        if(bloque2){return false;}
        bloque2 = bloqueVariable;
        return true;
    }
    public void NullBloque2(){
        bloque2 = null;
    }

    public bool ConnectTo(Bloque parent, int index){
        if(parent is not BloqueControl){return false;}
        BloqueControl bloque = (BloqueControl) parent; 
        if(index == 1){
            this.index = index;
            return bloque.SetBloqueCondicion(this);
        }
        return false;
    }
    public override void DisConnectTo(Bloque parent){
        if(parent is not BloqueControl){return;}
        BloqueControl bloque = (BloqueControl) parent; 
        this.transform.parent = parent.transform.parent;
        this.SetParent(null);
        if(index == 1){
            bloque.SetNullBloqueCondicion();
        }
        this.transform.position = this.transform.position + new Vector3(0,0,0.0075f);
    }
}
