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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC tuþuna basýldý!"); // ESC tuþunun algýlandýðýný kontrol edin
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Menüyü kapat
        gunsUI.SetActive(false);
        Time.timeScale = 1f; // Oyun zamanýný devam ettir
        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitle
        Cursor.visible = false; // Fare imlecini gizle
        isPaused = false;
    }

    public void Pause()
    {
        pm.ResetFov();
        pauseMenuUI.SetActive(true); // Menüyü aç
        gunsUI.SetActive(true);
        Time.timeScale = 0f; // Oyun zamanýný durdur
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
