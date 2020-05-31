using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class NOOMNOOM : Boss
    {
        [SerializeField] private float pullPower;
        [SerializeField] private float pullDuration;
        [SerializeField] private float pullCooldown;
        [SerializeField] private UnityEvent secondBeforePullEvent;
        [SerializeField] private UnityEvent onPullEvent;

        private float _pullActiveTimer;
        private bool _isPulling;
        private float _pullCooldownTimer;
        private bool _isPullRecharging = true;

        private bool _invokedSecondBeforePullEvent;
        private bool _invokedPullEvent;

        protected override void UpdateTimers()
        {
            base.UpdateTimers();

            if (_isPullRecharging)
            {
                _isPullRecharging = !AdvanceAndCheckTimer(ref _pullCooldownTimer, pullCooldown);
                
                if (_pullCooldownTimer >= pullCooldown - 1f)
                {
                    if (!_invokedSecondBeforePullEvent)
                    {
                        secondBeforePullEvent.Invoke();
                        _invokedSecondBeforePullEvent = true;
                                
                        _invokedPullEvent = false;
                    }
                }
            }
            else
            {
                if (_isPulling)
                {
                    _pullActiveTimer += Time.deltaTime;
                    if (_pullActiveTimer >= pullDuration)
                    {
                        _invokedSecondBeforePullEvent = false;
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
                if (!_invokedPullEvent)
                {
                    onPullEvent.Invoke();
                    _invokedPullEvent = true;
                }
                
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