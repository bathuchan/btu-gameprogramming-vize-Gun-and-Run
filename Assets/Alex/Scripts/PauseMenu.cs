using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseMenuUI; // Duraklama men�s� paneli
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
        pauseMenuUI.SetActive(false); // Ba�lang��ta men�y� devre d��� b�rak
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
        Time.timeScale = 1f; // Oyun zaman�n� devam ettir
        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitle
        Cursor.visible = false; // Fare imlecini gizle
        isPaused = false;
        pauseMenuUI.SetActive(false); // Men�y� kapat
        gunsUI.SetActive(false);
        
    }

    public void Pause()
    {
        pm.ResetFov();
        pauseMenuUI.SetActive(true); // Men�y� a�
        gunsUI.SetActive(true);
        Time.timeScale = 0.0000001f; // Oyun zaman�n� durdur
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest b�rak
        Cursor.visible = true; // Fare imlecini g�ster
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Oyun zaman�n� s�f�rla
        pauseMenuUI.SetActive(false); // Men�y� kapat
        gunsUI.SetActive(false);
        Cursor.visible = true; // Fare imlecini g�ster
        isPaused = false;
        inMainMenu=true;
        SceneManager.LoadScene("MainMenu"); // Ana men� sahnesini y�kle
    }

   
}
