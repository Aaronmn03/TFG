using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueVariable : Bloque
{

    public abstract void SetValue(object value);
    public override bool isConectable(Bloque other)
    {
        return other is BloqueControl || other is BloqueAccion || other is BloqueRaiz; 
    }

}

