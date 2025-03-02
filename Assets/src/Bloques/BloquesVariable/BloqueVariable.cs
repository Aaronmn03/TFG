using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueVariable : Bloque, IConnectable
{
    private int index;
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    public abstract void SetValue(object value);

    public abstract Color GetColor();
    public override bool isConectable(Bloque other)
    {
        return other is BloqueControl || other is BloqueAccion || other is BloqueRaiz; 
    }

    public bool ConnectTo(Bloque parent, int index){
        if(parent is not BloqueCondicion){return false;}
        BloqueCondicion bloque = (BloqueCondicion) parent; 
        if(index == 1){
            this.index = index;
            return bloque.SetBloque1(this);
        }else if(index == 2){
            this.index = index;
            return bloque.SetBloque2(this);
        }
        return false;
    }
    public override void DisConnectTo(Bloque parent){
        if(parent is not BloqueCondicion){return;}
        BloqueCondicion bloque = (BloqueCondicion) parent; 
        this.transform.parent = parent.transform.parent;
        this.SetParent(null);
        if(index == 1){
            bloque.NullBloque1();
        }else if(index == 2){
            bloque.NullBloque2();
        }
        this.transform.position = this.transform.position + new Vector3(0,0,0.0075f);
    }
}

