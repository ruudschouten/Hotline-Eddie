using UnityEngine;

namespace Characters
{
    public class BodyPartHelper : MonoBehaviour
    {
        [SerializeField] private BodyPart[] parts;

        public void Teleport(Transform trans)
        {
            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                if (i == 0)
                {
                    part.transform.position = trans.position - trans.right * part.MinimumDistance;
                    continue;
                }

                var prevTrans = parts[i - 1].transform;
                part.transform.position = prevTrans.position - prevTrans.right * part.MinimumDistance;
            }
        }
    }
}