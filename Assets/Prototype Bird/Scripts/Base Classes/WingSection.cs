using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingSection
{
    // These variables are defined relative to the previous section and transform

    public AirfoilData airfoil { get; private set; }
    public float chord { get; private set; }
    public float boneLength { get; private set; } // Length of the line connecting this quarter-chord to the previous

    public float dihedralLocal;  // Vertical dihedral angle of this section relative to previous section
    public float sweepLocal; // Sweepback angle of the quarter-chord, relative to the previous section
    public float twistLocal; // Local twist angle around the quarter-chord, relative to the previous section


    public Vector3 quarterChordPosition { get; private set; } // Position of quarter-chord relative to the root transform
    public float twistAbsolute { get; private set; }


    public WingSection(AirfoilData airfoil, float chord, float boneLength) {
        this.airfoil = airfoil;
        this.chord = chord;
        this.boneLength = boneLength;

        this.sweepLocal = 0;
        this.twistLocal = 0;
        this.dihedralLocal = 0;
    }



    public void SetAngles(float dihedral, float sweep, float twist) {
        this.dihedralLocal = dihedral;
        this.sweepLocal = sweep;
        this.twistLocal = twist;
    }




    
    public void SetPositionAndTwist(Vector3 quarterChordPosition, float twistAbsolute) {
        // Only use this function with WingPanelCreator
        this.quarterChordPosition = quarterChordPosition;
        this.twistAbsolute = twistAbsolute;
    }
}
