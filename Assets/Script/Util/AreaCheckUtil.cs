using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCheckUtil
{
    public bool CheckTwoCircleIntersection(Vector3 center1, float radius1, Vector3 center2, float radius2)
    {
        return ( center1 - center2 ).XZSqrMagnitude() < ( radius1 + radius2 ) * ( radius1 + radius2 );
    }

}
