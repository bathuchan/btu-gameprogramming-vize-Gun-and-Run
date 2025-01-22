using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu,howToMenu,creditsMenu;
    bool settingsIsOn = false, howToIsOn=false,creditsIsOn=false;
    AudioManagerAlex managerAlex;
    private void Start()
    {
        managerAlex = GameObject.FindObjectOfType<AudioManagerAlex>();

        if(settingsMenu!=null && settingsMenu.activeSelf) settingsMenu.SetActive(false);
        if (howToMenu != null && howToMenu.activeSelf) howToMenu.SetActive(false);
        if(creditsMenu != null && creditsMenu.activeSelf)creditsMenu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetAxis("Escape") == 1) 
        {
            
            if (settingsIsOn)
            {
                CloseSettings();
            }
            else if (howToIsOn) 
            {
                CloseHowTo();
            }else if (creditsIsOn) 
            {
                CloseCredits();
            }
        }
        
    }
    public void StartGame()
    {
        
        SceneManager.LoadScene(1);
    }

    public void OpenSettings() 
    {
        if (managerAlex!=null) { managerAlex.PlayClickSound(); }
        
        settingsIsOn = true;
        settingsMenu.SetActive(true);
    }

    public void CloseSettings() 
    {
        if (managerAlex != null) { managerAlex.PlayClickSound(); }
        EventSystem.current.SetSelectedGameObject(null);
        settingsIsOn = false;
        settingsMenu.SetActive(false);
    }

    public void OpenHowTo()
    {
        if (managerAlex != null) { managerAlex.PlayClickSound(); }
        howToIsOn = true;
        howToMenu.SetActive(true);
    }

    public void CloseHowTo()
    {
        if (managerAlex != null) { managerAlex.PlayClickSound(); }
        EventSystem.current.SetSelectedGameObject(null);
        howToIsOn = false;
        howToMenu.SetActive(false);
    }

    public void OpenCredits()
    {
        if (managerAlex != null) { managerAlex.PlayClickSound(); }
        creditsIsOn = true;
        creditsMenu.SetActive(true);
    }

    public void CloseCredits()
    {
        if (managerAlex != null) { managerAlex.PlayClickSound(); }
        EventSystem.current.SetSelectedGameObject(null);
        creditsIsOn = false;
        creditsMenu.SetActive(false);
    }

}
