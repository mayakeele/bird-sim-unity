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
        public float boneLength; // Length of the line connecting this quarter-chord to the next section
    }

    public SectionData[] wingSectionData;




    public List<WingSection> CreateWingSections() {

        int numSections = wingSectionData.Length;
        List<WingSection> wingSections = new List<WingSection>();

        for(int i=0; i<numSections; i++) {
            SectionData section = wingSectionData[i];
            wingSections.Add(new WingSection(section.airfoil, section.chord, section.boneLength));
        }

        return wingSections;
    }

}
