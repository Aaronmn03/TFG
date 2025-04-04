using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueIF : BloqueControl
{
    public override IEnumerator Action() {
        nivel.IfUsed();
        if (condicion == null){
            nivel.Lose("Fallo de sintaxis, falta la condicion en el if");
            yield break;
        }
        Coroutine condCoroutine = StartCoroutine(condicion.AccionConjunta());
        coroutines.Add(condCoroutine);
        yield return condCoroutine;
        if(condicion.ObtenerResultado()){
            foreach (Bloque bloque in bloquesDentro) {
                Coroutine c = StartCoroutine(bloque.AccionConjunta());
                coroutines.Add(c);
                yield return c;
            }
        }
    }
}
