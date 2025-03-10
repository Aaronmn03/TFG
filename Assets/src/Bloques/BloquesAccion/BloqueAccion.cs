using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueAccion : Bloque
{
    protected ActionableObject actionableObject;
    
    public override bool isConectable(Bloque other)
    {
        return other is BloqueAccion || other is BloqueControl || other is BloqueRaiz; 
    }

    public override bool isConectable(Bloque other, TipoContacto tipoContacto)
    {
        switch (tipoContacto)
        {
            case TipoContacto.ContactoNormal:
                return other is BloqueAccion || other is BloqueControl || other is BloqueRaiz; 
                
            case TipoContacto.ContactoInterno:
                return other is BloqueControl;
                
            case TipoContacto.ContactoPosicional:
                return false;
                
            default:
                return false;
        }
    }
}
