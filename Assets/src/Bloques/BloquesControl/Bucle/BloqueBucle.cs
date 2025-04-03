using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueBucle : BloqueControl
{
    public override IEnumerator Action() {
        //nivel.IfUsed();
        if (condicion == null){
            nivel.Lose("Fallo de sintaxis, falta la condicion en el bucle");
            yield break;
        }
        //Ejecutamos el contenido del bucle
        while(true){
            foreach (Bloque bloque in bloquesDentro) {
                Coroutine c = StartCoroutine(bloque.Action());
                coroutines.Add(c);
                yield return c;
            }
            //Hasta que la condicion no sea verdadera
            Coroutine condCoroutine = StartCoroutine(condicion.Action());
            coroutines.Add(condCoroutine);
            yield return condCoroutine;
            if(!condicion.ObtenerResultado()){
                yield break;
            }
        }
    }
}
