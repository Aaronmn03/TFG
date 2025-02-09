using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueRaiz : Bloque
{
    public override bool isConectable(Bloque other){
        return false;
    }

}
