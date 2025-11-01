using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _aimPivot;
    [SerializeField] private Transform _bulletSpawnPoint;
    public bool AllowMovement { get; set; } = true;
    private Rigidbody2D _rb; 
    private Vector2 _moveInput;
    private Vector2 _aimDirection;

    public event Action<Vector2, Vector2> OnShootingInputEvent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Debug.Log(_rb.name);

    }

    // Update is called once per frame
    void Update()
    {
        _rb.linearVelocity = _moveInput * _moveSpeed;
        PointInAimDirection();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(AllowMovement)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
    }

    public void Aim(InputAction.CallbackContext context)
    {
        _aimDirection = context.ReadValue<Vector2>();
    }

    private void PointInAimDirection()
    {
        if(_aimDirection != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _aimDirection);
            Quaternion rotation = Quaternion.RotateTowards(_aimPivot.rotation, targetRotation, 100f * Time.deltaTime);
            _aimPivot.rotation = rotation;
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //                   Pivot Direction, Bullet Spawn Position
            Vector2 shotDirection = _aimPivot.rotation * Vector2.up;
            OnShootingInputEvent?.Invoke(shotDirection, _bulletSpawnPoint.position);
        }
    }
}
