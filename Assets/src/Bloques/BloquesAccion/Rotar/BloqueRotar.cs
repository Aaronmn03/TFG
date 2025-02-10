using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BloqueRotar : BloqueAccion{
    public override IEnumerator Action(){
        actionableObject = programableObject.GetComponent<ActionableObject>();
        actionableObject.RotateRight();
        while (actionableObject.IsMoving()) {
            yield return null;
        }
    }
}
