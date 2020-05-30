using System.Collections;
using Characters;
using NaughtyAttributes;
using UnityEngine;
using Waves;
using Random = UnityEngine.Random;

public class Wave : MonoBehaviour
{
    [SerializeField] private EnemyAmountDictionary enemies;
    [SerializeField] private ConsumableAmountDictionary consumables;
    [SerializeField] private AudioClip waveMusic;
    [SerializeField] private bool hasNextWave;

    [SerializeField] [ShowIf("hasNextWave")]
    private Wave nextWave;

    [SerializeField] [ShowIf("hasNextWave")]
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
    private int _defeatedEnemies;

    public void Initialize(Player player, Rect areaSize, Transform[] spawnLocations, AudioSource musicPlayer,
        WaveManager manager)
    {
        _player = player;
        _areaSize = areaSize;
        _enemySpawnLocations = spawnLocations;
        _musicPlayer = musicPlayer;

        _manager = manager;
    }

    public void EnemyKilled()
    {
        _defeatedEnemies++;

        if (_defeatedEnemies >= enemies.Keys.Count)
        {
            if (hasNextWave)
            {
                if (nextWave.Started)
                {
                    return;
                }

                // If all enemies are killed, and a next wave is available, start the next wave.
                _manager.StartNextWave(true);
            }
            else
            {
                // TODO: Player won the game, show screen.
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

        if (playMusic)
        {
            StartWaveMusic();
        }

        SpawnEnemies();
        SpawnConsumables();

        if (hasNextWave)
        {
            StartNextWaveAfter(secondsToWaitBeforeStartingNextWave);
        }
    }

    private void SpawnEnemies()
    {
        foreach (var pair in enemies)
        {
            for (var i = 0; i < pair.Value; i++)
            {
                var enemy = Instantiate(pair.Key, GetRandomPosition(), Quaternion.identity);
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