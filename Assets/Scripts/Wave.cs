using System.Collections;
using Characters;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wave : MonoBehaviour
{
    [SerializeField] private EnemyAmountDictionary enemies;
    [SerializeField] private ConsumableAmountDictionary consumables;
    [SerializeField] private Rect areaSize;
    [SerializeField] private Player player;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioClip waveMusic;
    [SerializeField] private Transform[] enemySpawnLocations;
    [SerializeField] private bool hasNextWave;

    [SerializeField] [ShowIf("hasNextWave")]
    private Wave nextWave;

    public Player Player
    {
        get => player;
        set => player = value;
    }
    
    [ShowNonSerializedField] private int _defeatedEnemies;

    public void EnemyKilled()
    {
        _defeatedEnemies++;

        if (_defeatedEnemies >= enemies.Keys.Count)
        {
            Debug.Log("Player killed all enemies.");
            
            if (hasNextWave)
            {
                nextWave.Player = player;
                nextWave.Spawn(true);
                enabled = false;
            }
            else
            {
                // TODO: Player won the game, show screen.
            }
        }
    }

    public void SpawnDelayed(float seconds)
    {
        StartCoroutine(SpawnRoutine(seconds));
    }

    private IEnumerator SpawnRoutine(float seconds)
    {
        StartWaveMusic();
        yield return new WaitForSeconds(seconds);
        Spawn(false);
    }
    
    public void Spawn(bool playMusic)
    {
        if (playMusic)
        {
            StartWaveMusic();
        }
        
        SpawnEnemies();
        SpawnConsumables();
    }
    
    private void SpawnEnemies()
    {
        foreach (var pair in enemies)
        {
            for (var i = 0; i < pair.Value; i++)
            {
                var enemy = Instantiate(pair.Key, GetRandomPosition(), Quaternion.identity);
                enemy.Initialize(player);
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
                consumable.Initialize(player);
            }
        }
    }

    private void StartWaveMusic()
    {
        if (musicPlayer.clip != waveMusic)
        {
            musicPlayer.Stop();
            musicPlayer.clip = waveMusic;
            musicPlayer.Play();
        }
    }

    private Vector2 GetRandomPositionOnArea()
    {
        var x = Random.Range(areaSize.x, areaSize.y);
        var y = Random.Range(areaSize.width, areaSize.height);
        return new Vector2(x, y);
    }

    private Vector3 GetRandomPosition()
    {
        return enemySpawnLocations[Random.Range(0, enemySpawnLocations.Length)].position;
    }
}