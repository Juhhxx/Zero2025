using UnityEngine;

public class ExitButton : MonoBehaviour
{

    void Awake()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            gameObject.SetActive(false);
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
