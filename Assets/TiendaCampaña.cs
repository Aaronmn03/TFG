using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiendaCampaña : ObtenedorVariable
{
    [SerializeField] Transform tiendaCampaña;
    private void Start() {
        this.valor = tiendaCampaña.position;
    }
}
