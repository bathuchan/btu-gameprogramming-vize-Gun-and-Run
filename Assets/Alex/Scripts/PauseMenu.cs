using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Duraklama menüsü paneli
    private bool isPaused = false; // Duraklama durumunu takip et

    void Start()
    {
        pauseMenuUI.SetActive(false); // Baþlangýçta menüyü devre dýþý býrak
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
        Time.timeScale = 1f; // Oyun zamanýný devam ettir
        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitle
        Cursor.visible = false; // Fare imlecini gizle
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Menüyü aç
        Time.timeScale = 0f; // Oyun zamanýný durdur
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest býrak
        Cursor.visible = true; // Fare imlecini göster
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Oyun zamanýný sýfýrla
        SceneManager.LoadScene("MainMenu"); // Ana menü sahnesini yükle
    }

   
}
