using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class MonoRenderer : MonoBehaviour
    {
        [SerializeField] protected new Rigidbody2D rigidbody;
        [SerializeField] protected new SpriteRenderer renderer;
        [SerializeField] protected new BoxCollider2D collider;

        public Rigidbody2D Rigidbody
        {
            get => rigidbody;
            set => rigidbody = value;
        }

        public BoxCollider2D Collider
        {
            get => collider;
            set => collider = value;
        }

        public SpriteRenderer Renderer
        {
            get => renderer;
            set => renderer = value;
        }
    }
}