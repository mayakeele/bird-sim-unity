using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "WingData", menuName = "ScriptableObjects/WingData", order = 1)]
[System.Serializable]
public class WingData : ScriptableObject
{
    [System.Serializable]
    public struct SectionData {
        public AirfoilData airfoil;

        public float chord; // Chord of the airfoil at the root of this section
        public float lengthLE; // Length of the line connecting this section's LE to the next section's LE

        public float dihedralLocal;
        public float sweepLocalLE;
        public float twistLocal;

        public int numPanelsInward;
    }

    public SectionData[] wingSectionData;




    //public List<WingSection> CreateWingSections(float configurationInput) {

    //    int numSections = wingSectionData.Length;
    //    List<WingSection> wingSections = new List<WingSection>();

    //    for (int i = 0; i < numSections; i++) {
    //        SectionData section = wingSectionData[i];

    //        // Set angles based on given configuration
    //        SectionAngles lowAngles = section.neutralConfiguration;
    //        SectionAngles highAngles = configurationInput >= 0 ? section.fastConfiguration : section.maneuverConfiguration;
    //        float currDihedral = Mathf.Lerp(lowAngles.dihedralLocal, highAngles.dihedralLocal, Mathf.Abs(configurationInput));
    //        float currSweep = Mathf.Lerp(lowAngles.sweepLocal, highAngles.sweepLocal, Mathf.Abs(configurationInput));
    //        float currTwist = Mathf.Lerp(lowAngles.twistLocal, highAngles.twistLocal, Mathf.Abs(configurationInput));

    //        wingSections.Add(
    //            new WingSection(section.airfoil, section.chord, section.boneLength,
    //            currDihedral, currSweep, currTwist,
    //            section.numPanelsInward));
    //    }

    //    return wingSections;
    //}

}
