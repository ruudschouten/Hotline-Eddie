using System;
using UnityEngine;

namespace Core
{
    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;

        private void Update()
        {
            transform.position = target.position + offset;
        }
    }
}