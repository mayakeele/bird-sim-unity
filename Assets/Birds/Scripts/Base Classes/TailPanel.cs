using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailPanel
{
    public TailData tailData;
    public Transform rootTransform;

    public float spreadPercent;
    public float spreadAngle;
    public float span;

    public Vector3 ACPosition; // position of aerodynamic center relative to tail root
    public Vector3 areaCenter; // position of mean tail area relative to the wing root
    public Vector3 forward;
    public Vector3 up;
    public Vector3 left;

    public float currPitch;
    public float currRoll;


    public TailPanel(TailData tailData, Transform rootTransform) {
        this.tailData = tailData;
        this.rootTransform = rootTransform;
        SetSpread(0);
        SetAngles(0,0);
    }


    public void SetSpread(float spreadAngle) {
        span = tailData.Span(spreadAngle);

        UpdateCenterPosition();
    }



    public void SetAngles(float pitch, float roll) {
        Quaternion pitchRotation = Quaternion.AngleAxis(pitch, Vector3.left);
        forward = pitchRotation * Vector3.forward;
        //forward = rootTransform.forward;

        Quaternion rollRotation = Quaternion.AngleAxis(roll, forward);
        up = rollRotation * pitchRotation * Vector3.up;
        //up = rootTransform.up;

        left = Vector3.Cross(forward, up).normalized;

        UpdateCenterPosition();
    }

    //public void SetAngles(float pitch, float roll) {

    //    Quaternion rollRotation = Quaternion.AngleAxis(roll, Vector3.forward);
    //    left = rollRotation * Vector3.left;

    //    Quaternion pitchRotation = Quaternion.AngleAxis(pitch, left);
    //    forward = pitchRotation * Vector3.forward;
        
    //    up = -Vector3.Cross(forward, left).normalized;

    //    UpdateCenterPosition();
    //}

    private void UpdateCenterPosition() {
        float distanceAC = 0.667f * tailData.mainChord * Mathf.Cos(0.5f * spreadAngle * Mathf.Deg2Rad);
        ACPosition = -forward.normalized * distanceAC;
    }



    public Vector3[] CalculateAerodynamicLoads(Vector3 panelVelocityLocal, float density) {
        // Returns a float array with two values: [0] is the lift force, [1] is the drag force, [2] is the side slip force


        float alpha = Aerodynamics.Alpha(panelVelocityLocal, forward, up, out Vector3 velocityAlpha);
        float beta = Aerodynamics.Beta(panelVelocityLocal, forward, left, out Vector3 velocityBeta);
        float vSqrAlpha = velocityAlpha.sqrMagnitude;
        float vSqrBeta = velocityBeta.sqrMagnitude;
        float area = 0.5f * Mathf.Pow(tailData.mainChord, 2) * spreadAngle * Mathf.Deg2Rad;

        float liftForce = tailData.LiftForce(alpha, vSqrAlpha, span, density);
        float dragForce = tailData.DragForce(alpha, vSqrAlpha, area, density, liftForce);
        float sideslipForce = tailData.SideslipForce(beta, vSqrBeta, area, density);

        Vector3 liftDirection = Vector3.Cross(left, panelVelocityLocal).normalized;
        Vector3 dragDirection = -panelVelocityLocal.normalized;
        Vector3 sideslipDirection = Vector3.Cross(panelVelocityLocal, up).normalized;

        return new Vector3[] { liftForce * liftDirection, dragForce * dragDirection, sideslipForce * sideslipDirection};
    }


    
}
