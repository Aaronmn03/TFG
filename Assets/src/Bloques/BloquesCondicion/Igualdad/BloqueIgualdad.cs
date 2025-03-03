using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueIgualdad : BloqueCondicion
{
    public override IEnumerator Action (){
        if (bloque1 == null || bloque2 == null)
        {
            nivel.Lose("Algun campo de la igualdad esta vacio");
        }
        if (bloque1.GetType() != bloque2.GetType()){
            nivel.Lose("Las clases de la concicion no son los mismos");
        }
        object valor1 = bloque1.GetColor();
        object valor2 = bloque2.GetColor();
        while (EsColorNegro(valor1) || EsColorNegro(valor2))
        {
            Debug.Log("Esperando a que los valores cambien...");
            valor1 = bloque1.GetColor();
            Debug.Log(valor1);
            valor2 = bloque2.GetColor();
            Debug.Log(valor2);
            yield return null;
        }
        Debug.Log("El valor1 es : " + valor1);
        Debug.Log("El valor2 es : " + valor2);
        resultado = valor1.Equals(valor2);
        Debug.Log("Hemos llegado a la igualdad y el resultado es: " + resultado);
        yield return null; 
    }

    private bool EsColorNegro(object valor)
    {
        if (valor is Color color)
        {
            return color == Color.clear;
        }
        return false;
    }
}
