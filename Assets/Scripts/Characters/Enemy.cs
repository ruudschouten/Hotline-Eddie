using Helpers;
using NaughtyAttributes;
using UnityEngine;

namespace Characters
{
    public class Enemy : Character
    {
        [SerializeField] private bool hasSpawnClip;
        [SerializeField] [ShowIf("hasSpawnClip")] private AudioClip spawnClip;
        [SerializeField] protected bool shouldMove;
        [SerializeField] protected MinMaxFloat minMaxMovementSpeed;
        [SerializeField] protected int meleeDamage;
        [SerializeField] protected float timeBetweenAttacks;
        [SerializeField] protected float meleeDistance;
        [SerializeField] protected Sprite deadSprite;
        [SerializeField] protected Player player;

        protected float MovementSpeed;
        private bool _canHit;
        private bool _canAttack;
        private float _attackTimer;

        public Player Player => player;

        protected void Awake()
        {
            base.Awake();

            MovementSpeed = minMaxMovementSpeed.RandomBetween();
            
            if (hasSpawnClip)
            {
                source.PlayOneShot(spawnClip);
            }
        }

        public void Initialize(Player player)
        {
            shouldMove = true;
            this.player = player;
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

            if (!shouldMove)
            {
                return;
            }

            if (_canHit)
            {
                if (_canAttack)
                {
                    Attack();
                }
                else
                {
                    _canAttack = AdvanceAndCheckTimer(ref _attackTimer, timeBetweenAttacks);
                }
            }

            AttackOrMove();
        }

        private void AttackOrMove()
        {
            var dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist <= meleeDistance)
            {
                _canHit = true;
            }
            else
            {
                _canHit = false;
                Move();
            }
        }

        protected void Move()
        {
            transform.position -= transform.right * (MovementSpeed * Time.deltaTime);
        }

        private void Attack()
        {
            onHitTarget.Invoke();
            player.GetHit(meleeDamage);
            _canHit = false;
            _canAttack = false;
            _attackTimer = 0f;
        }

        protected void LookAt()
        {
            var angle = LookAtHelper.GetAngleAtTarget(player.transform.position, transform.position) - 180;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        protected bool AdvanceAndCheckTimer(ref float timer, float valueToHit)
        {
            timer += Time.deltaTime;
            return timer >= valueToHit;
        }

        public void OnDeath()
        {
            renderer.sortingOrder -= 1;
            renderer.sprite = deadSprite;
            collider.enabled = false;
        }
    }
}