using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueControl : Bloque
{
    protected BloqueCondicion condicion;//Sacar la forma de añadir aqui
    public List<Bloque> bloquesDentro = new List<Bloque>(); //Sacar la forma de añadir aqui
    public override bool isConectable(Bloque other)
    {
        return other is BloqueControl || other is BloqueAccion || other is BloqueRaiz; 
    }
}
