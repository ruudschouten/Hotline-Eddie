using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Character : MonoRenderer
    {
        [SerializeField] protected int maxHealth;
        [SerializeField] protected int health;

        [Space] [SerializeField] protected AudioSource source;
        [SerializeField] private Vector2 minMaxAudioPitch;
        [SerializeField] protected AudioClip[] hitSounds;
        [SerializeField] protected AudioClip[] deathSounds;
        [Space] [SerializeField] protected UnityEvent onHitTarget;
        [SerializeField] protected UnityEvent onDeath;
        [SerializeField] private UnityEvent onGetHit;

        public int Health => health;
        public int MaxHealth => maxHealth;

        public UnityEvent OnDeathEvent => onDeath;
        public UnityEvent OnHitTargetEvent => onHitTarget;

        public UnityEvent OnGetHit => onGetHit;

        public bool IsDead => health <= 0;

        protected void Awake()
        {
            OnDeathEvent.AddListener(() => PlayRandomClip(deathSounds));
            onHitTarget.AddListener(() => PlayRandomClip(hitSounds));
        }

        protected void PlayClip(AudioClip clip)
        {
            source.pitch = Random.Range(minMaxAudioPitch.x, minMaxAudioPitch.y);
            source.PlayOneShot(clip);
        }

        protected void PlayRandomClip(AudioClip[] clips)
        {
            source.pitch = Random.Range(minMaxAudioPitch.x, minMaxAudioPitch.y);
            source.PlayOneShot(GetRandom(clips));
        }

        protected AudioClip GetRandom(AudioClip[] clips)
        {
            return clips[Random.Range(0, clips.Length)];
        }

        public virtual void GetHit(int damage)
        {
            health -= damage;

            if (IsDead)
            {
                Debug.Log($"I have died {transform.name}");
                onDeath.Invoke();
            }
            else
            {
                onGetHit.Invoke();
            }
        }

        public void FlipTextureIfNeeded()
        {
            var euler = transform.rotation.eulerAngles;

            renderer.flipY = euler.z > 90 && euler.z < 270;

            if (euler.z >= 360)
            {
                euler.z -= 360;
            }

            if (euler.z <= 360)
            {
                euler.z += 360;
            }
        }
    }
}