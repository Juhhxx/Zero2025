using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameObject _ghostBullet;
    [SerializeField] private GameObject _materializedBullet;
    [SerializeField] private int _passesNeeded;
    private int _passes;

    [SerializeField, ReadOnly] private bool _isMaterialized = false;

    public UnityEvent OnBulletKill;

    private BulletPool _pool;
    public void SetPool(BulletPool pool) => _pool = pool;

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

            LayerMask excluded = LayerMask.GetMask("Player", "Borders");
            _collider.excludeLayers = excluded;
        }
        else
        {
            _ghostBullet.SetActive(true);
            _materializedBullet.SetActive(false);
            _collider.isTrigger = true;

            LayerMask excluded = LayerMask.GetMask("Player", "Borders", "PlayerHurtBox");
            _collider.excludeLayers = excluded;
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

    public void dematerializeBullet()
    {
        _isMaterialized = false;
        _passes = 0;
        _collider.isTrigger = true;
    }
    
    public bool getIsMaterialized()
    {
        return _isMaterialized;
    }
}
