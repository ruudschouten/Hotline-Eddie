using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Trigger : MonoBehaviour
{
    [SerializeField] private AudioSource triggerSource;
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private new BoxCollider2D collider;

    [SerializeField] private UnityEvent playerEnteredEvent;

    public Rigidbody2D Rigidbody => rigidbody;

    public BoxCollider2D Collider => collider;

    private bool _playedAudio;

    public void DisableCollision()
    {
        collider.enabled = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        HitObject(other.collider);
    }

    private void HitObject(Collider2D other)
    {
        if (_playedAudio)
        {
            return;
        }
        
        Debug.Log($"Hit {other.name}");
        
        if (other.CompareTag("Player"))
        {
            _playedAudio = true;
            triggerSource.Play();
            
            playerEnteredEvent.Invoke();

            StartCoroutine(SlowDisable());
        }
    }

    private IEnumerator SlowDisable()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}