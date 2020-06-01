using Core;
using Helpers;
using UnityEngine;

namespace Characters
{
    public class BodyPart : MonoRenderer
    {
        [SerializeField] private Enemy character;
        [SerializeField] private float movementSpeed;
        [SerializeField] private Transform partToFollow;
        [SerializeField] private float minimumDistance;

        public float MinimumDistance => minimumDistance;

        public Character Character => character;

        private void Update()
        {
            if (character.IsDead)
            {
                return;
            }
            
            LookAt();
            var distance = Vector3.Distance(transform.position, partToFollow.position);
            if (distance > minimumDistance)
            {
                MoveTowards();   
            }
        }
        
        protected void LookAt()
        {
            var angle = LookAtHelper.GetAngleAtTarget(partToFollow.position, transform.position) - 180;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        protected void MoveTowards()
        {
            transform.position -= transform.right * (movementSpeed * Time.deltaTime);
        }
    }
}