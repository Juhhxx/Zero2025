using UnityEngine.SceneManagement;

public class SceneController : Controller<SceneController>
{
    public void loadMainMenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void loadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
