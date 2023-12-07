using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingSection : MonoBehaviour
{
    public float chord;
    public float twistLocal; // Local twist angle around the quarter-chord, relative to the previous section

    // These variables are defined relative to the previous section.
    // Root section will ignore the following by definition.
    public float boneLength; // Length of the line connecting this quarter-chord to the previous
    public float sweepLocal; // Sweepback angle of the quarter-chord, relative to the previous section
    public float dihedralLocal; // Vertical dihedral angle of this section relative to previous section

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
