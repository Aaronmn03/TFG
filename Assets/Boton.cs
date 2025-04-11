using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton : ObtenedorVariable
{
    [SerializeField] bool hasBeenPressed = false;
    private Nivel nivel;
    private void Start() {
        this.valor = hasBeenPressed;
        this.nivel = FindObjectOfType<Nivel>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.TryGetComponent<ProgramableObject>(out ProgramableObject programableObject)) {
            hasBeenPressed = true;
        }
    }
}
