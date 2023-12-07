using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingSection : MonoBehaviour
{
    // These variables are defined relative to the previous section and transform

    [Header("Section Properties")]
    public float chord;
    public float boneLength; // Length of the line connecting this quarter-chord to the previous
    [Space]
    public float dihedralLocal; // Vertical dihedral angle of this section relative to previous section
    public float sweepLocal; // Sweepback angle of the quarter-chord, relative to the previous section
    public float twistLocal; // Local twist angle around the quarter-chord, relative to the previous section

    public WingSection(float chord, float twistLocal, float boneLength, float sweepLocal, float dihedralLocal) {
        this.chord = chord;
        this.twistLocal = twistLocal;
        this.boneLength = boneLength;
        this.sweepLocal = sweepLocal;
        this.dihedralLocal = dihedralLocal;
    }


    [HideInInspector] public Vector3 quarterChordPosition; // Position of quarter-chord relative to the root transform
    [HideInInspector] public float twistAbsolute;
}
