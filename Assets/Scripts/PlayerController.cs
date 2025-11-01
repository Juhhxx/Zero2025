using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform aimPivot;
    private Rigidbody2D rb; 
    private Vector2 moveInput;
    private Vector2 aimDirection;

    public static event Action<Vector2> OnShootingInputEvent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log(rb.name);

    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        PointInAimDirection();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Aim(InputAction.CallbackContext context)
    {
        aimDirection = context.ReadValue<Vector2>();
    }

    private void PointInAimDirection()
    {
        if(aimDirection != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, aimDirection);
            Quaternion rotation = Quaternion.RotateTowards(aimPivot.rotation, targetRotation, 100f * Time.deltaTime);
            aimPivot.rotation = rotation;
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnShootingInputEvent(aimDirection);
        }
    }
}
