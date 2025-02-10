using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueComenzar : BloqueRaiz {
    public override IEnumerator Action() {
        foreach (Bloque bloque in bloquesConectados) {
            yield return StartCoroutine(bloque.Action());
        }
        nivel.Lose();
    }
}
