using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueVariable : Bloque, IConnectable
{
    private int index;
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    public object valor;
    public ObtenedorVariable referencia;

    public override IEnumerator Action()
    {
        yield return null;
    }

    public void SetValue(object value){
        if (value is ObtenedorVariable referencia)
        {
            this.referencia = referencia;
            SetIcon(referencia);
            valor = null;
            return;
        }else{
            if(value is Color color){
                SetColor(color);
                this.valor = value;
            }else if(value is Texture2D texture2D){
                valor = 0;
                SetIcon(texture2D);
            }else{
                Debug.LogError("El valor no es un ObtenedorVariable o un Color o una textura");
                return;
            }
        }
    }

    public object GetValor(){
        if(referencia != null){
            return referencia.GetValor();
        }
        if (valor is int && (int) valor == 0)
        {
            return programableObject.transform.position;
        }
        return valor;
    }
    public override bool isConectable(Bloque other)
    {
        return other is BloqueCondicion; 
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
                return other is BloqueCondicion;
                
            default:
                return false;
        }
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
        this.transform.position = this.transform.position + new Vector3(0,0,0.05f);
    }

    private void SetIcon(ObtenedorVariable referencia){
        SetIcon(referencia.GetTexture2D());
    }

    private void SetIcon(Texture2D texture){
        Renderer childRenderer = transform.GetChild(1).GetComponent<Renderer>();
        Material newMaterial = childRenderer.material; 
        newMaterial.mainTexture = texture;
        childRenderer.material = newMaterial;
    }
    private void SetColor(Color color){
        Material newMaterial = new Material(Shader.Find("Standard")); 
        newMaterial.color = color; 
        Renderer childRenderer = transform.GetChild(1).GetComponent<Renderer>();
        childRenderer.material = newMaterial;
    }
}

