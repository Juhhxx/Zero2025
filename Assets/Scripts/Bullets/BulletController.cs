using NaughtyAttributes;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private int _passesNeeded;
    private int _passes;

    [SerializeField, ReadOnly] private bool _isMaterialized = false;

    private SpriteRenderer _spr;
    private Collider2D _collider;

    private void Start()
    {
        _spr = GetComponentInChildren<SpriteRenderer>();
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
            _spr.color = Color.white;
            _collider.isTrigger = false;
        }
        else
        {
            _spr.color = Color.purple;
            _collider.isTrigger = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _passes++;

        if (_passes == _passesNeeded) _isMaterialized = true;
    }
}
