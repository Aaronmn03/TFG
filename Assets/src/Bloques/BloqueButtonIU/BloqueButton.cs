using UnityEngine;
using UnityEngine.UI;
public class BloqueButton : MonoBehaviour
{
    protected DatosBloque datosBloque;
    public virtual void InicializarBloque(DatosBloque datosBloque){
        SetName(datosBloque.nombre);
        gameObject.SetActive(false);
        gameObject.AddComponent<Button>().onClick.AddListener(() => OnShowBloqueButtonClicked(datosBloque));
    }

    public virtual void OnShowBloqueButtonClicked(DatosBloque datosBloque)
    {
        this.datosBloque = datosBloque;
        GameObject bloque = Instantiate(datosBloque.prefab, GameObject.Find("AreaTrabajo").transform);
    }

    protected void SetName(string name){
        gameObject.name = name;
    }
}

