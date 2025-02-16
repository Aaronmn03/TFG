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
    public void GenerarBloques(List<DatosBloque> bloques, GameObject playCanvas){
        Transform contenedorBloques = playCanvas.transform.GetChild(2).GetChild(0).GetChild(0);
        LimpiarBloques(contenedorBloques);
        foreach (DatosBloque datos in bloques)
        {
            if (datos == null) continue;
            BloqueButton nuevoBloque = Instantiate(datos.prefabIcon, contenedorBloques).GetComponent<BloqueButton>();
            nuevoBloque.InicializarBloque(datos);
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
