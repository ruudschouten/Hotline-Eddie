using System.Collections;
using Characters;
using UnityEngine;

namespace Waves
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private Wave[] waves;
        [SerializeField] private Transform[] enemySpawnLocations;
        [Space]
        [SerializeField] private Player player;
        [SerializeField] private Rect areaSize;
        [SerializeField] private AudioSource musicPlayer;

        private int _currentWave;

        public void StartDelayed(float seconds)
        {
            StartCoroutine(SpawnRoutine(seconds));
        }
        
        private IEnumerator SpawnRoutine(float seconds)
        {
            waves[_currentWave].Initialize(player, areaSize, enemySpawnLocations, musicPlayer, this);
            yield return new WaitForEndOfFrame();
            waves[_currentWave].StartWaveMusic();
            yield return new WaitForSeconds(seconds);
            waves[_currentWave].Spawn(false);
        }
        
        public void StartNextWave(bool playMusic)
        {
            waves[_currentWave].enabled = false;
            _currentWave++;
            waves[_currentWave].Initialize(player, areaSize, enemySpawnLocations, musicPlayer, this);
            waves[_currentWave].Spawn(playMusic);
        }
    }
}