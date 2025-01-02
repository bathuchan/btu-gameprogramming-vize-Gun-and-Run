using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Duraklama men�s� paneli
    private bool isPaused = false; // Duraklama durumunu takip et

    void Start()
    {
        pauseMenuUI.SetActive(false); // Ba�lang��ta men�y� devre d��� b�rak
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC tu�una bas�ld�!"); // ESC tu�unun alg�land���n� kontrol edin
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
        pauseMenuUI.SetActive(false); // Men�y� kapat
        Time.timeScale = 1f; // Oyun zaman�n� devam ettir
        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitle
        Cursor.visible = false; // Fare imlecini gizle
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Men�y� a�
        Time.timeScale = 0f; // Oyun zaman�n� durdur
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest b�rak
        Cursor.visible = true; // Fare imlecini g�ster
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Oyun zaman�n� s�f�rla
        SceneManager.LoadScene("MainMenu"); // Ana men� sahnesini y�kle
    }

   
}
