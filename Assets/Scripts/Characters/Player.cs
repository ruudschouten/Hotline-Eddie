using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Player : Character
    {
        [SerializeField] private float stamina;
        [SerializeField] private float staminaRegen;
        [SerializeField] private float timeToWaitForStaminaRegen;
        [SerializeField] private UnityEvent onShoot;
        [SerializeField] private UnityEvent onHeal;
        [SerializeField] private UnityEvent onStaminaUse;
        [SerializeField] private UnityEvent onStaminaRecovered;

        public float MaxStamina => _maxStamina;
        
        private float _maxStamina;
        private float _staminaRegenTimer;
        private bool _isStaminaRegenerating;
        
        public float Stamina
        {
            get => stamina;
            set
            {
                stamina = value;
                _isStaminaRegenerating = false;
                _staminaRegenTimer = 0;
                onStaminaUse.Invoke();
            }
        }
        
        public UnityEvent OnHeal => onHeal;
        public UnityEvent OnShoot => onShoot;

        public void Awake()
        {
            OnDeathEvent.AddListener(() => PlayRandomClip(deathSounds));
            OnShoot.AddListener(() => PlayRandomClip(hitSounds));

            _maxStamina = stamina;
        }

        private void Update()
        {
            FlipTextureIfNeeded();

            if (_maxStamina >= stamina)
            {
                if (_isStaminaRegenerating)
                {
                    stamina += staminaRegen * Time.deltaTime;
                    onStaminaRecovered.Invoke();
                }
                else
                {
                    _staminaRegenTimer += Time.deltaTime;
                    if (_staminaRegenTimer > timeToWaitForStaminaRegen)
                    {
                        _isStaminaRegenerating = true;
                    }
                }
            }
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