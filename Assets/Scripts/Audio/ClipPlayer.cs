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
    }
}