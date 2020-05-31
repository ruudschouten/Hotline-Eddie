using Characters;
using UnityEngine;

public class EnemyCuller : MonoBehaviour
{
    [SerializeField] private Transform waveManager;
    
    public void CullEnemies()
    {
        foreach (Transform wave in waveManager)
        {
            foreach (Transform child in wave)
            {
                if (child.CompareTag("Enemy"))
                {
                    var enemy = GetComponent<Enemy>();
                    if (enemy == null)
                    {
                        continue;
                    }
                    if (enemy.IsDead)
                    {
                        continue;
                    }
                    enemy.GetHit(9999999);
                }
            }
        }
    }
}