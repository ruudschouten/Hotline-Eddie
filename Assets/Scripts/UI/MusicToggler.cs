using UnityEngine;
using UnityEngine.Audio;

namespace UI
{
    public class MusicToggler : MonoBehaviour
    {
        [SerializeField] private AudioMixerSnapshot muted;
        [SerializeField] private AudioMixerSnapshot unmuted;

        private bool notMuted = true;

        public void Toggle()
        {
            if (notMuted)
            {
                muted.TransitionTo(0f);
            }
            else
            {
                unmuted.TransitionTo(0f);
            }

            notMuted = !notMuted;
        }
    }
}