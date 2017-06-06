using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCheckUtil
{
    public static  bool CheckTwoCircleIntersection(Vector3 center1, float radius1, Vector3 center2, float radius2)
    {
        return ( center1 - center2 ).XZSqrMagnitude() < ( radius1 + radius2 ) * ( radius1 + radius2 );
    }

    //startPos 为矩形宽的中点
    public static bool IsPointInRect(Vector3 startPos, Vector3 startDir, Vector3 startRightDir, Vector3 targetPos, float width, float height)
    {
        if (startDir.sqrMagnitude != 1)
            startDir = startDir.normalized;
        var dirToTarget = targetPos - startPos;
        var vForward = dirToTarget.XZDot(startDir);
        if (vForward < 0)
            return false;

        if (startRightDir.sqrMagnitude != 1)
            startRightDir = startRightDir.normalized;

        var vRight = dirToTarget.XZDot(startRightDir);
        if (vRight < 0)
        {
            return false;
        }
        return vForward <= height && vRight <= width * 0.5;
    }

    static float Sign(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return ( p1.x - p3.x ) * ( p2.z - p3.z ) - ( p2.x - p3.x ) * ( p1.z - p3.z );
    }

    //https://stackoverflow.com/questions/2049582/how-to-determine-if-a-point-is-in-a-2d-triangle
    public static bool IsPointInTriangle(Vector3 p, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        bool b1, b2, b3;
        b1 = Sign(p, v1, v2) < 0.0f;
        b2 = Sign(p, v2, v3) < 0.0f;
        b3 = Sign(p, v3, v1) < 0.0f;
        return ( b1 == b2 ) && ( b2 == b3 );
    }

    //多边形也可以使用
    //http://www.cnblogs.com/graphics/archive/2010/08/05/1793393.html
    static bool IsSameSide(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    {
        Vector3 AB = B - A;
        Vector3 AC = C - A;
        Vector3 AP = P - A;
        Vector3 v1 = Vector3.Cross(AB, AC);
        Vector3 v2 = Vector3.Cross(AB, AP);
        return Vector3.Dot(v1, v2) > 0;
    }

    public static bool IsPointInTriangle2(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    {
        return IsSameSide(A, B, C, P) &&
        IsSameSide(B, C, A, P) &&
        IsSameSide(C, A, B, P);
    }

}
