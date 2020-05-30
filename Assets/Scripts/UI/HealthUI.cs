using Characters;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;

    public void UpdateHealth()
    { 
        var index = 10;

        foreach (var healthObject in hearts)
        {
            if (player.Health > index)
            {
                healthObject.sprite = fullHeart;

                index += 20;
                healthObject.enabled = true;
            }
            else if (player.Health + 10 > index)
            {
                healthObject.sprite = halfHeart;
                index += 10;
                healthObject.enabled = true;
            }
            else
            {
                healthObject.enabled = false;
            }
        }
    }
}