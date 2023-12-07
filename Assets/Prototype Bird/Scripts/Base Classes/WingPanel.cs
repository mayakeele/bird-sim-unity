using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingPanel
{
    public AirfoilData airfoil;
    public float chord;
    public float area;
    public float localAR;

    public Vector3 positionRoot; // position of quarter chord relative to the wing root
    public Vector3 forward;
    public Vector3 up;
    public Vector3 left;

    public WingPanel(AirfoilData airfoil, float chord, float area, float localAR, Vector3 position, Vector3 forward, Vector3 up) {
        this.airfoil = airfoil;
        this.chord = chord;
        this.area = area;
        this.localAR = localAR;
        this.positionRoot = position;
        this.forward = forward.normalized;
        this.up = up.normalized;
        this.left = Vector3.Cross(forward, up).normalized;
        //this.quarterChordTransform = quarterChordTransform;
    }



    public Vector3[] CalculateAerodynamicLoads(Vector3 panelVelocityLocal, float density) {
        // Returns a float array with three values: [0] is the lift force, [1] is the drag force and [2] is the pitching moment


        float alpha = Aerodynamics.Alpha(panelVelocityLocal, forward, up, out Vector3 planeVelocity);
        //Debug.Log("Alpha: " + alpha);

        float CL = airfoil.GetLiftCoefficient(alpha);
        float CD = airfoil.GetDragCoefficient(alpha, CL, localAR);
        float CM = airfoil.pitchingMoment;

        float vSquared = planeVelocity.sqrMagnitude;

        float liftForce = Aerodynamics.LiftForce(CL, vSquared, area, density);
        float dragForce = Aerodynamics.DragForce(CD, vSquared, area, density);
        float pitchingMoment = Aerodynamics.PitchingMoment(CM, vSquared, area, chord, density);

        Vector3 liftDirection = Vector3.Cross(left, panelVelocityLocal).normalized;
        Vector3 dragDirection = -panelVelocityLocal.normalized;

        return new Vector3[] { liftForce * liftDirection, dragForce * dragDirection, pitchingMoment * left};
    }

}
