
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AudioManagerAlex : MonoBehaviour
{
    
    [Header(" ---------- Audio Sources ---------- ")]
    [SerializeField] AudioSource backgroundSound;
    [SerializeField] AudioSource clickSound;

    [Header("---------- Audio Clips ---------- ")]
    public AudioClip background;
    public AudioClip click;

    //private static AudioManagerAlex instance;

    [SerializeField] Slider masterVolumeSlider, musicVolumeSlider, sfxVolumeSlider;
    [SerializeField]AudioMixer audioMixer;


    void Start()
    {
        

        if (!PlayerPrefs.HasKey("MasterVolume") &&!PlayerPrefs.HasKey("MusicVolume")&&!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("MainVolume", 1);
            PlayerPrefs.SetFloat("MusicVolume", 1);
            PlayerPrefs.SetFloat("SFXVolume", 1);


        }
        else
        {
            Load();
        }


    }


    public void ChangeMasterVolume()
    {
        
        Save("MasterVolume", masterVolumeSlider.value);
    }

    public void ChangeMusicVolume()
    {
       
        Save("MusicVolume", musicVolumeSlider.value);
    }
    public void ChangeSfxVolume()
    {
       
        Save("SFXVolume", sfxVolumeSlider.value);
        
    }

    public void PlaySfxPreview()
    {
        if (!clickSound.isPlaying)
        {
            clickSound.Play();
        }
    }

    public void StopSfxPreview()
    {
        clickSound.Stop();
    }

    public void PlaySfxLoop()
    {
        if (!clickSound.isPlaying)
        {
            clickSound.loop = true;
            clickSound.Play();
        }
    }

    public void StopSfxLoop()
    {
        clickSound.loop = false;
        clickSound.Stop();
    }
    private void Load()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolumeSlider.value= PlayerPrefs.GetFloat("MusicVolume");
        sfxVolumeSlider.value= PlayerPrefs.GetFloat("SFXVolume");

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolumeSlider.value) * 20f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolumeSlider.value) * 20f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolumeSlider.value) * 20f);
    }

    private void Save(string pref,float val)
    {
        PlayerPrefs.SetFloat(pref, val);
        audioMixer.SetFloat(pref,Mathf.Log10(val)*20f);
    }

    public void PlayClickSound() 
    {
        clickSound.Play();
    }
}
