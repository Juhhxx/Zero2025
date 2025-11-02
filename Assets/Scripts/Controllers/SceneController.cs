using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Controller<SceneController>
{
    public void loadMainMenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void loadGameScene()
    {
        SceneManager.LoadScene(Random.Range(1,SceneManager.sceneCountInBuildSettings));
    }
}
