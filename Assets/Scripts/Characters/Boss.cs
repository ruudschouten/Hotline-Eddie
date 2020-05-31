using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Boss : Enemy
    {
        [SerializeField] protected AudioClip halfHealthClip;
        [SerializeField] protected AudioClip threeQuartersHealthClip;
        [SerializeField] protected AudioClip killClip;
        [Space] [SerializeField] protected Bullet bulletPrefab;
        [SerializeField] protected Transform bulletEmitTransform;
        [SerializeField] protected float bulletPlacementRandomization;
        [SerializeField] protected float minRangedDistance;
        [SerializeField] protected float maxRangedDistance;
        [SerializeField] protected int bulletsToFireAtOnce;
        [SerializeField] protected float secondsBetweenShots;
        [SerializeField] protected float rangedCooldown;
        [SerializeField] protected UnityEvent halfASecondBeforeShootEvent;
        [SerializeField] protected UnityEvent onShootEvent;

        protected float Distance;

        protected bool IsMeleeRecharging;
        private float _meleeCooldownTimer;

        // We set this to true from the start, so the boss won't shoot as soon as the player sees him.
        protected bool IsRangeRecharging = true;
        private float _rangeCooldownTimer;

        private bool _playedHalfClip;
        private bool _playedThreeQuartersClip;
        private bool _playedKillClip;

        private bool _shouldInvokeRangeTell;

        public void PlayHealthClips()
        {
            var percentage = (float) Health / (float) MaxHealth * 100;
            if (!_playedThreeQuartersClip)
            {
                if (Between(51, 75, (int) percentage))
                {
                    PlayClip(threeQuartersHealthClip);
                    _playedThreeQuartersClip = true;
                }
            }

            if (!_playedHalfClip)
            {
                if (Between(1, 50, (int) percentage))
                {
                    PlayClip(halfHealthClip);
                    _playedHalfClip = true;
                }
            }
        }

        private bool Between(int lowest, int highest, int value)
        {
            return value <= highest && value >= lowest;
        }

        private void Update()
        {
            if (IsDead)
            {
                return;
            }

            if (player.IsDead)
            {
                if (!_playedKillClip)
                {
                    source.PlayOneShot(killClip);
                    _playedKillClip = true;
                }

                return;
            }

            FlipTextureIfNeeded();

            LookAt();

            if (!shouldMove)
            {
                return;
            }

            UpdateTimers();

            DetermineAction();
        }

        private void UpdateTimers()
        {
            if (IsRangeRecharging)
            {
                IsRangeRecharging = !AdvanceAndCheckTimer(ref _rangeCooldownTimer, rangedCooldown);
                if (_shouldInvokeRangeTell)
                {
                    if (_rangeCooldownTimer <= 0.5f)
                    {
                        halfASecondBeforeShootEvent.Invoke();
                        _shouldInvokeRangeTell = false;
                    }
                }
            }

            if (IsMeleeRecharging)
            {
                IsMeleeRecharging = !AdvanceAndCheckTimer(ref _meleeCooldownTimer, timeBetweenAttacks);
            }
        }

        protected virtual void DetermineAction() 
        {
        
        }

        public void MeleeAttack()
        {
            _meleeCooldownTimer = 0;
            onHitTarget.Invoke();
            player.GetHit(meleeDamage);
        }

        public void Shoot()
        {
            _rangeCooldownTimer = 0;
            StartCoroutine(ShootRoutine());
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

            _shouldInvokeRangeTell = true;
            IsRangeRecharging = true;
        }
    }
}