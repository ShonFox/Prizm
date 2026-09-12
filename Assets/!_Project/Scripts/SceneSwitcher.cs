using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Имена сцен
    [SerializeField] private string _mainMenuSceneName = "01_MainMenu";
    [SerializeField] private string _gameLevelSceneName = "02_GameLevel";
    [SerializeField] private string _fatalSceneName = "03_FatalScene";
    [SerializeField] private string _finalSceneName = "04_FinalScene";

    //Загрузка сцен
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }

    public void LoadGameLevel()
    {
        SceneManager.LoadScene(_gameLevelSceneName);
    }

    public void LoadFatalScene()
    {
        SceneManager.LoadScene(_fatalSceneName);
    }

    public void LoadFinalScene()
    {
        SceneManager.LoadScene(_finalSceneName);
    }
}
