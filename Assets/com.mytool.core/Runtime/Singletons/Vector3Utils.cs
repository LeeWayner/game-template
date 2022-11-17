using UnityEngine;
namespace MyTool.Core.Runtime.Utils
{
    public static class Vector3Utils
    {
        public static float FastDistance(ref Vector3 start, ref Vector3 end)
        {
            return Mathf.Sqrt((end.x - start.x) * (end.x - start.x) + (end.y - start.y) * (end.y - start.y) +
                              (end.z - start.z) * (end.z - start.z));
        }

        public static void FastLerp(ref Vector3 start, ref Vector3 end, float percent, ref Vector3 result)
        {
            result.x = start.x + percent * (end.x - start.x);
            result.y = start.y + percent * (end.y - start.y);
            result.z = start.z + percent * (end.z - start.z);
        }
    }

    public static class Vector2Utils
    {
        public static Vector2 Zero = Vector2.zero;
    }
}
