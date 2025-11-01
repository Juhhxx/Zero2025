using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _player1;
    [SerializeField] private PlayerInput _player2;

    void Start()
    {
        var pads = Gamepad.all;
        Debug.Log(pads.Count);

        if (pads.Count < 2)
        {
            Debug.LogError("Did not find 2 gamepads, falling back to keyboard");
            _player1.SwitchCurrentControlScheme("WASD", Keyboard.current);
            _player2.SwitchCurrentControlScheme("Arrows", Keyboard.current);
        }

        _player1.SwitchCurrentControlScheme("Gamepad", pads[0]);
        _player2.SwitchCurrentControlScheme("Gamepad", pads[1]);
    }
}
