using UnityEngine;

public abstract class ObtenedorVariable : MonoBehaviour
{
    [SerializeField] protected object valor;
    [SerializeField] protected Texture2D texture;
    [SerializeField] protected DatosBloque datosBloque;
    protected Nivel nivel;

    protected void Start() {
        nivel = FindObjectOfType<Nivel>();
    }
    public void SelectObject(){
        GameObject bloque = Instantiate(datosBloque.prefab, nivel.GetSelectedProgramableObject().GetZonaProgramacion().transform);
        if (bloque.TryGetComponent<BloqueVariable>(out var bloqueVariable)) {
            bloqueVariable.SetValue(this);
        }else if (bloque.TryGetComponent<BloqueBotonPulsado>(out var bloqueBotonPulsado)) {
            bloqueBotonPulsado.SetBoton(this);
        }
    }

    public object GetValor(){
        return valor;
    }

    public Texture2D GetTexture2D(){
        return texture;
    }
}
