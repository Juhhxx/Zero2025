using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject _ghostBullet;
    [SerializeField] private GameObject _materializedBullet;
    [SerializeField] private int _passesNeeded;
    private int _passes;

    [SerializeField, ReadOnly] private bool _isMaterialized = false;

    private Collider2D _collider;

    public UnityEvent OnBulletKill;

    private BulletPool _pool;
    public void SetPool(BulletPool pool) => _pool = pool;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        CheckMaterialization();
    }

    private void CheckMaterialization()
    {
        if (_isMaterialized)
        {
            _ghostBullet.SetActive(false);
            _materializedBullet.SetActive(true);
            _collider.isTrigger = false;
        }
        else
        {
            _ghostBullet.SetActive(true);
            _materializedBullet.SetActive(false);
            _collider.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _passes++;

        if (_passes == _passesNeeded) _isMaterialized = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Activate diving particle system
    }
    
    public void KillBullet()
    {
        Debug.Log($"Bullet {name} Destroyed");

        OnBulletKill?.Invoke();
        _pool?.DespawnBullet(gameObject);

        if (_pool == null) Destroy(gameObject);
    }
}
