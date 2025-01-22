using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseMenuUI; // Duraklama menüsü paneli
    public GameObject gunsUI;
    [HideInInspector]public bool isPaused = false; // Duraklama durumunu takip et
    private PlayerMovement pm;
    [HideInInspector] bool inMainMenu=false;



    void Start()
    {
        
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            inMainMenu = true;
        }
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        pauseMenuUI.SetActive(false); // Baþlangýçta menüyü devre dýþý býrak
        gunsUI.SetActive(false);

    }

    void Update()
    {
        
        if (Input.GetAxis("Escape")==1&&!isPaused)
        {
            Pause();

            //if (isPaused)
            //{
            //    Resume();
            //}
            //else
            //{
                
            //}
            
        }

        
    }


    public void Resume()
    {
        Debug.Log("Game resumed");
        Time.timeScale = 1f; // Oyun zamanýný devam ettir
        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitle
        Cursor.visible = false; // Fare imlecini gizle
        isPaused = false;
        pauseMenuUI.SetActive(false); // Menüyü kapat
        gunsUI.SetActive(false);
        
    }

    public void Pause()
    {
        pm.ResetFov();
        pauseMenuUI.SetActive(true); // Menüyü aç
        gunsUI.SetActive(true);
        Time.timeScale = 0.0000001f; // Oyun zamanýný durdur
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest býrak
        Cursor.visible = true; // Fare imlecini göster
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Oyun zamanýný sýfýrla
        pauseMenuUI.SetActive(false); // Menüyü kapat
        gunsUI.SetActive(false);
        Cursor.visible = true; // Fare imlecini göster
        isPaused = false;
        inMainMenu=true;
        SceneManager.LoadScene("MainMenu"); // Ana menü sahnesini yükle
    }

   
}
