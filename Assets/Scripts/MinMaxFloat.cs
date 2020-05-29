using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class MinMaxFloat
{
    [SerializeField] private float min;
    [SerializeField] private float max;

    public float Min
    {
        get => min;
        set => min = value;
    }

    public float Max
    {
        get => max;
        set => max = value;
    }

    public float RandomBetween()
    {
        return Random.Range(min, max);
    }
}