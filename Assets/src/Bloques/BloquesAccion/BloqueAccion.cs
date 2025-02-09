using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueAccion : Bloque
{
    protected ActionableObject actionableObject;
    
    public override bool isConectable(Bloque other)
    {
        if(other.GetComponent<BloqueArrastrable>().GetHasBeenPut()){
            return other is BloqueAccion || other is BloqueControl || other is BloqueRaiz; 
        }
        return false;
    }
}
