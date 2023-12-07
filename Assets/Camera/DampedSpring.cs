using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DampedSpring
{

    public static Vector3 GetDampedSpringAcceleration(Vector3 trackerDisplacement, Vector3 trackerRelativeVelocity, float naturalFrequency, float dampingRatio){
        // Calculates the combined force of a spring and damping force on a tracker, with tracker velocity being relative to the target's velocity

        Vector3 acceleration = (-2 * dampingRatio * naturalFrequency * trackerRelativeVelocity) - (Mathf.Pow(naturalFrequency, 2) * trackerDisplacement);

        return acceleration;
    }
}
