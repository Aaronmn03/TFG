using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GestorNiveles : MonoBehaviour
{
    public List<DatosNivel> niveles;
    private int nivelActual;
    public Nivel nivelEnEscena;

    void Start()
    {
        nivelActual = PlayerPrefs.GetInt("actualLevel") - 1;
        nivelEnEscena = GetComponent<Nivel>();
        CargarNivel(nivelActual);
    }

    public void CargarNivel(int indice)
    {
        if (indice < niveles.Count)
        {
            if (nivelEnEscena != null)
            {
                nivelEnEscena.AsignarNivel(niveles[nivelActual]);
            }

        }
    }

    public void SiguienteNivel()
    {
        nivelActual++;
        if (nivelActual < niveles.Count)
        {
            CargarNivel(nivelActual);
        }
    }

    public void ReiniciarNivel()
    {
        nivelEnEscena.Reiniciar();
    }

    public int GetLevelCount()
    {
        return niveles.Count;
    }
}
