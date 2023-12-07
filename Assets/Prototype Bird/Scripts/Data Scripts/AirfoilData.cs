using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "AirfoilData", menuName = "ScriptableObjects/AirfoilData", order = 1)]
public class AirfoilData : ScriptableObject
{
    public AnimationCurve liftCurve;
    public AnimationCurve dragCurve;
    public bool generateCurves;

    [Header("Lift Properties")]
    public float liftCurveSlopeLinear = 0.1097f;
    public float alphaZeroLift = 0f;
    public float alphaNonlinear = 14f;
    [Space]
    public float alphaStall = 18f;
    public float CLStall = 1.7f;
    [Space]
    public float alphaPostStallPeak = 45f;
    public float CLPostStallPeak = 1.1f;

    [Space]
    [Header("Drag Properties")]
    public float parasiticDrag = 0.01f;
    [Space]
    public float wingAspectRatio = 6f;
    public float spanwiseEfficiencyFactor = 1;
    [Space]
    public float CDMax = 2.0f;

    [Space]
    [Header("Pitching Moment Properties")]
    public float pitchingMoment = -0.01f;




    public AnimationCurve GenerateLiftCurve() {
        
        AnimationCurve curve = new AnimationCurve();

        // POSITIVE ALPHA SIDE


        // Zero lift alpha
        Keyframe key0 = new Keyframe(alphaZeroLift, 0, liftCurveSlopeLinear, liftCurveSlopeLinear);
        curve.AddKey(key0);


        // Yield (end of linear section)
        float yieldLift = liftCurveSlopeLinear * (alphaNonlinear - alphaZeroLift);

        Keyframe key1a = new Keyframe(alphaNonlinear, yieldLift, liftCurveSlopeLinear, liftCurveSlopeLinear);
        curve.AddKey(key1a);

        Keyframe key1b = new Keyframe(alphaZeroLift-alphaNonlinear, -yieldLift, liftCurveSlopeLinear, liftCurveSlopeLinear);
        curve.AddKey(key1b);


        // Stall (max lift)
        Keyframe key2a = new Keyframe(alphaStall, CLStall, 0, 0);
        curve.AddKey(key2a);

        Keyframe key2b = new Keyframe(alphaZeroLift-alphaStall, -CLStall, 0, 0);
        curve.AddKey(key2b);


        // Post-stall drop
        float dropAlpha = (alphaStall + alphaPostStallPeak) / 2;
        float dropCL = CLPostStallPeak * 0.9f;

        Keyframe key3a = new Keyframe(dropAlpha, dropCL);
        curve.AddKey(key3a);

        Keyframe key3b = new Keyframe(alphaZeroLift-dropAlpha, -dropCL);
        curve.AddKey(key3b);


        // Post-stall peak
        Keyframe key4a = new Keyframe(alphaPostStallPeak, CLPostStallPeak, 0, 0);
        curve.AddKey(key4a);

        Keyframe key4b = new Keyframe(alphaZeroLift-alphaPostStallPeak, -CLPostStallPeak, 0, 0);
        curve.AddKey(key4b);


        // 90 degrees zero lift
        //float slope90degrees = (-CLPostStallPeak) / (90f - alphaPostStallPeak);

        Keyframe key5a = new Keyframe(90, 0, 0, 0);
        curve.AddKey(key5a);

        Keyframe key5b = new Keyframe(-90, 0, 0, 0);
        curve.AddKey(key5b);


        return curve;
    }


    public AnimationCurve GenerateDragCurve() {

        AnimationCurve curve = new AnimationCurve();

        // POSITIVE ALPHA SIDE

        // Zero lift alpha, only parasitic drag
        Keyframe key0 = new Keyframe(alphaZeroLift, parasiticDrag, 0, 0);
        curve.AddKey(key0);

        // Quadratically (concave up) increase drag while lift is linear
        float maxLiftLinear = liftCurveSlopeLinear * (alphaStall - alphaZeroLift);
        float scaleFactor = 1 / (Mathf.PI * wingAspectRatio * spanwiseEfficiencyFactor);
        float dragPreStall = (maxLiftLinear * maxLiftLinear) * scaleFactor + parasiticDrag;

        float leftSlope = 4 * scaleFactor * liftCurveSlopeLinear;
        float rightSlope = 2 * (CDMax - dragPreStall) / (90 - alphaStall);


        Keyframe key1a = new Keyframe(alphaStall, dragPreStall, leftSlope, rightSlope);
        curve.AddKey(key1a);

        Keyframe key1b = new Keyframe(alphaZeroLift-alphaStall, dragPreStall, -leftSlope, -rightSlope);
        curve.AddKey(key1b);


        // Quadratically (concave down) increase drag in post-stall region
        Keyframe key2a = new Keyframe(90, CDMax, 0, 0);
        curve.AddKey(key2a);

        Keyframe key2b = new Keyframe(-90, CDMax, 0, 0);
        curve.AddKey(key2b);


        return curve;
    }


    private void OnValidate() {
        if (generateCurves) {
            Debug.Log("Generated lift and drag curves for airfoil");

            liftCurve = GenerateLiftCurve();
            dragCurve = GenerateDragCurve();
            generateCurves = false;
        }

    }


    public float GetLiftCoefficient(float alphaDeg) {
        return liftCurve.Evaluate(alphaDeg);
    }

    public float GetDragCoefficient(float alphaDeg) {
        return dragCurve.Evaluate(alphaDeg);
    }

}
