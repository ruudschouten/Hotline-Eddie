using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class RangedEnemy : Enemy
    {
        [SerializeField] private AudioClip spawnClip;
        [Space] [SerializeField] private AudioSource bulletSource;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletEmitTransform;
        [SerializeField] private float minRangedDistance;
        [SerializeField] private float maxRangedDistance;
        [SerializeField] private float timeBetweenShots;
        [SerializeField] private UnityEvent onShootEvent;

        private bool _inRange;
        private bool _canShoot;
        private float _shootTimer;

        private float _movementSpeed;

        protected void Awake()
        {
            base.Awake();

            _movementSpeed = minMaxMovementSpeed.RandomBetween();
            source.PlayOneShot(spawnClip);
        }

        protected virtual void Update()
        {
            if (IsDead)
            {
                return;
            }

            if (player.IsDead)
            {
                return;
            }

            // Call Character.Update so the sprite flips when it would otherwise appear upside down.
            FlipTextureIfNeeded();

            LookAt();

            DetermineAction();
            
            if (_inRange)
            {
                if (_canShoot)
                {
                    Shoot();
                }
                else
                {
                    _canShoot = AdvanceAndCheckTimer(ref _shootTimer, timeBetweenShots);
                }
            }
        }

        private void DetermineAction()
        {
            var distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance >= minRangedDistance && distance <= maxRangedDistance)
            {
                _inRange = true;
            }
            else if (distance <= minRangedDistance)
            {
                _inRange = false;
                MoveAway();
            }
            else
            {
                _inRange = false;
                Move();
            }
        }

        private void MoveAway()
        {
            transform.position += transform.right * (_movementSpeed * Time.deltaTime);
        }

        public void Shoot()
        {
            onShootEvent.Invoke();
            var bullet = Instantiate(bulletPrefab, bulletEmitTransform.position, bulletEmitTransform.rotation);
            bullet.ShouldUpdate = true;
            _shootTimer = 0;
            _canShoot = false;
        }
    }
}