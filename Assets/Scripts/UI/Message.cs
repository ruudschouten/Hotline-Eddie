using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private float hideDelay = 5f;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private float fadeDuration;
        [SerializeField] private UnityEvent onFade;

        public CanvasGroup Group => @group;

        public UnityEvent OnFade => onFade;
        
        public void ShowAndHide()
        {
            StartCoroutine(ShowAndHideRoutine());
        }

        private IEnumerator ShowAndHideRoutine()
        {
            group.alpha = 1;
            yield return new WaitForSeconds(hideDelay);
            for (var time = 0.01f; time < fadeDuration; time+=Time.deltaTime)
            {
                group.alpha -= time;
                yield return null;
            }
            
            OnFade.Invoke();

            yield return new WaitForSeconds(0.5f);
            group.alpha = 0;
        }
    }
}