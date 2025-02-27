using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueIgualdad : BloqueCondicion
{
    [SerializeField] private BloqueVariable bloque1;
    [SerializeField] private BloqueVariable bloque2;
    public override bool EvaluarCondicion(){
        if (bloque1 == null || bloque2 == null)
        {
            nivel.Lose("Algun campo de la igualdad esta vacio");
        }
        if (bloque1.GetType() != bloque2.GetType()){
            nivel.Lose("Las clases de la concicion no son los mismos");
        }
        object valor1 = StartCoroutine(bloque1.Action());
        object valor2 = StartCoroutine(bloque2.Action());
        return valor1.Equals(valor2); 
    }

}
