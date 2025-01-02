using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header(" ---------- Audio Sources ---------- ")]
    [SerializeField] AudioSource backgroundSound;
    [SerializeField] AudioSource clikSound;

    [Header("---------- Audio Clips ---------- ")]
    public AudioClip background;
    public AudioClip clik;








    private static AudioManager instance;

    [SerializeField] Slider volumeSlider;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
        }
        else
        {
            Load();
        }

        backgroundSound.clip = background;
        backgroundSound.Play();

    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
    }

}
