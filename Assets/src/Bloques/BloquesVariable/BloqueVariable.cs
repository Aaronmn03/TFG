using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueVariable : Bloque, IConnectable
{

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
            return bloque.SetBloque1(this);
        }else if(index == 2){
            return bloque.SetBloque2(this);
        }
        return false;
    }
    public void UnConnectTo(Bloque parent, int index){
        if(parent is not BloqueCondicion){return;}
        BloqueCondicion bloque = (BloqueCondicion) parent; 
        if(index == 1){
            bloque.SetBloque1(null);
        }else if(index == 2){
            bloque.SetBloque2(null);
        }
    }
}

