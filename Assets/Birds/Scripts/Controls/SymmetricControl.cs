using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SymmetricControl", menuName = "ScriptableObjects/Control Range/Symmetric")]
public class SymmetricControl : ControlRange
{
    public float maxDeflection;

    public override void SetRange() {
        forward = maxDeflection;
        neutral = 0;
        reverse = -maxDeflection;
    }
}
