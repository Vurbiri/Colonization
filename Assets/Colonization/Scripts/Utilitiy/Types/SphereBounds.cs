using UnityEngine;

namespace Vurbiri.Colonization
{
    public struct SphereBounds
    {
        private readonly Vector3 _center;
        private readonly float _sqrRadius;

        public SphereBounds(Vector3 center, float radius)
        {
            _center = center;
            _sqrRadius = radius * radius;
        }

        public SphereBounds(float radius) : this(Vector3.zero, radius) { }

        public readonly Vector3 ClosestPoint(Vector3 point)
        {
            float sqrRatio = _sqrRadius / (point - _center).sqrMagnitude;

            if (sqrRatio < 1.0f)
                point *= Mathf.Pow(sqrRatio, 0.5f);

            return point;
        }
    }
}
