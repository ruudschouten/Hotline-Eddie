using TMPro;
using UnityEngine;

namespace UI
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        public void SetWaveNumber(int number)
        {
            messageText.text = $"Wave {number:00}";
        }

        public void SetText(string text)
        {
            messageText.text = text;
        }
    }
}