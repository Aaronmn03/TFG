using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandlerPlayer : MonoBehaviour
{
    private Animator anim;
    private void Start() {
        anim = transform.GetComponent<Animator>();
    }
    public void Walk()
    {
        anim.SetBool("walk", true);
        
    }
    public void StopWalk(){
        anim.SetBool("walk", false);
    }
}
