using UnityEngine;
using UnityEngine.UI;
public class BloqueButton : MonoBehaviour
{
    private Nivel nivel;
    protected DatosBloque datosBloque;

    private void Start() {
        nivel = FindObjectOfType<Nivel>();
    }
    public virtual void InicializarBloque(DatosBloque datosBloque){
        SetName(datosBloque.nombre);
        gameObject.SetActive(false);
        gameObject.AddComponent<Button>().onClick.AddListener(() => OnShowBloqueButtonClicked(datosBloque));
    }

    public virtual void OnShowBloqueButtonClicked(DatosBloque datosBloque)
    {
        this.datosBloque = datosBloque;
        GameObject bloque = Instantiate(datosBloque.prefab, nivel.GetSelectedProgramableObject().GetZonaProgramacion().transform);
    }

    protected void SetName(string name){
        gameObject.name = name;
    }
}

