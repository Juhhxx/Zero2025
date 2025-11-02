using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _aimPivot;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private GameObject _bulletPreviewPrefab;
    private bool _allowMovement = true;
    public bool AllowMovement
    {
        get => _allowMovement;
        set
        {   
            //this resets the bullet when we go back to planning phase
            //Probably not the best way to do this
            if (!value)
                _hasBullet = true;
            _allowMovement = value;
        } 
    }
    private bool _hasBullet = true;
    private bool _keyboardAimRight = false;
    private bool _keyboardAimLeft = false;
    private Rigidbody2D _rb; 
    private Vector2 _moveInput;
    private Vector2 _aimDirection;

    [SerializeField] private bool _showPreview;
    public void SetShowpreviw(bool set)
    {
        _showPreview = set;
        
        if (!set) Destroy(_bulletPreview);
    } 
    private GameObject _bulletPreview = null;
    private Quaternion _aimRotation;
    private Timer _calculatePreview;

    public event Action<Vector2, Vector2, Vector2> OnShootingInputEvent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Debug.Log(_rb.name);
        _calculatePreview = new Timer(0.1f);
        _calculatePreview.OnTimerDone += CalculatePreview;
    }

    // Update is called once per frame
    void Update()
    {
        if (_allowMovement) _rb.linearVelocity = _moveInput * _moveSpeed;
        else _rb.linearVelocity = Vector2.zero; //prevents residual speed from carrying over to aiming phase
        PointInAimDirection();
        KeyboardAim();
        _calculatePreview.CountTimer();
    }

    private void CalculatePreview()
    {
        var temp = _aimPivot.rotation;

        if (temp != _aimRotation && _showPreview)
        {
            Debug.LogWarning("DOING PREVIEW");
            if (_bulletPreview != null)
            {
                Destroy(_bulletPreview);
                _bulletPreview = null;
            }

            _bulletPreview = Instantiate(_bulletPreviewPrefab, _bulletSpawnPoint.position, Quaternion.identity);

            _bulletPreview.GetComponent<BulletPreview>().Move(_aimPivot.rotation * Vector2.up);
        }

        _aimRotation = _aimPivot.rotation;
    }

    public void Move(InputAction.CallbackContext context)
    {
        
        _moveInput = context.ReadValue<Vector2>();
        
    }

    public void Aim(InputAction.CallbackContext context)
    {        
        _aimDirection = context.ReadValue<Vector2>();
    }

    public void KeyboardAimLeft(InputAction.CallbackContext context)
    {
        if(!_allowMovement)
        {
            if (context.performed)
                _keyboardAimLeft = true;
            if (context.canceled)
                _keyboardAimLeft = false;
        }
    }

    public void KeyboardAimRight(InputAction.CallbackContext context)
    {
        if(!_allowMovement)
        {
            if (context.performed)
                _keyboardAimRight = true;
            if (context.canceled)
                _keyboardAimRight = false;
        }
        
    }

    private void KeyboardAim()
    {   
        if(_hasBullet)
        {
            if (_keyboardAimRight)
            _aimPivot.Rotate(new Vector3(0, 0, -90*Time.deltaTime));
            if (_keyboardAimLeft)
            _aimPivot.Rotate(new Vector3(0, 0, 90*Time.deltaTime));   
        }

    }

    private void PointInAimDirection()
    {   
        if(_hasBullet)
        {
            if(_aimDirection != Vector2.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _aimDirection);
                Quaternion rotation = Quaternion.RotateTowards(_aimPivot.rotation, targetRotation, 100f * Time.deltaTime);
                _aimPivot.rotation = rotation;
            }            
        }

    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed && _hasBullet)
        {
            //                   Pivot Direction, Bullet Spawn Position
            Vector2 shotDirection = _aimPivot.rotation * Vector2.up;
            OnShootingInputEvent?.Invoke(shotDirection, _bulletSpawnPoint.position, transform.position);
            _hasBullet = false;
        }
    }

    public void setAimPreviewState(bool newState)
    {
        _aimPivot.gameObject.SetActive(newState);
    }
}
