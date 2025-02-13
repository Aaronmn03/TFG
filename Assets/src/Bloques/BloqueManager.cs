using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Vuforia;

public class BloqueManager : MonoBehaviour
{
    public Nivel nivel;

    private DatosBloque datosBloqueSelected;

    private void Awake()
    {
        if (nivel == null)
        {
            nivel = GetComponent<Nivel>();
        }
    }

    public void OnShowBloqueButtonClicked(DatosBloque datosBloque)
    {
        this.datosBloqueSelected = datosBloque;
        nivel.GetAirFinder().SetActive(true);
        nivel.GetAirFinder().GetComponent<ContentPositioningBehaviour>().AnchorStage = datosBloque.prefab.GetComponent<AnchorBehaviour>();
        nivel.GetAirFinder().GetComponent<ContentPositioningBehaviour>().OnContentPlaced.AddListener(OnContentPlacedHandler);
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

    public void OnContentPlacedHandler(GameObject placedObject)
    {
        placedObject.transform.parent = GameObject.Find("AreaTrabajo").transform;
        nivel.GetAirFinder().SetActive(false);
    }

}
