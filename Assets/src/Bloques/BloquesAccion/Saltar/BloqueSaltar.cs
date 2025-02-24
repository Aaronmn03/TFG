using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BloqueSaltar : BloqueAccion{

    public override IEnumerator Action(){
        actionableObject = programableObject.GetComponent<ActionableObject>();
        actionableObject.Jump();
        while (actionableObject.IsMoving()) {
            yield return null;
        }
    }
}
