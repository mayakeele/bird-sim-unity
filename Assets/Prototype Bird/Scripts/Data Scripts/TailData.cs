using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TailData", menuName = "ScriptableObjects/TailData", order = 1)]
public class TailData : ScriptableObject
{
    public AnimationCurve liftCurve;
    public AnimationCurve dragCurve;
    public bool generateCurves;

    [Header("Geometric Properties")]
    public float mainChord;
    [Space]
    public float minSpreadAngle = 10f;
    public float maxSpreadAngle = 160f;

    [Space]
    [Header("Lift Properties")]
    public float liftEfficiency = 1;
    public float vortexLiftSlopeMultiplier = 1.5f;
    [Space]
    public float alphaVortex = 20f;
    public float alphaStall = 40f;

    [Space]
    [Header("Drag Properties")]
    public float CDParasitic = 0.01f;
    public float CDMax = 2.0f;

    [Space]
    [Header("Pitching Moment Properties")]
    public float pitchingMoment = -0.01f;


    public AnimationCurve GenerateLiftCurve() {

        AnimationCurve curve = new AnimationCurve();

        float slopeAttached = 0.0274f * liftEfficiency;
        float slopeVortex = slopeAttached * vortexLiftSlopeMultiplier;

        // Zero lift alpha
        Keyframe key0 = new Keyframe(0, 0, slopeAttached, slopeAttached);
        curve.AddKey(key0);


        // Flow separation (end of attached section)
        float separationLift = slopeAttached * (alphaVortex);

        Keyframe key1a = new Keyframe(alphaVortex, separationLift, slopeAttached, slopeVortex);
        curve.AddKey(key1a);

        Keyframe key1b = new Keyframe(-alphaVortex, -separationLift, slopeVortex, slopeAttached);
        curve.AddKey(key1b);


        // Stall (max lift)
        float CLStall = (slopeAttached * alphaVortex) + (slopeVortex * (alphaStall - alphaVortex));

        Keyframe key2a = new Keyframe(alphaStall, CLStall, slopeVortex, 0);
        curve.AddKey(key2a);

        Keyframe key2b = new Keyframe(-alphaStall, -CLStall, 0, slopeVortex);
        curve.AddKey(key2b);


        // 90 degrees zero lift
        //float slope90degrees = (-CLPostStallPeak) / (90f - alphaPostStallPeak);

        Keyframe key5a = new Keyframe(90, 0, 0, 0);
        curve.AddKey(key5a);

        Keyframe key5b = new Keyframe(-90, 0, 0, 0);
        curve.AddKey(key5b);


        return curve;
    }

    private void OnValidate() {
        if (generateCurves) {
            Debug.Log("Generated lift and drag curves for airfoil");

            liftCurve = GenerateLiftCurve();
            //dragCurve = GenerateDragCurve();
            generateCurves = false;
        }

    }




    public float SpreadAngle(float spreadPercent) {
        return Mathf.Lerp(minSpreadAngle, maxSpreadAngle, spreadPercent);
    }

    public float Span(float spreadPercent) {
        float angle = SpreadAngle(spreadPercent);

        return 2 * Mathf.Sin(angle * 0.5f * Mathf.Deg2Rad);
    }

    public float CPDistance(float spreadPercent) {
        float angle = SpreadAngle(spreadPercent);

        return 0.6667f * Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
    }


    /*public Vector3[] CalculateLiftDragPitch(Vector3 panelVelocityLocal, float density) {
        // Returns a float array with three values: [0] is the lift force, [1] is the drag force and [2] is the pitching moment


        float alpha = Aerodynamics.Alpha(panelVelocityLocal, forward, up, out Vector3 planeVelocity);
        //Debug.Log("Alpha: " + alpha);

        float CL = airfoil.GetLiftCoefficient(alpha);
        float CD = airfoil.GetDragCoefficient(alpha);
        float CM = airfoil.pitchingMoment;

        float vSquared = planeVelocity.sqrMagnitude;

        float liftForce = Aerodynamics.LiftForce(CL, vSquared, area, density);
        float dragForce = Aerodynamics.DragForce(CD, vSquared, area, density);
        float pitchingMoment = Aerodynamics.PitchingMoment(CM, vSquared, area, chord, density);

        Vector3 liftDirection = Vector3.Cross(left, panelVelocityLocal).normalized;
        Vector3 dragDirection = -panelVelocityLocal.normalized;

        return new Vector3[] { liftForce * liftDirection, dragForce * dragDirection, pitchingMoment * left };
    }*/


    public float LiftForce(float alphaDeg, float vSquared, float span, float density) {
        float CL = liftCurve.Evaluate(alphaDeg);
        return 0.5f * density * vSquared * CL * span*span;
    }

    public float DragForce(float alphaDeg, float vSquared, float tailArea, float density, float liftForce) {
        float alphaAbs = Mathf.Abs(alphaDeg);

        float parasiticDrag = 0.5f * density * vSquared * tailArea * CDParasitic;

        if (alphaAbs <= alphaStall) {
            float inducedDrag = 0.5f * liftForce * alphaDeg * Mathf.Deg2Rad;
            return inducedDrag + parasiticDrag;
        }
        else if (alphaAbs <= 90) {
            float tangentClamped = Mathf.Abs(Mathf.Tan(alphaAbs * Mathf.Deg2Rad));
            float pressureDrag = liftForce * tangentClamped;
            return pressureDrag + parasiticDrag;
        }
        else {
            float tangentClamped = Mathf.Abs(Mathf.Tan(alphaAbs * Mathf.Deg2Rad));
            float pressureDrag = liftForce * tangentClamped;
            return pressureDrag + parasiticDrag;
        }
    }
}
