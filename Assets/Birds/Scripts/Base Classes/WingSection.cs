using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingSection
{
    // These variables are defined relative to the previous section and transform

    public AirfoilData airfoil { get; private set; }
    public float chord { get; private set; }
    public float lengthLE { get; private set; } // Length of the line connecting this section's LE to the previous section's LE
    //public float boneLength { get; private set; } // Length of the line connecting this quarter-chord to the previous

    public float dihedralLocal;  // Vertical dihedral angle of this section relative to previous section
    public float sweepLocalLE; // Sweepback angle of the leading edge, relative to the previous section
    //public float sweepLocal; // Sweepback angle of the quarter-chord, relative to the previous section
    public float twistLocal; // Local twist angle around the quarter-chord, relative to the previous section
    public float twistControlOffset; // Twist offset (

    public int numPanels { get; private set; }


    public Vector3 quarterChordPosition { get; private set; } // Position of quarter-chord relative to the root transform
    public float twistAbsolute { get; private set; }


    public WingSection(AirfoilData airfoil, float chord, float lengthLE, float dihedral, float sweepLE, float twist, int numPanels) {
        this.airfoil = airfoil;
        this.chord = chord;
        //this.boneLength = boneLength;
        this.lengthLE = lengthLE;

        this.dihedralLocal = dihedral;
        this.sweepLocalLE = sweepLE;
        this.twistLocal = twist;

        this.numPanels = numPanels;
    }



    public void SetAngles(float dihedral, float sweepLE, float twist) {
        this.dihedralLocal = dihedral;
        this.sweepLocalLE = sweepLE;
        this.twistLocal = twist;
    }




    
    public void SetPositionAndTwist(Vector3 position, float twistAbsolute) {
        // Only use this function with WingPanelCreator
        this.quarterChordPosition = position;
        this.twistAbsolute = twistAbsolute;
    }
}
