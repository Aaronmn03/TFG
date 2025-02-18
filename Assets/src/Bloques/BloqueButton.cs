using UnityEngine;
using UnityEngine.UI;
public class BloqueButton : MonoBehaviour
{
    private DatosBloque datosBloque;
    public void InicializarBloque(DatosBloque datosBloque){
        SetName(datosBloque.nombre);
        gameObject.SetActive(false);
        gameObject.AddComponent<Button>().onClick.AddListener(() => OnShowBloqueButtonClicked(datosBloque));
    }

    private void OnShowBloqueButtonClicked(DatosBloque datosBloque)
    {
        this.datosBloque = datosBloque;
        GameObject bloque = Instantiate(datosBloque.prefab, GameObject.Find("AreaTrabajo").transform);
    }

    private void SetName(string name){
        gameObject.name = name;
    }
}