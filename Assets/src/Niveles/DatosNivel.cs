using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "NuevoNivel", menuName = "Nivel/Crear Nuevo Nivel")]
public class DatosNivel : ScriptableObject
{
    [Header("Prefab a Instanciar")]
    public GameObject prefabToSpawn;
    public string nombre;               
    public string objetivo;             
    public List<DatosBloque> bloquesDisponibles; 
    
}
