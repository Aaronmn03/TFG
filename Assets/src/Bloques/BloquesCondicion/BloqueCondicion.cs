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
        return other is BloqueControl;
    }

    public override bool isConectable(Bloque other, TipoContacto tipoContacto)
    {
        switch (tipoContacto)
        {
            case TipoContacto.ContactoNormal:
                return false; 
                
            case TipoContacto.ContactoInterno:
                return false;
                
            case TipoContacto.ContactoPosicional:
                return other is BloqueControl;
                
            default:
                return false;
        }
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

    protected bool ValidarBloques()
    {
        if (bloque1 == null || bloque2 == null)
        {
            nivel.Lose("Algún campo de la condicion esta vacío");
            return false;
        }
        if (bloque1.GetType() != bloque2.GetType())
        {
            nivel.Lose("Las clases de la condición no son las mismas");
            return false;
        }
        return true;
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

    protected bool EsValorNulo(object valor)
    {
        if (valor is Color color){
            return color == Color.clear;
        }else if(valor == null){
            return true;
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
        this.transform.position = this.transform.position + new Vector3(0,0,0.05f);
    }

    protected bool CalculateResultado(object valor1, object valor2){
        bool resultado;
        if (valor1 is Vector3 v1 && valor2 is Vector3 v2)
        {
            float tolerance = 0.09f;
            if (Vector3.Distance(v1, v2) < tolerance)
            {
                resultado = true;
            }
            else
            {
                resultado = false;
            }
        }else{
            resultado = valor1.Equals(valor2);
        }
        return resultado;
    }
}
