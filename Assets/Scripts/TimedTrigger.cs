using System;
using Characters;
using Core;
using UnityEngine;

public class TimedTrigger : MonoRenderer
{
    [SerializeField] private AudioSource triggerSource;
    [SerializeField] private float damageCooldown;
    [SerializeField] private int damageDone;

    private bool _canDamage;
    private float _damageTimer;
    private Player _player;

    private bool _isPlayerInside;
    
    private void Update()
    {
        _damageTimer += Time.deltaTime;
        if (_damageTimer >= damageCooldown)
        {
            _canDamage = true;
        }

        if (_isPlayerInside)
        {
            HitObject(_player.Collider);
        }
    }

    public void DisableCollision()
    {
        collider.enabled = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        HitObject(other.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HitObject(other);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            _isPlayerInside = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
        }
    }

    protected virtual void HitObject(Collider2D other)
    {
        if (!_canDamage)
        {
            return;
        }
        
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            triggerSource.Play();
            if (_player == null)
            {
                _player = other.GetComponent<Player>();
            }
            _player.GetHit(damageDone);
            _damageTimer = 0;
            _canDamage = false;
        }
    }
}