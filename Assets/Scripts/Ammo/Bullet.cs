﻿using System;
using Characters;
using Core;
using NaughtyAttributes;
using UnityEngine;

public class Bullet : MonoRenderer
{
    [SerializeField] private bool shouldUpdate;
    [SerializeField] private int damage;
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

        transform.position += transform.up * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            var obstacle = other.GetComponent<Obstacle>();
            if (obstacle.StopsBullets)
            {
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            // Damage enemy
            var enemy = other.GetComponent<Enemy>();
            if (enemy.IsDead)
            {
                return;
            }

            enemy.GetHit(damage);

            _enemiesPierced++;

            if (_enemiesPierced >= timesToPierce)
            {
                Destroy(gameObject);
            }
        }
    }
}