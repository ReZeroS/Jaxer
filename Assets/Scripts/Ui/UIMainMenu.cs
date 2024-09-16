using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{

    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UIFadeScreen fadeScreen;

    private void Start()
    {
        if (!SaveManager.instance.HasSaveData())
        {
            continueButton.SetActive(false);
        }
    }


    public void ContinueGame()
    {
        StartCoroutine(FadeToScene(1.5f));
    }


    public void StartNewGame()
    {
        SaveManager.instance.DeleteSaveFile();
        StartCoroutine(FadeToScene(1.5f));
    }
    

    public void QuitGame()
    {
        Application.Quit();
    }


    IEnumerator FadeToScene(float delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
    
    
}
