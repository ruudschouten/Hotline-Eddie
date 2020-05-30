using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Player : Character
    {
        [SerializeField] private UnityEvent onShoot;
        [SerializeField] private UnityEvent onDamageReceived;

        public UnityEvent OnDamageReceived => onDamageReceived;
        public UnityEvent OnShoot => onShoot;

        public void Awake()
        {
            OnDeathEvent.AddListener(() => PlayRandomClip(deathSounds));
            OnShoot.AddListener(() => PlayRandomClip(hitSounds));
        }

        public override void GetHit(int damage)
        {
            base.GetHit(damage);

            onDamageReceived.Invoke();
        }
    }
}