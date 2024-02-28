using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] GameObject loadingPanel;
    public static LoadingScreen instance;
    private void Awake()
    {
        instance = this;
    }

    private string sceneName;
    public void LoadLevel(int sceneIndex)//using this function to load other scene and main menu scene
    {
        loadingPanel.SetActive(true);


        fillImage.fillAmount = 0f;
        StartCoroutine(LoadLevelInAsync(sceneIndex));
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }

    }
    IEnumerator LoadLevelInAsync(int sceneIndex)//this loads level with the build index
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {

            fillImage.fillAmount = asyncOperation.progress;

            yield return new WaitForSeconds(Time.deltaTime);

        }

        yield return new WaitForSeconds(Time.deltaTime * 0.01f);
    }

    //this function is attached to the next button
   
    public void LoadNextLevel(int index)
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }

        sceneName = "Level" + index;


        loadingPanel.SetActive(true);
        fillImage.fillAmount = 0f;
        StartCoroutine(LoadLevelInAsync(sceneName));
    }
    //this is attached to restart button
    public void RestartLevel()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        sceneName = SceneManager.GetActiveScene().name;
        loadingPanel.SetActive(true);
        fillImage.fillAmount = 0f;
        StartCoroutine(LoadLevelInAsync(sceneName));
    }

    IEnumerator LoadLevelInAsync(string sceneName)//this loads level with level name
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {

            fillImage.fillAmount = asyncOperation.progress;
            print(fillImage.fillAmount);
            yield return new WaitForSeconds(Time.deltaTime);


        }

        yield return new WaitForSeconds(Time.deltaTime * 0.2f);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
