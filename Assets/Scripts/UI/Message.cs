using System.Collections;
using UnityEngine;

namespace UI
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private float hideDelay = 5f;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private float fadeDuration;
        
        public void ShowAndHide()
        {
            StartCoroutine(ShowAndHideRoutine());
        }

        private IEnumerator ShowAndHideRoutine()
        {
            for (var time = 0.01f; time < fadeDuration; time+=Time.deltaTime)
            {
                group.alpha += time;
                yield return null;
            }
            yield return new WaitForSeconds(hideDelay);
            for (var time = 0.01f; time < fadeDuration; time+=Time.deltaTime)
            {
                group.alpha -= time;
                yield return null;
            }
            transform.gameObject.SetActive(false);
        }
    }
}