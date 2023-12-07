using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingPanel
{
    public AirfoilData airfoil;
    public float chord;
    public float area;

    //public Transform quarterChordTransform;
    public Vector3 position;
    public Vector3 forward;
    public Vector3 up;
    public Vector3 left;

    public WingPanel(AirfoilData airfoil, float chord, float area, Vector3 position, Vector3 forward, Vector3 up) {
        this.airfoil = airfoil;
        this.chord = chord;
        this.area = area;
        this.position = position;
        this.forward = forward.normalized;
        this.up = up.normalized;
        this.left = Vector3.Cross(forward, up).normalized;
        //this.quarterChordTransform = quarterChordTransform;
    }



    public Vector3[] CalculateLiftDragPitch(Vector3 cgVelocity, Vector3 bodyRotationRate, float density) {
        // Returns a float array with three values: [0] is the lift force, [1] is the drag force and [2] is the pitching moment

        Vector3 rotationVelocity = Vector3.Cross(bodyRotationRate, position);
        Vector3 totalVelocity = cgVelocity + rotationVelocity;

        float alpha = Aerodynamics.Alpha(totalVelocity, forward, up, out Vector3 planeVelocity);

        float CL = airfoil.GetLiftCoefficient(alpha);
        float CD = airfoil.GetDragCoefficient(alpha);

        float vSquared = planeVelocity.sqrMagnitude;

        float liftForce = Aerodynamics.LiftForce(CL, vSquared, area, density);
        float dragForce = Aerodynamics.DragForce(CD, vSquared, area, density);
        float pitchingMoment = 0;

        Vector3 liftDirection = Vector3.Cross(left, planeVelocity).normalized;
        Vector3 dragDirection = -planeVelocity.normalized;

        return new Vector3[] { liftForce * liftDirection, dragForce * dragDirection, pitchingMoment * left};
    }

}
