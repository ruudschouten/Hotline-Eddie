using Characters;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wave : MonoBehaviour
{
    [SerializeField] private EnemyAmountDictionary enemies;
    [SerializeField] private Player player;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioClip waveMusic;
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private bool hasNextWave;

    [SerializeField] [ShowIf("hasNextWave")]
    private Wave nextWave;

    public Player Player
    {
        get => player;
        set => player = value;
    }
    
    [ShowNonSerializedField] private int _defeatedEnemies;

    private void Awake()
    {
        Spawn();
    }

    public void EnemyKilled()
    {
        _defeatedEnemies++;

        if (_defeatedEnemies >= enemies.Keys.Count)
        {
            Debug.Log("Player killed all enemies.");
            
            if (hasNextWave)
            {
                nextWave.Player = player;
                nextWave.Spawn();
                enabled = false;
            }
            else
            {
                // TODO: Player won the game, show screen.
            }
        }
    }

    public void Spawn()
    {
        musicPlayer.Stop();
        musicPlayer.clip = waveMusic;
        musicPlayer.Play();
        
        foreach (var keyPair in enemies)
        {
            for (var i = 0; i < keyPair.Value; i++)
            {
                var enemy = Instantiate(keyPair.Key, GetRandomPosition(), Quaternion.identity);
                enemy.ShouldMove = true;
                enemy.OnDeathEvent.AddListener(EnemyKilled);
                enemy.Player = player;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        return spawnLocations[Random.Range(0, spawnLocations.Length)].position;
    }
}