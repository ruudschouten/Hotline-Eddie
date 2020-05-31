using Characters;
using UnityEngine;

public class FinalBossHelper : MonoBehaviour
{
    [SerializeField] private SecondStage stageContainer;
    
    private Boss _firstStage;

    public Boss FirstStage
    {
        get => _firstStage;
        set => _firstStage = value;
    }
    
    public void InstantiateSecondStageContainer()
    {
        var stage = Instantiate(stageContainer, Vector3.zero, Quaternion.identity);
        stage.SpawnDelayedAtBoss(_firstStage);
    }
}