using Unity.VisualScripting;
using UnityEngine;

public class PauseController : Controller<SceneController>
{
    public void pauseGame()
    {
        Time.timeScale = 0f;
    }

    public void unpauseGame()
    {
        Time.timeScale = 1f;
    }
}
