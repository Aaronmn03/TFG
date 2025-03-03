using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BloqueVariableColor : BloqueVariable
{
    public Color color;
    public Semaforo referencia;

    public override void SetValue(object value)
    {
        if (value is Semaforo semaforo)
        {
            this.referencia = semaforo;
            SetIcon(semaforo);
            return;
        }
        if (value is Color colorValue)
        {
            SetColor(colorValue);
            return;
        }
        Debug.LogError("El tipo de variable no coincide");
    }

    public override Color GetColor(){
        if(color == Color.clear){
            color = referencia.GetColor();
            Debug.Log("El valor de ref es:" + color);
            return referencia.GetColor();
        }
        return color;
    }
    private void SetIcon(Semaforo semaforo){
        Renderer childRenderer = transform.GetChild(1).GetComponent<Renderer>();
        Material newMaterial = childRenderer.material; 
        newMaterial.mainTexture = semaforo.GetTexture2D();
        childRenderer.material = newMaterial;
        color = Color.clear;
    }

    private void SetColor(object value){
        this.color = (Color)value; 
        Material newMaterial = new Material(Shader.Find("Standard")); 
        newMaterial.color = this.color; 
        Renderer childRenderer = transform.GetChild(1).GetComponent<Renderer>();
        childRenderer.material = newMaterial;
    }
    public override IEnumerator Action()
    {
        yield return null;
    }
}