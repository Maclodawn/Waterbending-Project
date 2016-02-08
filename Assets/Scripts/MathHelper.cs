using UnityEngine;
using System.Collections;

public static class MathHelper
{
    public static float angle(Vector2 _from, Vector2 _to)
    {
        float angle = Vector2.Angle(_from, _to);
        {
            Vector3 cross = Vector3.Cross(_from, _to);
            if (cross.z > 0)
                angle = -angle;
        }

        return angle;
    }
}
