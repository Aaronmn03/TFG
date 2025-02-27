using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "NuevoBloque", menuName = "Bloque/Crear Nuevo Bloque")]
public class DatosBloque : ScriptableObject
{
    public string nombre;   
    public GameObject prefab; 
    public GameObject prefabIcon; 
}
