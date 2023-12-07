using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Aerodynamics
{
    const float deg2Rad = Mathf.Deg2Rad;
    const float rad2Deg = Mathf.Rad2Deg;


    public static float CalculateLift(float CL, float velocitySquared, float area, float density) {
        return 0.5f * density * velocitySquared * area * CL;
    }




    public static float CalculateAlpha(Vector3 localVelocity) {
        float alpha = Mathf.Atan(-localVelocity.y / localVelocity.z);
        return alpha * rad2Deg;
    }
    public static float CalculateBeta(Vector3 localVelocity) {
        float beta = Mathf.Asin(-localVelocity.x / localVelocity.magnitude);
        return beta * rad2Deg;
    }

    




    /*
    Vector3 CalcLiftDirection(Vector3 velocityDirection) {
        // Returns a direction vector perpendicular to velocity, in the same plane as the up vector
        Vector3 liftDir = Vector3.ProjectOnPlane(transform.up, velocityDirection).normalized;
        return liftDir;
    }*/
}
