using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton : ObtenedorVariable
{
    [SerializeField] bool hasBeenPressed = false;
    private Animator anim;
    private void Start() {
        base.Start();
        this.valor = hasBeenPressed;
        if (nivel != null) {
            nivel.ResetEvent += Resetear;
        }
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.TryGetComponent<ProgramableObject>(out ProgramableObject programableObject)) {
            hasBeenPressed = true;
            anim.SetBool("isPress", hasBeenPressed);
            this.valor = hasBeenPressed;
        }
    }

    private void Resetear(){
        hasBeenPressed = false;
        anim.SetBool("isPress", hasBeenPressed);
        this.valor = hasBeenPressed;
    }
}
