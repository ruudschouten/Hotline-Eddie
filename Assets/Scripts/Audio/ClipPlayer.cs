using System.Collections;
using UnityEngine;

namespace Audio
{
    public class ClipPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource source;

        public void PlayClip(AudioClip clip)
        {
            source.PlayOneShot(clip);
        }

        public void PlayAfterHalfASecond(AudioClip clip)
        {
            PlayDelayed(clip, 0.5f);
        }
        
        public void PlayDelayed(AudioClip clip, float seconds)
        {
            StartCoroutine(PlayRoutine(clip, seconds));
        }

        private IEnumerator PlayRoutine(AudioClip clip, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            source.PlayOneShot(clip);
        }
    }
}