using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private int _initialPool;

    [SerializeField] private GameObject _objectPrefab;

    private Stack<GameObject> _pool;

    private void Start()
    {
        _pool = new Stack<GameObject>();

        if (_initialPool > 0)
        {
            for (int i = 0; i < _initialPool; i++)
            {
                GameObject newChalk = CreateObject();
                newChalk.SetActive(false);
            }
        }
    }

    private GameObject CreateObject()
    {
        GameObject newObject = Instantiate(_objectPrefab);
        newObject.GetComponent<BulletController>().SetPool(this);
        _pool.Push(newObject);
        return newObject;
    }

    public GameObject SpawnBullet(Vector2 direction)
    {
        GameObject newBullet = _pool.Pop();

        BulletMovement bulletMove = newBullet.GetComponent<BulletMovement>();

        bulletMove.Move(direction);
        newBullet.SetActive(true);

        return newBullet;
    }

    public void DespawnBullet(GameObject bullet)
    {
        _pool.Push(bullet);
        bullet.SetActive(false);
    }
}
