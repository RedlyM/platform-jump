using UnityEngine;

namespace PlatformJump.Helpers
{
    public class PlatformHelper
    {
        private readonly BoxCollider2D _platform;

        public PlatformHelper(BoxCollider2D platform)
        {
            _platform = platform;
        }

        public Vector2 ComputeNearestPerimeterPoint(Vector2 worldPos)
        {
            var localPos = _platform.transform.InverseTransformPoint(worldPos);
            var halfSize = _platform.size * 0.5f;
            var localClosest = localPos;

            var absLocal = new Vector2(Mathf.Abs(localPos.x), Mathf.Abs(localPos.y));
            var dx = halfSize.x - absLocal.x;
            var dy = halfSize.y - absLocal.y;

            if (dx < dy)
            {
                localClosest.x = Mathf.Sign(localPos.x) * halfSize.x;
                localClosest.y = Mathf.Clamp(localPos.y, -halfSize.y, halfSize.y);
            }
            else
            {
                localClosest.y = Mathf.Sign(localPos.y) * halfSize.y;
                localClosest.x = Mathf.Clamp(localPos.x, -halfSize.x, halfSize.x);
            }

            return  _platform.transform.TransformPoint(localClosest);
        }
    }
}