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
        GameObject bloque = Instantiate(datosBloque.prefab, GameObject.Find("AreaTrabajo").transform);
    }

    public void GenerarBloques(List<DatosBloque> bloques, GameObject playCanvas){
        Transform contenedorBloques = playCanvas.transform.GetChild(2).GetChild(0).GetChild(0);
        LimpiarBloques(contenedorBloques);
        foreach (DatosBloque datos in bloques)
        {
            if (datos == null) continue;
            GameObject nuevoBloque = Instantiate(datos.prefabIcon, contenedorBloques);
            Debug.Log("Generando bloque: " + datos.nombre);
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
    public void OnContentPlacedHandler()
    {
        nivel.GetAirFinder().SetActive(false);
        ProgramableObject programableObject = GameObject.Find("ARManipulator").GetComponent<ObjectManipulator>().GetProgramableObject();
        programableObject.SetZonaProgramacion(GameObject.Find("AreaTrabajo").GetComponent<ZonaProgramacion>());
        nivel.ActivateZonaBloques();
    }
}
