using UnityEngine;
using UnityEngine.Events;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _maxBounces;

    private int _bounces;
    private Rigidbody2D _rb;

    private BulletController _controller;

    private void Awake()
    {
        Debug.Log("SPAWNED BULLET");
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<BulletController>();

    }

    private void Update()
    {
        Debug.Log($"VELOCITY : {_rb.linearVelocity}");
    }

    public void Move(Vector2 direction)
    {
        Debug.LogWarning($"MOVE VECTOR : {direction}");

        _rb.linearVelocity = direction * _speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _rb.rotation = angle;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_bounces == _maxBounces && _maxBounces > 0)
        {
            _controller.KillBullet();
            return;
        } 
        
        Debug.LogWarning($"COLLIDNG WITH {collision.collider.name}");

        Vector2 newDir = Vector2.Reflect(transform.right, collision.contacts[0].normal);

        Debug.LogWarning($"DIRECTION ANGLE {newDir}");

        Move(newDir);

        _bounces++;
    }
}
