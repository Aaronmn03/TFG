using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueControl : Bloque
{
    public override bool isConectable(Bloque other)
    {
        if(other.GetComponent<BloqueArrastrable>().GetHasBeenPut()){
            return other is BloqueControl || other is BloqueAccion || other is BloqueRaiz; 
        }
        return false;
    }

}
