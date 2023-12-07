using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlRange : ScriptableObject
{
    [HideInInspector] public float forward;
    [HideInInspector] public float neutral;
    [HideInInspector] public float reverse;

    public abstract void SetRange();

    public float GetAngle(float throwPercent) {
        SetRange();

        if(throwPercent >= 0) {
            return Mathf.Lerp(neutral, forward, throwPercent);
        }
        else {
            return Mathf.Lerp(neutral, reverse, -throwPercent);
        }
    }
}
