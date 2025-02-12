using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueAccion : Bloque
{
    protected ActionableObject actionableObject;
    
    public override bool isConectable(Bloque other)
    {
        Debug.Log("BloqueAccion");
        if(other.GetComponent<BloqueArrastrable>().GetHasBeenPut()){
            Debug.Log("El bloque ya ha sido puesto");
            return other is BloqueAccion || other is BloqueControl || other is BloqueRaiz; 
        }
        Debug.Log("El bloque no ha sido puesto" + other.GetComponent<BloqueArrastrable>().GetHasBeenPut());
        return false;
    }
}
