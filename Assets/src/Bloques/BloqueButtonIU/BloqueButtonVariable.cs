using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BloqueButtonVariable : BloqueButton
{
    public GameObject selection;

    public override void InicializarBloque(DatosBloque datosBloque){
        SetName(datosBloque.nombre);
        List<Color> colors = new List<Color>(){Color.red, Color.yellow, Color.green};
        int index = 0;
        foreach (var color in colors)
        {
            SetColor(selection.transform.GetChild(index).gameObject, color, datosBloque);
            index++;
        }
        selection.SetActive(false);
        gameObject.SetActive(false);
    }

    private void SetColor(GameObject gameObject, Color c, DatosBloque datosBloque){
        gameObject.GetComponent<Image>().color = c;
        gameObject.AddComponent<Button>().onClick.AddListener(() => OnShowBloqueButtonClicked(datosBloque, c));
    }

    public void ShowSelection()
    {
        Debug.Log("Holii");
        selection.SetActive(true);
    }

    public void OnShowBloqueButtonClicked(DatosBloque datosBloque, object value)
    {
        this.datosBloque = datosBloque;
        GameObject bloque = Instantiate(datosBloque.prefab, GameObject.Find("AreaTrabajo").transform);
        bloque.GetComponent<BloqueVariable>().SetValue(value);
    }
}