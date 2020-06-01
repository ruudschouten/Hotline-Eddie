using System.Collections;
using Characters;
using DefaultNamespace;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Waves
{
    public class Wave : MonoBehaviour
    {
        [SerializeField] private bool overrideSpawn;
        [SerializeField] [ShowIf("overrideSpawn")] private bool isBossWave;
        [SerializeField] [ShowIf("isBossWave")] private FinalBossHelper bossHelper;
        [SerializeField] [ShowIf("isBossWave")] private ConsumableSpawner consumableSpawner;

        [SerializeField] [ShowIf("overrideSpawn")]
        private Transform spawnPoint;

        [SerializeField] private EnemyAmountDictionary enemies;
        [SerializeField] private int enemyCount;
        [SerializeField] private ConsumableAmountDictionary consumables;
        [SerializeField] private AudioClip waveMusic;
        [SerializeField] private bool hasNextWave;
        [SerializeField] private UnityEvent onWaveStartEvent;
        [SerializeField] private UnityEvent onAllEnemiesDefeated;

        [SerializeField] [ShowIf("hasNextWave")]
        private Wave nextWave;

        [SerializeField] [ShowIf("hasNextWave")]
        private bool startNextWaveAfterCooldown;

        [SerializeField] [ShowIf("startNextWaveAfterCooldown")]
        private float secondsToWaitBeforeStartingNextWave;

        public bool Started => _started;

        public Player Player
        {
            get => _player;
            set => _player = value;
        }

        private Player _player;
        private Rect _areaSize;
        private Transform[] _enemySpawnLocations;
        private AudioSource _musicPlayer;

        private WaveManager _manager;

        private bool _started;
        [ShowNonSerializedField] private int _defeatedEnemies;

        public void Initialize(Player player, Rect areaSize, Transform[] spawnLocations, AudioSource musicPlayer,
            WaveManager manager)
        {
            _player = player;
            _areaSize = areaSize;
            if (!overrideSpawn)
            {
                _enemySpawnLocations = spawnLocations;
            }
            else
            {
                _enemySpawnLocations = new[] {spawnPoint};
            }

            _musicPlayer = musicPlayer;

            _manager = manager;
        }

        public void EnemyKilled()
        {
            _defeatedEnemies++;

            if (_defeatedEnemies >= enemyCount)
            {
                onAllEnemiesDefeated.Invoke();

                if (hasNextWave)
                {
                    if (nextWave.Started)
                    {
                        return;
                    }

                    // If all enemies are killed, and a next wave is available, start the next wave.
                    _manager.StartNextWave(true);
                }
            }
        }

        private void StartNextWaveAfter(float seconds)
        {
            StartCoroutine(StartNextWaveRoutine(seconds));
        }

        private IEnumerator StartNextWaveRoutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (nextWave.Started)
            {
                yield break;
            }

            _manager.StartNextWave(true);
        }

        public void Spawn(bool playMusic)
        {
            _started = true;
            onWaveStartEvent.Invoke();

            if (playMusic)
            {
                StartWaveMusic();
            }

            SpawnEnemies();
            SpawnConsumables();

            if (hasNextWave)
            {
                if (startNextWaveAfterCooldown)
                {
                    StartNextWaveAfter(secondsToWaitBeforeStartingNextWave);
                }
            }
        }

        private void SpawnEnemies()
        {
            foreach (var pair in enemies)
            {
                for (var i = 0; i < pair.Value; i++)
                {
                    var enemy = Instantiate(pair.Key, GetRandomPosition(), Quaternion.identity);
                    
                    if (isBossWave)
                    {
                        bossHelper.FirstStage = (Boss) enemy;
                        consumableSpawner.Boss = (Boss) enemy;
                    }
                    enemy.transform.SetParent(transform, true);
                    enemy.Initialize(_player);
                    enemy.OnDeathEvent.AddListener(EnemyKilled);
                }
            }
        }

        private void SpawnConsumables()
        {
            foreach (var pair in consumables)
            {
                for (var i = 0; i < pair.Value; i++)
                {
                    var consumable = Instantiate(pair.Key);
                    consumable.transform.localPosition = GetRandomPositionOnArea();
                    consumable.Initialize(_player);
                }
            }
        }

        public void StartWaveMusic()
        {
            if (_musicPlayer.clip != waveMusic)
            {
                _musicPlayer.Stop();
                _musicPlayer.clip = waveMusic;
                _musicPlayer.Play();
            }
        }

        private Vector2 GetRandomPositionOnArea()
        {
            var x = Random.Range(_areaSize.x, _areaSize.y);
            var y = Random.Range(_areaSize.width, _areaSize.height);
            return new Vector2(x, y);
        }

        private Vector3 GetRandomPosition()
        {
            return _enemySpawnLocations[Random.Range(0, _enemySpawnLocations.Length)].position;
        }
    }
}