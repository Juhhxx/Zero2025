using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : Controller<CameraController>
{
    public void ChangeToMenuCameraRenderer()
    {
        // Change camera renderer to one that doesnt contain the green vignette
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.SetRenderer(1);
    }

    public void ChangeToDefaultCameraRenderer()
    {
        // Change camera renderer to one that contains the green vignette
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.SetRenderer(0);
    }
}
