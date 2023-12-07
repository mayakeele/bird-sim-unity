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

    public WingPanel(AirfoilData airfoil, float chord, float area, Vector3 position, Vector3 forward, Vector3 up) {
        this.airfoil = airfoil;
        this.chord = chord;
        this.area = area;
        this.position = position;
        this.forward = forward.normalized;
        this.up = up.normalized;
        //this.quarterChordTransform = quarterChordTransform;
    }


}
