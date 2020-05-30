using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StaminaUI : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Image staminaBar;
        
        public void UpdateStamina()
        {
            var percentage = player.Stamina / player.MaxStamina;

            staminaBar.fillAmount = percentage;
        }
    }
}