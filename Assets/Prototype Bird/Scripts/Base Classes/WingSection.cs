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

    public int numPanels { get; private set; }


    public Vector3 position { get; private set; } // Position of quarter-chord relative to the root transform
    public float twistAbsolute { get; private set; }


    public WingSection(AirfoilData airfoil, float chord, float boneLength, float dihedral, float sweep, float twist, int numPanels) {
        this.airfoil = airfoil;
        this.chord = chord;
        this.boneLength = boneLength;

        this.dihedralLocal = dihedral;
        this.sweepLocal = sweep;
        this.twistLocal = twist;

        this.numPanels = numPanels;
    }



    public void SetAngles(float dihedral, float sweep, float twist) {
        this.dihedralLocal = dihedral;
        this.sweepLocal = sweep;
        this.twistLocal = twist;
    }




    
    public void SetPositionAndTwist(Vector3 position, float twistAbsolute) {
        // Only use this function with WingPanelCreator
        this.position = position;
        this.twistAbsolute = twistAbsolute;
    }
}
