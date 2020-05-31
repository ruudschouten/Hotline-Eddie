using System.Collections;
using UnityEngine;

namespace Characters
{
    public class SecondStage : MonoBehaviour
    {
        [SerializeField] private Boss boss;
        [SerializeField] private Transform legContainer;
        [SerializeField] private float spawnDelay;

        public void SpawnDelayedAtBoss(Boss firstStage)
        {
            StartCoroutine(SpawnRoutine(firstStage));
        }

        private IEnumerator SpawnRoutine(Boss firstStage)
        {
            transform.position = firstStage.transform.position;
            
            boss.Initialize(firstStage.Player);
            yield return new WaitForSeconds(spawnDelay);
            boss.gameObject.SetActive(true);
            legContainer.gameObject.SetActive(true);
            firstStage.Renderer.enabled = false;
        }
    }
}