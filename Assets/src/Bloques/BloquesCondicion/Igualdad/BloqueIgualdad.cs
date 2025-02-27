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
        resultado = valor1.Equals(valor2);
        yield return null; 
    }
}
