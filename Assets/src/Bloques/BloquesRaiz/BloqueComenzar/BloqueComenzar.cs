using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueComenzar : BloqueRaiz {
    public override IEnumerator Action() {
        programableObject.GetZonaProgramacion().GetComponent<ZonaProgramacion>().EmpezarEjecucion();
        foreach (Bloque bloque in bloquesConectados) {
            Coroutine c = StartCoroutine(bloque.AccionConjunta());
            coroutines.Add(c);
            yield return c;
        }
        programableObject.GetZonaProgramacion().GetComponent<ZonaProgramacion>().TerminarEjecucion();
    }
}
