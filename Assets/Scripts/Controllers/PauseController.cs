using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : Controller<PauseController>
{   
    public bool canPause = false;
    public bool isPaused = false;
    [SerializeField] GameObject _pauseMenu;

    public void TogglePause()
    {   

        if(!canPause)
        {
            return;
        }

        if (isPaused)
        {
            _pauseMenu.SetActive(false);
        }
        else
        {
            _pauseMenu.SetActive(true);
        }
    }
}
