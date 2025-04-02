using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "NuevoTutorial", menuName = "Tutorial/Crear Nuevo mensajes tutorial")]
public class DatosTutorial : ScriptableObject
{
    public int id;
    [TextArea] public string[] mensajesBasicos;
    [TextArea] public string[] pistas;
}
