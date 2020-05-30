using Core;
using UnityEngine;

public class Obstacle : MonoRenderer
{
    [SerializeField] private bool stopsBullets = true;

    public bool StopsBullets
    {
        get => stopsBullets;
        set => stopsBullets = value;
    }
}