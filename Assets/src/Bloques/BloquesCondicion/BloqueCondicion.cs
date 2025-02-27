using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueCondicion : Bloque, IConnectable
{
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

    public bool SetBloque2(BloqueVariable bloqueVariable){
        if(bloque2){return false;}
        bloque2 = bloqueVariable;
        return true;
    }

    public bool ConnectTo(Bloque parent, int index){
        return false;
    }
    public void UnConnectTo(Bloque parent, int index){
    }

}
