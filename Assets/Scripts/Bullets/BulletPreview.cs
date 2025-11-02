using UnityEngine;
using NaughtyAttributes;

public class BulletPreview : MonoBehaviour
{
    [SerializeField] private float _previewTime;

    [SerializeField] private Collider2D _collider;

    [SerializeField] private int _passesNeeded;
    private int _passes;

    [SerializeField, ReadOnly] private bool _isMaterialized = false;

    [SerializeField] private float _speed;
    [SerializeField] private int _maxBounces;

    private int _bounces;
    private Rigidbody2D _rb;
    private Timer _stopTimer;
    private bool _doLogic = true;

    private void Awake()
    {
        Debug.Log("SPAWNED BULLET PREVIEW");
        _rb = GetComponent<Rigidbody2D>();
        _stopTimer = new Timer(_previewTime, Timer.TimerReset.Manual);
        _stopTimer.OnTimerDone += () => _doLogic = false;
        _stopTimer.OnTimerDone += () => _rb.linearVelocity = Vector2.zero;
    }
    private void Update()
    {
        if (_doLogic)
        {
            CheckMaterialization();
        }
        _stopTimer.CountTimer();
    }

    private void CheckMaterialization()
    {
        if (_isMaterialized)
        {
            _collider.isTrigger = false;
        }
        else
        {
            _collider.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _passes++;

        if (_passes == _passesNeeded) _isMaterialized = true;
    }

    public void Move(Vector2 direction)
    {
        Debug.LogWarning($"MOVE VECTOR PREVIEW : {direction}");

        _rb.linearVelocity = direction * _speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _rb.rotation = angle;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_bounces == _maxBounces && _maxBounces > 0)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        } 
        
        Debug.LogWarning($"COLLIDNG WITH {collision.collider.name}");

        Vector2 newDir = Vector2.Reflect(transform.right, collision.contacts[0].normal);

        Debug.LogWarning($"DIRECTION ANGLE {newDir}");

        Move(newDir);

        _bounces++;
    }
}
