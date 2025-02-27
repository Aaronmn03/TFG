using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueIF : BloqueControl
{
    public override IEnumerator Action() {
        if(condicion.EvaluarCondicion()){
            foreach (Bloque bloque in bloquesDentro) {
                yield return StartCoroutine(bloque.Action());
            }
        }
    }

}
