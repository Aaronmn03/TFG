using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaforo : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private Texture2D texture;
    [SerializeField] private DatosBloque datosBloque;
    private void Start() {
        Nivel nivel = FindObjectOfType<Nivel>();
        if (nivel != null) {
            nivel.PlayEvent += OnPlay; 
        }
    }

    private void OnPlay(){
        List<Color> colors = new List<Color>(){Color.red, Color.yellow, Color.green};        
        color = colors[Random.Range(0, colors.Count)];
        Renderer renderer = GetComponent<Renderer>();        
        if (renderer != null) {
            renderer.material.color = color;
        } else {
            Debug.LogError("El objeto no tiene un componente Renderer.");
        }
    }

    public void SelectObject(){
        GameObject bloque = Instantiate(datosBloque.prefab, GameObject.Find("AreaTrabajo").transform);
        bloque.GetComponent<BloqueVariableColor>().SetValue(this);
    }

    public Color GetColor(){
        return color;
    }

    public Texture2D GetTexture2D(){
        return texture;
    }
}
