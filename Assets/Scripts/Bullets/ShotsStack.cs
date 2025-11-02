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
        [field: SerializeField] public Vector2 PlayerPosition { get; private set; }

        public ShotInfo(Vector2 position, Vector2 direction, Vector2 playerPos)
        {
            Position = position;
            Direcion = direction;
            PlayerPosition = playerPos;
        }
    }

    [SerializeField] private List<ShotInfo> _shotStack;
    [SerializeField] private bool _showShots;
    [SerializeField] private List<PlayerController> _players;
    [SerializeField] private GameObject _ghostPrefab;

    private BulletPool _bulletPool;

    private List<BulletController> _bulletsFired;
    private List<GameObject> _ghosts;

    void Awake()
    {
        _bulletsFired = new List<BulletController>();
        _ghosts = new List<GameObject>();
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

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void ShowGhosts()
    {
        foreach (ShotInfo si in _shotStack)
        {
            GameObject newGhost = Instantiate(_ghostPrefab, si.PlayerPosition, Quaternion.identity);

            _ghosts.Add(newGhost);
        }
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void DeleteGhosts()
    {
        foreach (GameObject go in _ghosts) Destroy(go);

        _ghosts.Clear();
    }

    public void AddShot(Vector2 direction, Vector2 position, Vector2 playerPos)
    {
        ShotInfo si = new ShotInfo(position, direction, playerPos);
        _shotStack.Add(si);
    }

    public void ClearBullets()
    {
        foreach (BulletController b in _bulletsFired)
        {
            b.dematerializeBullet();
            b.KillBullet();
        }
        _bulletsFired.Clear();
    }

    public void ResetStack() => _shotStack.Clear();
    
    private void OnDrawGizmos()
    {
        if (_showShots)
        {
            Gizmos.color = Color.red;

            foreach (ShotInfo si in _shotStack)
            {
                Gizmos.DrawLine(si.Position + (Vector2.up * 5), si.Position + (Vector2.down * 5));
                Gizmos.DrawLine(si.Position + (Vector2.left * 5), si.Position + (Vector2.right * 5));
                Gizmos.DrawLine(si.Position, (si.Position + si.Direcion * 10));
            }
        }
    }
}
