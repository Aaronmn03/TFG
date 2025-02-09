using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BloqueRotar : BloqueAccion{
    public override IEnumerator Action(){
        actionableObject = FindObjectOfType<ActionableObject>();
        actionableObject.RotateRight();
        while (actionableObject.IsMoving()) {
            yield return null; // Espera un frame y vuelve a comprobar
        }
    }
}
