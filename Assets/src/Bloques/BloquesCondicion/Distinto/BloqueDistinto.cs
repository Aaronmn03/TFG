using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueDistinto : BloqueCondicion
{
    public override IEnumerator Action (){
        if (!ValidarBloques()) yield break;
        object valor1 = bloque1.GetValor();
        object valor2 = bloque2.GetValor();
        while (EsValorNulo(valor1) || EsValorNulo(valor2))
        {
            valor1 = bloque1.GetValor();
            valor2 = bloque2.GetValor();
            yield return null;
        }
        resultado = !CalculateResultado(valor1, valor2);
        yield return null; 
    }
}
