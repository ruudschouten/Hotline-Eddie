using UnityEngine;

namespace Helpers
{
    public class LookAtHelper
    {
        public static float GetAngleAtTarget(Vector3 targetPosition, Vector3 sourcePosition)
        {
            var radians = Mathf.Atan2(targetPosition.y - sourcePosition.y,
                targetPosition.x - sourcePosition.x);
            var degrees = (180 / Mathf.PI) * radians;

            return degrees;
        }
    }
}