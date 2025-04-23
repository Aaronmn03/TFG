using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiendaCampaña : ObtenedorVariable
{
    [SerializeField] Transform tiendaCampaña;
    private void Start() {
        this.valor = tiendaCampaña.position;
        base.Start();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.TryGetComponent<ProgramableObject>(out ProgramableObject programableObject)) {
            nivel.Lose("No te choques con la tienda de campaña", true);
        }
    }
}
