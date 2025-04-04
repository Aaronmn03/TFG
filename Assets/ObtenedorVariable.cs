using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObtenedorVariable : MonoBehaviour
{
    [SerializeField] protected object valor;
    [SerializeField] protected Texture2D texture;
    [SerializeField] protected DatosBloque datosBloque;
    public void SelectObject(){
        GameObject bloque = Instantiate(datosBloque.prefab, GameObject.Find("AreaTrabajo").transform);
        bloque.GetComponent<BloqueVariable>().SetValue(this);
    }

    public object GetValor(){
        return valor;
    }

    public Texture2D GetTexture2D(){
        return texture;
    }
}
