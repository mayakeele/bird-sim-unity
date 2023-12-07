using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsymmetricRelativeControl", menuName = "ScriptableObjects/Control Range/Asymmetric Relative")]
public class AsymmetricRelativeControl : ControlRange
{
    public float neutralAngle;
    public float forwardAngleRelative;
    public float reverseAngleRelative;

    public override void SetRange() {
        forward = neutralAngle + forwardAngleRelative;
        neutral = neutralAngle;
        reverse = neutralAngle + reverseAngleRelative;
    }
}
