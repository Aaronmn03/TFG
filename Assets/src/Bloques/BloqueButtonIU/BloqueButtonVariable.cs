using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;


public class BloqueButtonVariable : BloqueButton
{
    public GameObject selection;
    public override void InicializarBloque(DatosBloque datosBloque){
        SetName(datosBloque.nombre);
        Texture2D texturePlayerPosition = Resources.Load<Texture2D>("Texture2D/posicion_jugador_texture");
        List<object> values = new List<object>(){Color.red,Color.green,texturePlayerPosition};
        int index = 0;
        foreach (var value in values)
        {
            if(value is Color color){
                SetColor(selection.transform.GetChild(index).gameObject, color, datosBloque);
            }else{
                SetIcon(selection.transform.GetChild(index).gameObject, (Texture2D) value, datosBloque);
            }
            index++;
        }
        selection.SetActive(false);
        gameObject.SetActive(false);
    }

    private void SetColor(GameObject gameObject, Color c, DatosBloque datosBloque){
        gameObject.GetComponent<Image>().color = c;
        gameObject.AddComponent<Button>().onClick.AddListener(() => OnShowBloqueButtonClicked(datosBloque, c));
    }

    private void SetIcon(GameObject gameObject,Texture2D textura, DatosBloque datosBloque){
        gameObject.GetComponent<Image>().sprite = Sprite.Create(textura, new Rect(0, 0, textura.width, textura.height), new Vector2(0.5f, 0.5f));;
        gameObject.AddComponent<Button>().onClick.AddListener(() => OnShowBloqueButtonClicked(datosBloque, textura));
    }

    public void ShowSelection()
    {
        selection.SetActive(!selection.activeSelf);
    }

    public void OnShowBloqueButtonClicked(DatosBloque datosBloque, object value)
    {
        this.datosBloque = datosBloque;
        GameObject bloque = Instantiate(datosBloque.prefab, GameObject.Find("AreaTrabajo").transform);
        bloque.GetComponent<BloqueVariable>().SetValue(value);
    }
}