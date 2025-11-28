using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _maxBounces;

    private int _bounces;
    private Rigidbody2D _rb;

    private BulletController _controller;

    [Header("Audio")]
    [SerializeField] private AudioResource _ricochetClips;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<BulletController>();

    }

    private void Update()
    {
        //Debug.Log($"VELOCITY : {_rb.linearVelocity}");
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
        PlayRicochetSFX();

        Debug.LogWarning($"COLLIDNG WITH {collision.collider.name}");

        Vector2 newDir = Vector2.Reflect(transform.right, collision.contacts[0].normal);

        Debug.LogWarning($"DIRECTION ANGLE {newDir}");

        Move(newDir);

        _bounces++;
    }

    #region Audio
    private void PlayRicochetSFX()
    {
        SoundFXManager.instance.PlaySoundFXResource(_ricochetClips, transform, 1f, 1.590f);
    }
    #endregion
}
