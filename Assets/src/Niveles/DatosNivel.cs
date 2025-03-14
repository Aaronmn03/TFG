using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "NuevoNivel", menuName = "Nivel/Crear Nuevo Nivel")]
public class DatosNivel : ScriptableObject
{
    public int id;
    public string nombre;               
    public Sprite icon;
    public string objetivo;             
    public List<DatosBloque> bloquesDisponibles; 
    public DatosTutorial datosTutorial;
    public bool requireIF;
}
