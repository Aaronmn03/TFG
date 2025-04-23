using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private List<AudioSource> musicSource;
    private List<AudioSource> soundSource;
    void Start()
    {
        AdjustAudioVolumes();
    }
    public void AdjustAudioVolumes()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        musicSource = new List<AudioSource>();
        soundSource = new List<AudioSource>();

        foreach (var audioSource in allAudioSources)
        {
            if (audioSource.CompareTag("Musica"))
            {
                musicSource.Add(audioSource);
            }
            else if (audioSource.CompareTag("Sonido"))
            {
                soundSource.Add(audioSource);
            }
        }
        foreach (AudioSource music in musicSource)
        {
            if(music != null){
                music.volume = PlayerPrefs.GetFloat("VolumenMusica");
            } 
        }

        foreach (AudioSource sound in soundSource)
        {
            if(sound != null){
                sound.volume = PlayerPrefs.GetFloat("VolumenEfectos");  
            }
        }
    }
}
