using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelChecker : MonoBehaviour
{
    public static NextLevelChecker Instance;
    public TextMeshProUGUI currentEnemyText;
    public TextMeshProUGUI maxEnemyText;

    public GameObject enemyContainer;
    public GameObject loadingScreen; 
    public TextMeshProUGUI loadingProgressText; 

    private int currentEnemyCount, countToKill = 0;
    private bool isNextSceneReady = false,startedLoading=false,triggered=false;
    private AsyncOperation asyncLoadOperation;

    void Start()
    {
        Instance = this;
        // Initialize enemy count and UI
        currentEnemyCount = enemyContainer.transform.childCount;
        maxEnemyText.text = "/" + currentEnemyCount.ToString();
        currentEnemyText.text = "0";

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false); 
        }

        
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex <= SceneManager.sceneCount && countToKill==1&&!startedLoading)
        {
            StartCoroutine(PreloadNextScene());
            startedLoading = true;
        }

        if (countToKill == currentEnemyCount&&!triggered)
        {
            if (isNextSceneReady) ActivateNextScene();
            else 
            {
                
                loadingProgressText.text = "this is the last scene for this demo thank you for playing";
                StartCoroutine(delayedHide(loadingScreen.gameObject, 5f));


            }
            triggered = true;
        }

        currentEnemyText.text = countToKill.ToString();
    }

    public void IncrementKillCount()
    {
        countToKill++;
    }

    private IEnumerator PreloadNextScene()
    {
        
        asyncLoadOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncLoadOperation.allowSceneActivation = false; 

        
        while (!asyncLoadOperation.isDone)
        {

      
            if (asyncLoadOperation.progress >= 0.9f)
            {
                break;
            }

            yield return null;
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
        isNextSceneReady=true;
    }

    private void ActivateNextScene()
    {
        asyncLoadOperation.allowSceneActivation = true;
    }
    IEnumerator delayedHide(GameObject go,float delay) 
    {
        go.SetActive(true);
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
        yield return null;
    }
}
