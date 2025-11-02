using NaughtyAttributes;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    [SerializeField] private float _speed;
    private bool _doRotate;

    public void ToogleRotation(bool onOff) => _doRotate = onOff;

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void TurnOn() => ToogleRotation(true);

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void TurnOff() => ToogleRotation(false);

    private void Update()
    {
        if (_doRotate) Spin();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void ResetPosition()
    {
        transform.rotation = Quaternion.identity;
    }
    private void Spin()
    {
        float rotation = _speed * Time.deltaTime;

        var temp = Vector3.zero;
        temp.z = rotation;

        transform.Rotate(temp);
    }


}
