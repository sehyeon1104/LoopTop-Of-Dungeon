using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathClass
{
    public static Quaternion VectorToQuaternion(Vector2 vector ,Transform transform, float flus = 0)
    {
        float angle = (Mathf.Atan2(vector.y, vector.x) + flus)* Mathf.Rad2Deg;
        Quaternion quaternion = Quaternion.AngleAxis(angle - 90, transform.forward);
        return quaternion;
    }
}
