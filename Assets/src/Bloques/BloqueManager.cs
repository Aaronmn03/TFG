using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BloqueManager : MonoBehaviour
{
    public Nivel nivel;



    private void Awake()
    {
        if (nivel == null)
        {
            nivel = GetComponent<Nivel>();
        }
    }

    public void OnShowBloqueButtonClicked(DatosBloque datosBloque)
    {
        nivel.GetAirFinder().SetActive(true);
        nivel.GetAirPlane().transform.GetChild(datosBloque.id).gameObject.SetActive(true);
    }

    public void GenerarBloques(List<DatosBloque> bloques, GameObject playCanvas){
        Transform contenedorBloques = playCanvas.transform.GetChild(2).GetChild(0).GetChild(0);
        LimpiarBloques(contenedorBloques);
        foreach (DatosBloque datos in bloques)
        {
            if (datos == null) continue;
            GameObject nuevoBloque = Instantiate(datos.prefabIcon, contenedorBloques);
            Bloque bloque = nuevoBloque.GetComponent<Bloque>();
            bloque.SetColor(datos.color);
            bloque.SetText(datos.texto, datos.colorTexto);
            bloque.SetName(datos.nombre);
            bloque.gameObject.SetActive(false);
            nuevoBloque.AddComponent<Button>().onClick.AddListener(() => OnShowBloqueButtonClicked(datos));
        }
    }

    private void LimpiarBloques(Transform contenedorBloques){
        foreach (Transform child in contenedorBloques)
        {
            Destroy(child.gameObject);
        }
    }

}
