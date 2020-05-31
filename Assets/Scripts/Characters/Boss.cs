using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Boss : Enemy
    {
        [SerializeField] private AudioClip halfHealthClip;
        [SerializeField] private AudioClip threeQuartersHealthClip;
        [SerializeField] private AudioClip killClip;
        [Space] [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletEmitTransform;
        [SerializeField] private float bulletPlacementRandomization; 
        [SerializeField] private float minRangedDistance;
        [SerializeField] private float maxRangedDistance;
        [SerializeField] private int bulletsToFireAtOnce;
        [SerializeField] private float secondsBetweenShots;
        [SerializeField] private float rangedCooldown;
        [SerializeField] private UnityEvent onShootEvent;

        private float _distance;

        private bool _isMeleeRecharging;
        private float _meleeCooldownTimer;

        // We set this to true from the start, so the boss won't shoot as soon as the player sees him.
        private bool _isRangeRecharging = true;
        private float _rangeCooldownTimer;

        private bool _playedHalfClip;
        private bool _playedThreeQuartersClip;
        private bool _playedKillClip;

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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -transform.right * 5);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position - transform.right * minRangedDistance, new Vector3(1, 1, 1));
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(transform.position - transform.right * maxRangedDistance, new Vector3(1, 1, 1));
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
            if (_isRangeRecharging)
            {
                _isRangeRecharging = !AdvanceAndCheckTimer(ref _rangeCooldownTimer, rangedCooldown);
            }

            if (_isMeleeRecharging)
            {
                _isMeleeRecharging = !AdvanceAndCheckTimer(ref _meleeCooldownTimer, timeBetweenAttacks);
            }
        }

        private void DetermineAction()
        {
            // Get the distance between the player and the boss.
            _distance = Vector3.Distance(transform.position, player.transform.position);
            // Check if player is outside of melee distance.
            if (_distance >= meleeDistance)
            {
                // Check if the ranged attack is ready.
                if (!_isRangeRecharging)
                {
                    // Check if the player is between the min and max range distances
                    if (_distance >= minRangedDistance && _distance <= maxRangedDistance)
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
            else
            {
                if (!_isMeleeRecharging)
                {
                    MeleeAttack();
                    _isMeleeRecharging = true;
                }
            }
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
                
                var randomPlacement = new Vector3(Random.Range(-bulletPlacementRandomization, bulletPlacementRandomization),
                    Random.Range(-bulletPlacementRandomization, bulletPlacementRandomization));
                var bullet = Instantiate(bulletPrefab, bulletEmitTransform.position + randomPlacement, bulletEmitTransform.rotation);
                bullet.ShouldUpdate = true;

                yield return new WaitForSecondsRealtime(secondsBetweenShots);
            }

            _isRangeRecharging = true;
        }
    }
}