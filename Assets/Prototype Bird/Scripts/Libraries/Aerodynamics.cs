using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Aerodynamics
{
    const float deg2Rad = Mathf.Deg2Rad;
    const float rad2Deg = Mathf.Rad2Deg;

    const float maxSpeedSquared = 300*300; // 300 m/s should be well below the sound barrier, or even lower for stability


    public static float LiftForce(float CL, float velocitySquared, float area, float density) {
        velocitySquared = Mathf.Min(velocitySquared, maxSpeedSquared);
        return 0.5f * density * velocitySquared * area * CL;
    }
    public static float DragForce(float CD, float velocitySquared, float area, float density) {
        velocitySquared = Mathf.Min(velocitySquared, maxSpeedSquared);
        return 0.5f * density * velocitySquared * area * CD;
    }

    public static float PitchingMoment(float CM, float velocitySquared, float area, float chord, float density) {
        velocitySquared = Mathf.Min(velocitySquared, maxSpeedSquared); 
        return 0.5f * density * velocitySquared * area * chord * CM;
    }



    public static float Alpha(Vector3 localVelocity) {
        float alpha = Mathf.Atan(-localVelocity.y / localVelocity.z);
        return alpha * rad2Deg;
    }
    public static float Alpha(Vector3 velocity, Vector3 forward, Vector3 up, out Vector3 planeVelocity) {
        Vector3 planeNormal = Vector3.Cross(forward, up);
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(velocity, planeNormal);
        planeVelocity = projectedVelocity;

        float alpha = Vector3.Angle(projectedVelocity, forward);
        float sign = -Mathf.Sign(Vector3.Dot(projectedVelocity, up));

        return alpha * sign;
    }


    public static float Beta(Vector3 localVelocity) {
        float beta = Mathf.Asin(-localVelocity.x / localVelocity.magnitude);
        return beta * rad2Deg;
    }
    public static float Beta(Vector3 velocity, Vector3 forward, Vector3 left, out Vector3 planeVelocity) {
        Vector3 planeNormal = Vector3.Cross(left, forward);
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(velocity, planeNormal);
        planeVelocity = projectedVelocity;

        float beta = Vector3.Angle(projectedVelocity, forward);
        float sign = Mathf.Sign(Vector3.Dot(projectedVelocity, left));

        return beta * sign;
    }





    /*
    Vector3 CalcLiftDirection(Vector3 velocityDirection) {
        // Returns a direction vector perpendicular to velocity, in the same plane as the up vector
        Vector3 liftDir = Vector3.ProjectOnPlane(transform.up, velocityDirection).normalized;
        return liftDir;
    }*/
}
