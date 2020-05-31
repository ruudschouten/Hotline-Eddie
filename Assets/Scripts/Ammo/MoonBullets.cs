using Characters;
using UnityEngine;

namespace Ammo
{
    public class MoonBullets : EnemyBullet
    {
        [SerializeField] private float slowerBulletTime;
        [SerializeField] private float slowSpeed;

        private float _slowerBulletTimer;
        private float _speedPercentage;
        private float _slowSpeed;
        private bool _shouldSlow;

        protected override void FixedUpdate()
        {
            if (_shouldSlow)
            {
                if (_slowerBulletTimer > slowerBulletTime)
                {
                    _slowerBulletTimer += Time.deltaTime;

                    _speedPercentage = _slowerBulletTimer / slowerBulletTime;

                    _slowSpeed = slowSpeed * _speedPercentage;
                }
                else
                {
                    _shouldSlow = false;
                }

                transform.position += transform.up * (_slowSpeed * Time.deltaTime);
            }
            else
            {
                transform.position += transform.up * (speed * Time.deltaTime);
            }
        }
    }
}