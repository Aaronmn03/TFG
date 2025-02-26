using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPrincipalController : MonoBehaviour
{
    private GameObject panelPrincipal;
    private GameObject panelNiveles;
    void Start()
    {
        panelPrincipal = GameObject.Find("MenuPrincipal");
        panelPrincipal.SetActive(true);
        panelNiveles = GameObject.Find("Niveles");
        panelNiveles.SetActive(false);
    }

    public void MenuPrincipal(){
        panelPrincipal.SetActive(true);
        panelNiveles.SetActive(false);
    }

    public void Niveles(){
        panelPrincipal.SetActive(false);
        panelNiveles.SetActive(true);
        gameObject.GetComponent<MenuNivelesController>().OnEnter();
    }

    public void ShutOut(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
        PlayerPrefs.SetInt("MaxLevel", 1);
    }
}
