using UnityEngine;

namespace Characters
{
    public class NOOMNOOM : Boss
    {
        [SerializeField] private float pullPower;
        [SerializeField] private float pullDuration;
        [SerializeField] private float pullCooldown;

        private float _pullActiveTimer;
        private bool _isPulling;
        private float _pullCooldownTimer;
        private bool _isPullRecharging = true;

        protected override void UpdateTimers()
        {
            base.UpdateTimers();

            if (_isPullRecharging)
            {
                _isPullRecharging = !AdvanceAndCheckTimer(ref _pullCooldownTimer, pullCooldown);
            }
            else
            {
                if (_isPulling)
                {
                    _pullActiveTimer += Time.deltaTime;
                    if (_pullActiveTimer >= pullDuration)
                    {
                        _isPulling = false;
                        _isPullRecharging = true;
                        _pullCooldownTimer = 0;
                    }
                }
            }
        }

        protected override void DetermineAction()
        {
            _isPulling = !_isPullRecharging;
            if (!_isPulling)
            {
                _pullActiveTimer = 0;
            }
            else
            {
                PullPlayer();
            }

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

        private void PullPlayer()
        {
            player.transform.position += transform.right * (pullPower * Time.deltaTime);
        }
    }
}