using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [System.Serializable]
    public struct Range
    {
        public float low;
        public float high;
        public float power;

        public Range(float low, float high, float power = 1) {
            this.low = low;
            this.high = high;
            this.power = power;
        }

        public float Evaluate(float gradient) {
            float modifiedGradient = Mathf.Pow(gradient, power);
            return Mathf.Lerp(low, high, modifiedGradient);

        }
    }



    [Header("Speed-Varying Properties")]
    public float speedLow;
    public float speedHigh;
    [Space]
    public Range followStrengthRange;
    public Range dampingRange;
    [Space]
    public Range FOVRange;
    public Range orbitDistanceRange;
    [Space]

    [Header("References")]
    public Camera camera;
    Transform cameraTransform;
    // Target is the object the camera wants to follow
    public Transform targetTransform;
    public Rigidbody targetRigidbody;



    // Tracker is the current location of the camera focus, tries to catch up to the target
    private Vector3 trackerPosition;
    private Vector3 trackerVelocity;



    void Start() {
        cameraTransform = camera.transform;

        trackerPosition = targetTransform.position;
        trackerVelocity = targetRigidbody.velocity;
    }


    void LateUpdate()
    {
        // Use bird's speed and state to modify camera parameters
        float speedGradient = Mathf.InverseLerp(speedLow, speedHigh, targetRigidbody.velocity.magnitude);

        float followStrength = followStrengthRange.Evaluate(speedGradient);
        float damping = dampingRange.Evaluate(speedGradient);
        float orbitDistance = orbitDistanceRange.Evaluate(speedGradient);
        float FOV = FOVRange.Evaluate(speedGradient);

        // Update tracker
        Vector3 targetPosition = targetTransform.position;
        Vector3 targetVelocity = targetRigidbody.velocity;
        Vector3 displacement = trackerPosition - targetPosition;
        Vector3 relativeVelocity = trackerVelocity - targetVelocity;
        UpdateTrackerPosition(displacement, relativeVelocity, followStrength, damping);

        // Camera orbit around tracker
        Vector3 lookDirection = targetVelocity.normalized;
        Vector3 cameraUp = Vector3.up;
        Vector3 cameraDisplacement = -lookDirection * orbitDistance;
        cameraTransform.position = trackerPosition + cameraDisplacement;
        cameraTransform.LookAt(trackerPosition, cameraUp);

        // Camera effects
        camera.fieldOfView = FOV;
    }



    public void GetBirdForces() {
        // call this in BirdController, pass force and bird state values to this script
        // change camera behavior based on bird state
    }



    private void UpdateTrackerPosition(Vector3 displacement, Vector3 relativeVelocity, float currFollowStrength, float currDamping) {

        Vector3 acceleration = DampedSpring.GetDampedSpringAcceleration(displacement, relativeVelocity, currFollowStrength, currDamping);

        Vector3 deltaVelocity = acceleration * Time.deltaTime;
        trackerVelocity += deltaVelocity;

        Vector3 deltaPosition = trackerVelocity * Time.deltaTime;
        if (deltaPosition.magnitude > displacement.magnitude) {
            deltaPosition *= (displacement.magnitude / deltaPosition.magnitude);
        }
        trackerPosition += deltaPosition;
    }

}
