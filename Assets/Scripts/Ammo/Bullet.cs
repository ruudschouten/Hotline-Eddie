using System;
using Core;
using NaughtyAttributes;
using UnityEngine;

public class Bullet : MonoRenderer
{
    [SerializeField] private bool shouldUpdate;
    [SerializeField] private float speed;
    [SerializeField] private float timesToPierce;

    [ShowNonSerializedField] private float _enemiesPierced;
    
    public bool ShouldUpdate
    {
        get => shouldUpdate;
        set => shouldUpdate = value;
    }
    
    private void FixedUpdate()
    {
        if (!shouldUpdate)
        {
            return;
        }
        
        // transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
        
        rigidbody.AddRelativeForce(new Vector2(0, speed * Time.deltaTime), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Enemy"))
        {
            // Damage enemy
            
            _enemiesPierced++;

            if (_enemiesPierced >= timesToPierce)
            {
                Destroy(gameObject);
            }
        }
    }
}