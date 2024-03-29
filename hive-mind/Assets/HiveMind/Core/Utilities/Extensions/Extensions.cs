using UnityEngine;

namespace HiveMind.Core.Utilities.Extensions
{
    public static class Extensions
    {
        public static Transform SetAxes(this Transform transform, float? x = null, float? y = null, float? z = null, bool local = false)
        {
            Vector3 pos = local ? transform.localPosition : transform.position;

            if (x != null)
                pos.x = x.Value;
            if (y != null)
                pos.y = y.Value;
            if (z != null)
                pos.z = z.Value;

            if (local)
                transform.localPosition = pos;
            else
                transform.position = pos;

            return transform;
        }

        public static void LookAtWithAxis(this Transform transform, Transform target, Vector3 axis, float angleOffset = 0f)
        {
            transform.LookAt(target);
            transform.Rotate(axis, angleOffset, Space.Self);
        }

        public static void LookAtGradually(this Transform transform, Transform target, Vector3 axis, float maxRadiansDelta, bool stableUpVector = false)
        {
            Vector3 dir = target.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, maxRadiansDelta, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);
            if (stableUpVector)
                transform.rotation = Quaternion.Euler(axis.normalized * transform.rotation.eulerAngles.magnitude);
        }

        public static Transform FindRecursive(this Transform transform, string name, bool includeInactive = false)
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (child.name.Equals(name))
                    return child;
            }
            return null;
        }

        public static Vector3 InputToIso(this Vector3 input, Matrix4x4 matrix4X4) => matrix4X4.MultiplyPoint3x4(input); 
    }
}