using UnityEngine;

public class pauseOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        PauseController.Instance.isPaused = true;
    }
    private void OnDisable()
    {
        PauseController.Instance.isPaused = false;
    }
}
