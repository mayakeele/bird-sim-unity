using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuaternionExtensions
{
    
    public static Quaternion Differerence(Quaternion startRotation, Quaternion endRotation){
        Quaternion difference = startRotation * Quaternion.Inverse(endRotation);
        return difference;
    }


    public static Vector3 AxisBetween(Quaternion startRotation, Quaternion endRotation, out float angleDegrees){
        Quaternion difference = Differerence(startRotation, endRotation);

        difference.ToAngleAxis(out float angle, out Vector3 axis);

        angleDegrees = angle;
        return axis;
    }
    public static Vector3 AxisBetween(Quaternion startRotation, Quaternion endRotation){
        Quaternion difference = Differerence(startRotation, endRotation);

        difference.ToAngleAxis(out float angle, out Vector3 axis);

        return axis;
    }
}
