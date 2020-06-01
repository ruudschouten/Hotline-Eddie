using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class NOOMNOOM : Boss
    {
        [SerializeField] private BodyPartHelper bodyPartHelper;
        [SerializeField] private float teleportRangeModifier;
        [SerializeField] private Rect areaSize;
        [SerializeField] private float pullPower;
        [SerializeField] private float pullDuration;
        [SerializeField] private float pullCooldown;
        [SerializeField] private TimedTrigger poisonPile;
        [SerializeField] private Transform poisonPileLocation;
        [SerializeField] private UnityEvent secondBeforePullEvent;
        [SerializeField] private UnityEvent secondAfterPullEvent;
        [SerializeField] private UnityEvent onPullEvent;

        // This is used so the _invokedSecondAfterPullEvent doesn't trigger a second after spawning.
        private bool _firstAfterInvoke = true;

        private float _pullActiveTimer;
        private bool _isPulling;
        private float _pullCooldownTimer;
        private bool _isPullRecharging = true;

        private bool _invokedSecondBeforePullEvent;
        private bool _invokedSecondAfterPullEvent;
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

                if (_pullCooldownTimer >= 1f)
                {
                    if (!_invokedSecondAfterPullEvent)
                    {
                        if (_firstAfterInvoke)
                        {
                            _invokedSecondAfterPullEvent = true;

                            _invokedPullEvent = false;
                            _firstAfterInvoke = false;
                        }
                        else
                        {
                            secondAfterPullEvent.Invoke();
                            _invokedSecondAfterPullEvent = true;

                            _invokedPullEvent = false;
                        }
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
                        _invokedSecondAfterPullEvent = false;
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

        public void SpitPoison()
        {
            Instantiate(poisonPile, poisonPileLocation.position, poisonPileLocation.rotation);
        }

        public void TeleportBehind()
        {
            transform.position = player.transform.position - transform.right * (Distance * teleportRangeModifier);

            if (transform.position.x < areaSize.x)
            {
                var diff = transform.position.x - areaSize.x;
                transform.position -= new Vector3(diff, 0, 0);                    
            }
            else if(transform.position.x > areaSize.y)
            {
                var diff = transform.position.x - areaSize.x;
                transform.position -= new Vector3(diff, 0, 0);
            }
            if (transform.position.y < areaSize.width)
            {
                var diff = transform.position.y - areaSize.width;
                transform.position -= new Vector3(0, diff, 0);                    
            }
            else if(transform.position.y > areaSize.height)
            {
                var diff = transform.position.y - areaSize.height;
                transform.position -= new Vector3(0, diff, 0);
            }

            bodyPartHelper.Teleport(transform);
            LookAt();
        }
    }
}