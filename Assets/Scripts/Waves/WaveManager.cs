using System.Collections;
using Characters;
using NaughtyAttributes;
using UnityEngine;

namespace Waves
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField][ReorderableList] private Wave[] waves;
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
            var waveToStart = waves[_currentWave];
            waveToStart.Initialize(player, areaSize, enemySpawnLocations, musicPlayer, this);
            yield return new WaitForEndOfFrame();
            waveToStart.StartWaveMusic();
            yield return new WaitForSeconds(seconds);
            waveToStart.Spawn(false);
        }
        
        public void StartNextWave(bool playMusic)
        {
            _currentWave++;
            waves[_currentWave].Initialize(player, areaSize, enemySpawnLocations, musicPlayer, this);
            waves[_currentWave].Spawn(playMusic);
        }
    }
}