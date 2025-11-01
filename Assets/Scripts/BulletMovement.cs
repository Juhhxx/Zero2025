using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector3 _angle;

    private void Awake()
    {
        _angle = transform.localEulerAngles;
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveVec = transform.position + (transform.right * _speed * Time.deltaTime);

        Debug.Log($"MOVE VECTOR : {moveVec}");

        transform.position = moveVec;
        transform.eulerAngles = _angle;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning($"COLLIDNG WITH {collision.collider.name}");

        Vector2 newDir = Vector2.Reflect(_angle, collision.contacts[0].normal);

        Debug.LogWarning($"DIRECTION ANGLE {newDir}");

        _angle = newDir;
    }
}
