using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Boss : Enemy
    {
        [SerializeField] protected AudioClip halfHealthClip;
        [SerializeField] protected AudioClip threeQuartersHealthClip;
        [SerializeField] protected AudioClip killClip;

        protected float Distance;

        protected bool IsMeleeRecharging;
        private float _meleeCooldownTimer;


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

            UpdateTimers();

            DetermineAction();
        }

        protected virtual void UpdateTimers()
        {
            if (IsMeleeRecharging)
            {
                IsMeleeRecharging = !AdvanceAndCheckTimer(ref _meleeCooldownTimer, timeBetweenAttacks);
            }
        }

        protected virtual void DetermineAction()
        {
            CalculateDistance();
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

        protected virtual void CalculateDistance()
        {
            // Get the distance between the player and the boss.
            Distance = Vector3.Distance(transform.position, player.transform.position);
        }

        protected virtual bool ShouldMelee()
        {
            if (!(Distance <= meleeDistance))
            {
                return false;
            }
            if (IsMeleeRecharging)
            {
                return false;
            }
            MeleeAttack();
            IsMeleeRecharging = true;
            return true;
        }

        public virtual void MeleeAttack()
        {
            _meleeCooldownTimer = 0;
            onHitTarget.Invoke();
            player.GetHit(meleeDamage);
        }
    }
}