using Core;
using UnityEngine;

namespace Characters
{
    public class Character : MonoRenderer
    {
        [SerializeField] protected int maxHealth;
        [SerializeField] protected int health;
    }
}