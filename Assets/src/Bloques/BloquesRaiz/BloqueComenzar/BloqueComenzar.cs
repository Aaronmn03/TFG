using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueComenzar : BloqueRaiz {
    public override IEnumerator Action() {
        foreach (Bloque bloque in bloquesConectados) {
            Coroutine c = StartCoroutine(bloque.AccionConjunta());
            coroutines.Add(c);
            yield return c;
        }
        programableObject.GetZonaProgramacion().GetComponent<ZonaProgramacion>().TerminarEjecucion();
    }
}
