using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput player1;
    [SerializeField] private PlayerInput player2;

    void Start()
    {
        var pads = Gamepad.all;
        Debug.Log(pads.Count);

        if (pads.Count < 2)
        {
            Debug.LogError("Did not find 2 gamepads, falling back to keyboard");
            player1.SwitchCurrentControlScheme("WASD", Keyboard.current);
            player2.SwitchCurrentControlScheme("Arrows", Keyboard.current);
        }

        player1.SwitchCurrentControlScheme("Gamepad", pads[0]);
        player2.SwitchCurrentControlScheme("Gamepad", pads[1]);
    }
}
