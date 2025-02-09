using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BloqueComenzar : BloqueRaiz{
    public override void Action(){
        foreach (Bloque bloque in bloquesConectados){
            bloque.Action();
        }
    }

}
