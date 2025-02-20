using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueRotar : BloqueAccion{
    [SerializeField] private bool turnLeft;
    public override IEnumerator Action(){
        actionableObject = programableObject.GetComponent<ActionableObject>();
        if(turnLeft){
            actionableObject.RotateLeft();
        }else{
            actionableObject.RotateRight();
        }
        while (actionableObject.IsMoving()) {
            yield return null;
        }
    }
}
