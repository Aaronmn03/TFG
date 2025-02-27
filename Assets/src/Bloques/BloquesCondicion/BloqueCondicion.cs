using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueCondicion : Bloque
{
    public abstract bool EvaluarCondicion();
    public override bool isConectable(Bloque other)
    {
        return other is BloqueControl || other is BloqueAccion || other is BloqueRaiz; 
    }

}
