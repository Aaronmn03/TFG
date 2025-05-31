using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;


public class MenuPrincipalController : MonoBehaviour
{
    private GameObject panelPrincipal;
    private GameObject panelNiveles;
    private GameObject panelAjustes;
    private GameObject panelAyuda;
    void Start()
    {
        panelPrincipal = GameObject.Find("MenuPrincipal");
        panelNiveles = GameObject.Find("Niveles");
        panelAjustes = GameObject.Find("Ajustes");
        panelAyuda = GameObject.Find("Ayuda");
        MenuPrincipal();
    }

    public void MenuPrincipal(){
        panelPrincipal.SetActive(true);
        panelNiveles.SetActive(false);
        panelAjustes.SetActive(false);
        panelAyuda.SetActive(false);
    }

    public void Niveles(){
        panelPrincipal.SetActive(false);
        panelNiveles.SetActive(true);
        gameObject.GetComponent<MenuNivelesController>().OnEnter();
    }

    public void Ajustes(){
        panelPrincipal.SetActive(false);
        panelAjustes.SetActive(true);
        gameObject.GetComponent<MenuAjustesController>().OnEnter();
    }

    public void Ayuda(){
        panelPrincipal.SetActive(false);
        panelAyuda.SetActive(true);
        gameObject.GetComponent<MenuAjustesController>().OnEnter();
    }

    public void ShutOut(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
