using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueBotonPulsado : BloqueRaiz {

    private ObtenedorVariable boton;
    public void SetBoton(ObtenedorVariable boton) {
        this.boton = boton;
    }
    public override IEnumerator Action() {
        programableObject.GetZonaProgramacion().GetComponent<ZonaProgramacion>().EmpezarEjecucion();
        while (boton.GetValor().Equals(false) && programableObject.GetZonaProgramacion().GetEstaEjecutando()) {
            yield return null;
        }
        if(!programableObject.GetZonaProgramacion().GetEstaEjecutando()){
            yield break;
        }
        foreach (Bloque bloque in bloquesConectados) {
            Coroutine c = StartCoroutine(bloque.AccionConjunta());
            coroutines.Add(c);
            yield return c;
        }
        programableObject.GetZonaProgramacion().GetComponent<ZonaProgramacion>().TerminarEjecucion();
    }
}
