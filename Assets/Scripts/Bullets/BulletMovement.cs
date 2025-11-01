using UnityEngine;
using UnityEngine.Events;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _maxBounces;

    private int _bounces;
    private Rigidbody2D _rb;

    public UnityEvent OnBulletKill;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        Move(transform.right);
    }
    private void Update()
    {
    }

    public void Move(Vector2 direction)
    {
        Debug.Log($"MOVE VECTOR : {direction}");

        _rb.linearVelocity = direction * _speed;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_bounces == _maxBounces && _maxBounces > 0)
        {
            KillBullet();
            return;
        } 
        
        Debug.LogWarning($"COLLIDNG WITH {collision.collider.name}");

        Vector2 newDir = Vector2.Reflect(transform.right, collision.contacts[0].normal);

        Debug.LogWarning($"DIRECTION ANGLE {newDir}");

        float angle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;

        _rb.rotation = angle;

        Move(newDir);

        _bounces++;
    }
    
    private void KillBullet()
    {
        Debug.Log($"Bullet {name} Destroyed");

        OnBulletKill?.Invoke();
        Destroy(gameObject);
    }
}
