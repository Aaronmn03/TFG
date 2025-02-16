using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BloqueButton : MonoBehaviour
{
    private Image bloqueImagen;
    private TextMeshProUGUI bloqueText;
    private DatosBloque datosBloque;

    public void InicializarBloque(DatosBloque datosBloque){
        SetColor(datosBloque.color);
        SetText(datosBloque.texto, datosBloque.colorTexto);
        SetName(datosBloque.nombre);
        gameObject.SetActive(false);
        gameObject.AddComponent<Button>().onClick.AddListener(() => OnShowBloqueButtonClicked(datosBloque));
    }

    private void OnShowBloqueButtonClicked(DatosBloque datosBloque)
    {
        this.datosBloque = datosBloque;
        GameObject bloque = Instantiate(datosBloque.prefab, GameObject.Find("AreaTrabajo").transform);
    }
    private void SetColor(Color color)
    {
        bloqueImagen = GetComponent<Image>();
        if (bloqueImagen != null)
        {
            bloqueImagen.color = color;
        }
    }
    private void SetText(string texto, Color color)
    {
        bloqueText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (bloqueText != null)
        {
            bloqueText.text = texto;
            bloqueText.color = color;
        }
    }

    private void SetName(string name){
        gameObject.name = name;
    }
}