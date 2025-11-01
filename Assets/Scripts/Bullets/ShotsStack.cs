using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ShotsStack : MonoBehaviour
{
    [Serializable]
    public struct ShotInfo
    {
        [field: SerializeField] public Vector2 Position { get; private set; }
        [field: SerializeField] public Vector2 Direcion { get; private set; }

        public ShotInfo(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direcion = direction;
        }
    }

    [SerializeField] private List<ShotInfo> _shotStack;
    [SerializeField] private bool _showShots;
    [SerializeField] private List<PlayerController> _players;

    private BulletPool _bulletPool;

    private List<BulletController> _bulletsFired;

    void Awake()
    {
        _bulletsFired = new List<BulletController>();
    }

    private void Start()
    {
        _bulletPool = FindAnyObjectByType<BulletPool>();

        foreach (PlayerController p in _players) p.OnShootingInputEvent += AddShot;
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void Shoot()
    {
        if (_shotStack.Count == 0) return;

        foreach (ShotInfo si in _shotStack)
        {
            GameObject newBullet = _bulletPool.SpawnBullet(si.Direcion, si.Position);

            _bulletsFired.Add(newBullet.GetComponent<BulletController>());
        }
    }

    public void AddShot(Vector2 direction, Vector2 position)
    {
        ShotInfo si = new ShotInfo(position, direction);
        _shotStack.Add(si);
    }

    public void ClearBullets()
    {
        foreach (BulletController b in _bulletsFired) b.KillBullet();
        _bulletsFired.Clear();
    }

    public void ResetStack() => _shotStack.Clear();
    
    private void OnDrawGizmos()
    {
    }
}
