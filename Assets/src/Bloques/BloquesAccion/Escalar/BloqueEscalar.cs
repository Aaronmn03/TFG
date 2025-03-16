using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueEscalar : BloqueAccion{
    [SerializeField] private bool crecer;
    public override IEnumerator Action(){
        actionableObject = programableObject.GetComponent<ActionableObject>();
        if(crecer){
            actionableObject.Agrandar();
        }else{
            actionableObject.Reducir();
        }
        while (actionableObject.IsMoving()) {
            yield return null;
        }
    }
}
