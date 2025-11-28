using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _aimPivot;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private GameObject _bulletPreviewPrefab;
    [SerializeField] private Animator _playerAnim;
    [SerializeField] private Animator _gunAnim;
    private bool _allowMovement = true;
    private DetectBulletHit detectBulletHit;
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

    [Header("Audio")]
    [SerializeField] private AudioResource _stepClips;
    [SerializeField] private AudioResource _hurtClips;
    [SerializeField] private AudioResource _gunReadyClips;

    public void SetShowPreview(bool set)
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
        _calculatePreview = new Timer(0.1f);
        _calculatePreview.OnTimerDone += CalculatePreview;
        GetComponentInChildren<DetectBulletHit>().OnHitEvent += PlayHurtSFX;
        detectBulletHit = GetComponentInChildren<DetectBulletHit>();
        detectBulletHit.OnHitEvent +=  () =>_playerAnim.SetTrigger("Death");
    }

    // Update is called once per frame
    void Update()
    {
        if (_allowMovement) _rb.linearVelocity = _moveInput * _moveSpeed;
        else _rb.linearVelocity = Vector2.zero; //prevents residual speed from carrying over to aiming phase
        PointInAimDirection();
        KeyboardAim();
        _calculatePreview.CountTimer();

        _playerAnim.SetFloat("Speed", _rb.linearVelocity.magnitude);

        MirrorHand();
    }

    public void ShotAnimation() => _gunAnim.SetTrigger("Shot");

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

     private void MirrorHand()
    {
        if (_bulletSpawnPoint.position.x >= transform.position.x)
        {
            _bulletSpawnPoint.GetComponent<SpriteRenderer>().flipY = false;
        }
        else
        {
            _bulletSpawnPoint.GetComponent<SpriteRenderer>().flipY = true;
        }
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
        if (_hasBullet)
        {
            if (_keyboardAimRight)
                _aimPivot.Rotate(new Vector3(0, 0, -90 * Time.deltaTime));
            if (_keyboardAimLeft)
                _aimPivot.Rotate(new Vector3(0, 0, 90 * Time.deltaTime));
        }
        else _aimPivot.Rotate(Vector2.zero); //prevents residual input from carrying over to the next aim phase

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
            SetShowPreview(false);
            PlayGunReadySFX();
        }
    }

    public void setAimPreviewState(bool newState)
    {
        _aimPivot.gameObject.SetActive(newState);
        Debug.Log("Set " + gameObject.name + " gun to " + newState);
    }

    // Resets aiming input (called when entering an aim phase)
    public void ClearKeyboardInput()
    {
        _keyboardAimRight = false;
        _keyboardAimLeft = false;

    }

    #region Audio
    private void PlayHurtSFX()
    {
        SoundFXManager.instance.PlaySoundFXResource(_hurtClips, transform, 1f, 0.580f);
    }

    public void PlayStepSFX()
    {
        SoundFXManager.instance.PlaySoundFXResource(_stepClips, transform, 1f, 0.420f);
    }
    public void PlayGunReadySFX()
    {
        SoundFXManager.instance.PlaySoundFXResource(_gunReadyClips, transform, 1f, 0.785f);
    }
    #endregion
}
