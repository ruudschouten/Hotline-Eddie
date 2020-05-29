using Helpers;
using UnityEngine;

namespace Characters
{
    public class Enemy : Character
    {
        [SerializeField] private Texture2D deadTexture;
        [SerializeField] private Transform target;
        [SerializeField] private bool shouldMove;
        [SerializeField] private float movementSpeed;

        private Vector3 _targetPosition;

        public bool ShouldMove
        {
            get => shouldMove;
            set => shouldMove = value;
        }

        private void Update()
        {
            if (IsDead)
            {
                return;
            }
            
            LookAt();
            
            if (!shouldMove)
            {
                return;
            }

            transform.position -= transform.right * (movementSpeed * Time.deltaTime);
        }

        protected void LookAt()
        {
            var angle = LookAtHelper.GetAngleAtTarget(target.position, transform.localPosition) - 180;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}