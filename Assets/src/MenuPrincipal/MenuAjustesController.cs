using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class MenuAjustesController : MonoBehaviour
{
    private AudioController audioController;
    [SerializeField] private Slider musicaSlider;
    [SerializeField] private Slider efectosSlider;
    [SerializeField] private Slider vozSlider;
    public void AdjustMusicaVolume()
    {
        PlayerPrefs.SetFloat("VolumenMusica", musicaSlider.value);
        PlayerPrefs.Save();
        audioController.AdjustAudioVolumes();
    }
    public void AdjustEfectosVolume()
    {
        PlayerPrefs.SetFloat("VolumenEfectos", efectosSlider.value);
        PlayerPrefs.Save();
        audioController.AdjustAudioVolumes();
    }
    public void AdjustVozVolume()
    {
        PlayerPrefs.SetFloat("VolumenVoz", vozSlider.value); 
        PlayerPrefs.Save();
    }
    public void OnEnter(){
        audioController = FindObjectOfType<AudioController>();
        musicaSlider.value = PlayerPrefs.GetFloat("VolumenMusica", 1f);
        efectosSlider.value = PlayerPrefs.GetFloat("VolumenEfectos", 1f);
        vozSlider.value = PlayerPrefs.GetFloat("VolumenVoz", 1f);
    }
    public void Atras(){
        gameObject.GetComponent<MenuPrincipalController>().MenuPrincipal();
    }
}
