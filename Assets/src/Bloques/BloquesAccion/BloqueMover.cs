using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BloqueMover : BloqueAccion{

    public override void Action(){
        actionableObject = FindObjectOfType<ActionableObject>();
        actionableObject.MoveForward();
    }
}
