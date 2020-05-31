using Characters;
using UnityEngine;

namespace Ammo
{
    public class EnemyBullet : Bullet
    {
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Obstacle"))
            {
                var obstacle = other.GetComponent<Obstacle>();
                if (obstacle.StopsBullets)
                {
                    Destroy(gameObject);
                }
            }
            
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<Player>();
                player.GetHit(damage);
                Destroy(gameObject);
            }
        }
    }
}