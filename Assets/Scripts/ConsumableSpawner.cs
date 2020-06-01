using Characters;
using UnityEngine;

namespace DefaultNamespace
{
    public class ConsumableSpawner : MonoBehaviour
    {
        [SerializeField] private Consumable consumable;
        [SerializeField] private Player player;

        public Boss Boss { get; set; }

        public void SpawnAtBoss()
        {
            if (Boss == null)
            {
                Spawn();
            }
            else
            {
                var spawn = Instantiate(consumable, Boss.transform.position, Quaternion.identity);
                spawn.Initialize(player);
            }
        }
        
        public void Spawn()
        {
            var spawn = Instantiate(consumable, Vector3.zero, Quaternion.identity);
            spawn.Initialize(player);
        }
    }
}