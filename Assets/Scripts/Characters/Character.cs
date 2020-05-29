using Core;
using UnityEditor.Connect;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Character : MonoRenderer
    {
        [SerializeField] protected int maxHealth;
        [SerializeField] protected int health;

        [SerializeField] protected UnityEvent onDeath;

        public bool IsDead => health <= 0;
        
        public void GetHit(int damage)
        {
            health -= damage;

            if (IsDead)
            {
                Debug.Log($"I have died {transform.name}");
                onDeath.Invoke();
            }
        }

        protected void Update()
        {
            var euler = transform.rotation.eulerAngles;

            renderer.flipY = euler.z > 90 && euler.z < 270;

            if (euler.z >= 360)
            {
                euler.z -= 360;
            }
            if(euler.z <= 360)
            {
                euler.z += 360;
            }
        }
    }
}