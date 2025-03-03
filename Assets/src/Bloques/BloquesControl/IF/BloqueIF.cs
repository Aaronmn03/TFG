using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueIF : BloqueControl
{
    public override IEnumerator Action() {
        yield return StartCoroutine(condicion.Action());
        if(condicion.ObtenerResultado()){
            Debug.Log("Ejecutamos el if");
            foreach (Bloque bloque in bloquesDentro) {
                yield return StartCoroutine(bloque.Action());
            }
        }else{
            Debug.Log("No ejecutamos el if");
        }
    }
}
