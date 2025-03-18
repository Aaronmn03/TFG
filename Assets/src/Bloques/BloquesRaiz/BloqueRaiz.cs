using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BloqueRaiz : Bloque
{
    protected List<Coroutine> coroutines = new List<Coroutine>();

    public override bool isConectable(Bloque other){
        return false;
    }
    
    public void PutBloque(){
        programableObject.PutBloqueRaiz(this);
    }

    public void RemoveBloque(){
        programableObject.RemoveBloqueRaiz(this);
    }
    public void StopAllActions()
    {
        foreach (Coroutine c in coroutines)
        {
            StopCoroutine(c);
        }
        coroutines.Clear();
    }
}
