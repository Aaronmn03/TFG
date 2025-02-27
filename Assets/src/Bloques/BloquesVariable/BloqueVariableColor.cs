using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BloqueVariableColor : BloqueVariable
{
    public Color color;
    public override void SetValue(object value){
        if(value.GetType() != color.GetType()){Debug.LogError("El tipo de variable no coincide");}
        this.color = (Color)value; 
        Material newMaterial = new Material(Shader.Find("Standard")); 
        newMaterial.color = this.color; 
        Renderer childRenderer = transform.GetChild(1).GetComponent<Renderer>();
        childRenderer.material = newMaterial;
    }
    public override IEnumerator Action()
    {
        yield return color;
    }
}