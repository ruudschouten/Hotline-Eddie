using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Player : Character
    {
        [SerializeField] private UnityEvent onShoot;
        [SerializeField] private UnityEvent onHeal;

        public UnityEvent OnHeal => onHeal;
        public UnityEvent OnShoot => onShoot;

        public void Awake()
        {
            OnDeathEvent.AddListener(() => PlayRandomClip(deathSounds));
            OnShoot.AddListener(() => PlayRandomClip(hitSounds));
        }

        private void Update()
        {
            FlipTextureIfNeeded();
        }

        public void Heal(int amount)
        {
            health += amount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            onHeal.Invoke();
        }
    }
}