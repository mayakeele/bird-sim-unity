using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingPanel
{
    public float chord;
    public float area;

    public Transform quarterChordTransform; // Rotation faces

    public WingPanel(float chord, float area, Transform quarterChordTransform) {
        this.chord = chord;
        this.area = area;
        this.quarterChordTransform = quarterChordTransform;
    }
}
