using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class MenuNivelesController : MonoBehaviour
{
    private GameObject panelNiveles;
    private DatosNivel[] niveles;
    private GameObject panelNivel;
    private bool hasLoaded;

    private void Start() {
        hasLoaded = false;
    }
    public void OnEnter()
    {
        if(hasLoaded){return;}
        panelNiveles = GameObject.Find("Container");
        niveles = Resources.LoadAll<DatosNivel>("Niveles/ScriptableObjects");
        panelNivel = Resources.Load<GameObject>("MenuPrincipal/Nivel");
        foreach (DatosNivel nivel in niveles) {
            GameObject nivelInstanciado = Instantiate(panelNivel, panelNiveles.transform);
            SetNivelData(nivelInstanciado.transform, nivel);
        }
        hasLoaded = true;
    }

    private void SetNivelData(Transform nivel, DatosNivel datos){
        nivel.GetChild(1).GetComponent<Image>().sprite = datos.icon;
        nivel.GetChild(2).GetComponent<TextMeshProUGUI>().text = datos.id.ToString();
        nivel.GetChild(3).GetComponent<TextMeshProUGUI>().text = datos.nombre;
        nivel.GetChild(4).GetComponent<Button>().onClick.AddListener(() => LoadLevel(datos.id));
        nivel.GetChild(5).gameObject.SetActive(NivelBloqueado(datos.id));
    }

    private void LoadLevel(int levelId){
        if(NivelBloqueado(levelId)){return;}
        PlayerPrefs.SetInt("actualLevel", levelId);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Niveles");
    } 

    public void Atras(){
        gameObject.GetComponent<MenuPrincipalController>().MenuPrincipal();
    }

    private bool NivelBloqueado(int nivel){
        int nivelesPasados = PlayerPrefs.GetInt("MaxLevel", 1);
        Debug.Log(nivelesPasados);
        return nivel > nivelesPasados;
    }
}
