using Helpers;
using UnityEngine;

namespace Characters
{
    public class Enemy : Character
    {
        [SerializeField] private bool shouldMove;
        [SerializeField] private MinMaxFloat minMaxMovementSpeed;
        [SerializeField] private int damage;
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float attackDistance;
        [SerializeField] private Sprite deadSprite;
        [SerializeField] private Player player;

        private Vector3 _targetPosition;

        private float _movementSpeed;
        private bool _canHit;
        private bool _canAttack;
        private float _attackTimer;

        private void Awake()
        {
            base.Awake();

            _movementSpeed = minMaxMovementSpeed.RandomBetween();
        }

        public bool ShouldMove
        {
            get => shouldMove;
            set => shouldMove = value;
        }

        public Player Player
        {
            get => player;
            set => player = value;
        }

        private void Update()
        {
            if (IsDead)
            {
                return;
            }

            // Call Character.Update so the sprite flips when it would otherwise appear upside down.
            base.Update();

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
                    _attackTimer += Time.deltaTime;
                    if (_attackTimer >= timeBetweenAttacks)
                    {
                        _canAttack = true;
                    }
                }
            }


            var dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist <= attackDistance)
            {
                _canHit = true;
            }
            else
            {
                transform.position -= transform.right * (_movementSpeed * Time.deltaTime);
                _canHit = false;
            }
        }

        private void Attack()
        {
            onHitTarget.Invoke();
            player.GetHit(damage);
            _canHit = false;
            _canAttack = false;
            _attackTimer = 0f;
        }

        protected void LookAt()
        {
            var angle = LookAtHelper.GetAngleAtTarget(player.transform.position, transform.localPosition) - 180;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        public void OnDeath()
        {
            renderer.sprite = deadSprite;
            collider.enabled = false;
            renderer.sortingOrder = -1;
        }
    }
}