using Characters;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Consumable : Trigger
{
    [SerializeField] private int healAmount;

    public void Initialize(Player player)
    {
        playerEnteredEvent.AddListener(() => player.Heal(healAmount));
    }
}