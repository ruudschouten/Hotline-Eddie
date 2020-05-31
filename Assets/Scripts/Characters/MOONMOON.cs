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
        [SerializeField] protected UnityEvent halfASecondBeforeShootEvent;
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
                    if (_rangeCooldownTimer <= 0.5f)
                    {
                        halfASecondBeforeShootEvent.Invoke();
                        ShouldInvokeRangeTell = false;
                    }
                }
            }
        }

        protected override void DetermineAction()
        {
            CalculateDistance();
            CheckForShooting();
            CheckForMelee();
        }

        protected virtual void CheckForShooting()
        {
            // Check if player is outside of melee distance.
            if (!(Distance >= meleeDistance))
            {
                return;
            }
            
            // Check if the ranged attack is ready.
            if (!_isRangeRecharging)
            {
                // Check if the player is between the min and max range distances
                if (Distance >= minRangedDistance && Distance <= maxRangedDistance)
                {
                    Shoot();

                    _isRangeRecharging = true;
                }
                else
                {
                    Move();
                }
            }
            else
            {
                // If the player is still recharging, move towards the player, hoping to get into melee range.
                Move();
            }
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