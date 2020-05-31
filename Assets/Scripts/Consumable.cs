using Characters;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Consumable : Trigger
{
    [SerializeField] private int healAmount;
    [SerializeField] private new SpriteRenderer renderer;
    [SerializeField] private Player player;

    private bool _isConsumed;

    public void Initialize(Player player)
    {
        this.player = player;
    }

    public void HealPlayer()
    {
        if (_isConsumed)
        {
            return;
        }

        // Only consume if the players' health is actually low
        if (player.Health == player.MaxHealth)
        {
            return;
        }

        player.Heal(healAmount);
        _isConsumed = true;
        renderer.enabled = false;
        collider.enabled = false;
    }
}