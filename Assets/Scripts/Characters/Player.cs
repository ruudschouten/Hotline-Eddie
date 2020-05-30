using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Player : Character
    {
        [SerializeField] private UnityEvent onDamageReceived;

        public UnityEvent OnDamageReceived => onDamageReceived;

        public override void GetHit(int damage)
        {
            base.GetHit(damage);

            onDamageReceived.Invoke();
        }
    }
}