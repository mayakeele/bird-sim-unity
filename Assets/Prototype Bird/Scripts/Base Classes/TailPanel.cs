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


    public TailPanel(TailData tailData, Transform rootTransform) {
        this.tailData = tailData;
        this.rootTransform = rootTransform;
        SetSpread(0);
        SetAngles(0,0,0);
    }


    public void SetSpread(float spreadPercent) {
        this.spreadPercent = spreadPercent;
        spreadAngle = tailData.SpreadAngle(spreadPercent);
        span = tailData.Span(spreadPercent);

        UpdateCenterPosition();
    }

    public void SetAngles(float pitch, float yaw, float roll) {
        Quaternion pitchYawRotation = Quaternion.AngleAxis(pitch, Vector3.left) * Quaternion.AngleAxis(yaw, Vector3.up);
        forward = pitchYawRotation * Vector3.forward;
        //forward = rootTransform.forward;

        Quaternion rollRotation = Quaternion.AngleAxis(roll, forward);
        up = rollRotation * pitchYawRotation * Vector3.up;
        //up = rootTransform.up;

        left = Vector3.Cross(forward, up).normalized;

        UpdateCenterPosition();
    }

    private void UpdateCenterPosition() {
        float distanceAC = 0.667f * tailData.mainChord * Mathf.Cos(0.5f * spreadAngle * Mathf.Deg2Rad);
        ACPosition = -forward * distanceAC;
    }



    public Vector3[] CalculateAerodynamicLoads(Vector3 panelVelocityLocal, float density) {
        // Returns a float array with two values: [0] is the lift force, [1] is the drag force


        float alpha = Aerodynamics.Alpha(panelVelocityLocal, forward, up, out Vector3 planeVelocity);
        Debug.Log(alpha);
        float vSquared = planeVelocity.sqrMagnitude;
        float area = tailData.mainChord * spreadAngle * Mathf.Deg2Rad;

        float liftForce = tailData.LiftForce(alpha, vSquared, span, density);
        float dragForce = tailData.DragForce(alpha, vSquared, area, density, liftForce);

        Vector3 liftDirection = Vector3.Cross(left, panelVelocityLocal).normalized;
        Vector3 dragDirection = -panelVelocityLocal.normalized;

        return new Vector3[] { liftForce * liftDirection, dragForce * dragDirection};
    }


    
}
