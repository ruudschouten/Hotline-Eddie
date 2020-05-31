using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class MOONMOON : Boss
    {
        [SerializeField] protected float bulletPlacementRandomization;
        [SerializeField] protected int bulletsToFireAtOnce;
        [SerializeField] protected float secondsBetweenShots;
        [Space] [SerializeField] protected Bullet bulletPrefab;
        [SerializeField] protected Transform bulletEmitTransform;
        [SerializeField] protected float minRangedDistance;
        [SerializeField] protected float maxRangedDistance;
        [SerializeField] protected float rangedCooldown;
        [SerializeField] protected UnityEvent secondBeforeShootEvent;
        [SerializeField] protected UnityEvent onShootEvent;

        // We set this to true from the start, so the boss won't shoot as soon as the player sees him.
        private bool _isRangeRecharging = true;
        private float _rangeCooldownTimer;

        private void Shoot()
        {
            _rangeCooldownTimer = 0;
            StartCoroutine(ShootRoutine());
        }

        protected override void UpdateTimers()
        {
            base.UpdateTimers();

            if (_isRangeRecharging)
            {
                _isRangeRecharging = !AdvanceAndCheckTimer(ref _rangeCooldownTimer, rangedCooldown);
                if (ShouldInvokeRangeTell)
                {
                    if (_rangeCooldownTimer <= 1f)
                    {
                        secondBeforeShootEvent.Invoke();
                        ShouldInvokeRangeTell = false;
                    }
                }
            }
        }

        protected override void DetermineAction()
        {
            CalculateDistance();
            CheckForShooting();
            if (ShouldMelee())
            {
                return;
            }

            if (!shouldMove)
            {
                return;
            }

            Move();
        }

        protected virtual void CheckForShooting()
        {
            // Check if player is outside of melee distance.
            if (!(Distance >= meleeDistance))
            {
                return;
            }

            // Check if the ranged attack is ready.
            if (_isRangeRecharging)
            {
                return;
            }

            // Check if the player is between the min and max range distances
            if (!(Distance >= minRangedDistance) || !(Distance <= maxRangedDistance))
            {
                return;
            }

            Shoot();

            _isRangeRecharging = true;
        }

        private IEnumerator ShootRoutine()
        {
            for (var i = 0; i < bulletsToFireAtOnce; i++)
            {
                if (i % 10 == 0)
                {
                    onShootEvent.Invoke();
                }

                var randomPlacement = new Vector3(
                    Random.Range(-bulletPlacementRandomization, bulletPlacementRandomization),
                    Random.Range(-bulletPlacementRandomization, bulletPlacementRandomization));
                var bullet = Instantiate(bulletPrefab, bulletEmitTransform.position + randomPlacement,
                    bulletEmitTransform.rotation);
                bullet.ShouldUpdate = true;

                yield return new WaitForSecondsRealtime(secondsBetweenShots);
            }

            ShouldInvokeRangeTell = true;
            _isRangeRecharging = true;
        }
    }
}