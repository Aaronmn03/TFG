using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "NuevoBloque", menuName = "Bloque/Crear Nuevo Bloque")]
public class DatosBloque : ScriptableObject
{
    public int id;
    public string nombre;   
    public string texto;   
    public Color colorTexto;   
    public Color color;            
    public GameObject prefab; 
    public GameObject prefabIcon; 
}
