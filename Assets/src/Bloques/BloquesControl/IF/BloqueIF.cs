using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueIF : BloqueControl
{
    [SerializeField] private BloqueCondicion condicion;
    public override IEnumerator Action() {
        if(condicion.ObtenerResultado()){
            foreach (Bloque bloque in bloquesDentro) {
                yield return StartCoroutine(bloque.Action());
            }
        }
    }

}
