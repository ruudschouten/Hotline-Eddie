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

        protected bool ShouldInvokeRangeTell;

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
                if (ShouldInvokeRangeTell)
                {
                    if (_rangeCooldownTimer <= 0.5f)
                    {
                        halfASecondBeforeShootEvent.Invoke();
                        ShouldInvokeRangeTell = false;
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
            // Get the distance between the player and the boss.
            Distance = Vector3.Distance(transform.position, player.transform.position);
            
            // Check if player is outside of melee distance.
            if (Distance >= meleeDistance)
            {
                // Check if the ranged attack is ready.
                if (!IsRangeRecharging)
                {
                    // Check if the player is between the min and max range distances
                    if (Distance >= minRangedDistance && Distance <= maxRangedDistance)
                    {
                        Shoot();

                        IsRangeRecharging = true;
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
            else
            {
                if (!IsMeleeRecharging)
                {
                    MeleeAttack();
                    IsMeleeRecharging = true;
                }
            }
        }

        public virtual void MeleeAttack()
        {
            _meleeCooldownTimer = 0;
            onHitTarget.Invoke();
            player.GetHit(meleeDamage);
        }

        public virtual void Shoot()
        {
            _rangeCooldownTimer = 0;
        }
    }
}