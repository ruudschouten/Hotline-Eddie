using Characters;
using UnityEngine;

namespace DefaultNamespace
{
    public class ConsumableSpawner : MonoBehaviour
    {
        [SerializeField] private Consumable consumable;
        [SerializeField] private Player player;

        public void Spawn()
        {
            var spawn = Instantiate(consumable, Vector3.zero, Quaternion.identity);
            spawn.Initialize(player);
        }
    }
}