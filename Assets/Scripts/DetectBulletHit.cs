using System;
using UnityEngine;

public class DetectBulletHit : MonoBehaviour
{
    public event Action OnHitEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.GetComponent<BulletController>()!=null)
        {   
            // if the bullet is materialized, register a hit
            if(other.GetComponent<BulletController>().getIsMaterialized())
            {
                Debug.Log(transform.name + " was Hit");
                OnHitEvent?.Invoke();
            }
        }
    }
}
