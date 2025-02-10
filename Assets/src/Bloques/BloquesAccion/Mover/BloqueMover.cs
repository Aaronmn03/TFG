using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BloqueMover : BloqueAccion{

    public override IEnumerator Action(){
        actionableObject = FindObjectOfType<ActionableObject>();
        actionableObject.MoveForward();
        while (actionableObject.IsMoving()) {
            yield return null;
        }
    }
}
