using Characters;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Consumable : Trigger
{
    [SerializeField] private int healAmount;
    [SerializeField] private int staminaRecovered;
    [SerializeField] private new SpriteRenderer renderer;
    [SerializeField] private Transform pickupRadius;
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

        player.Stamina += staminaRecovered;
        player.Heal(healAmount);
        _isConsumed = true;
        pickupRadius.gameObject.SetActive(false);
        renderer.enabled = false;
        collider.enabled = false;
    }

    protected override void HitObject(Collider2D other)
    {
        if (PlayedAudio)
        {
            return;
        }

        if (_isConsumed)
        {
            return;
        }
        
        if (player.Health == player.MaxHealth)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            PlayedAudio = true;
            triggerSource.Play();

            playerEnteredEvent.Invoke();
        }
    }
}