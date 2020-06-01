using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Trigger : MonoBehaviour
{
    [SerializeField] protected AudioSource triggerSource;
    [SerializeField] protected new Rigidbody2D rigidbody;
    [SerializeField] protected new BoxCollider2D collider;

    [SerializeField] protected UnityEvent playerEnteredEvent;

    public Rigidbody2D Rigidbody => rigidbody;

    public BoxCollider2D Collider => collider;

    protected bool PlayedAudio;

    public void DisableCollision()
    {
        collider.enabled = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        HitObject(other.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HitObject(other);
    }

    protected virtual void HitObject(Collider2D other)
    {
        if (PlayedAudio)
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