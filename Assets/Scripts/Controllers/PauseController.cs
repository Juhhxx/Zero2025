using Unity.VisualScripting;
using UnityEngine;

public class PauseController : Controller<PauseController>
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
