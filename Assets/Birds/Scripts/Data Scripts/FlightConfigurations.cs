using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlightConfigurations", menuName = "ScriptableObjects/FlightConfigurations", order = 1)]
[System.Serializable]
public class FlightConfigurations : ScriptableObject
{
    [Header("Misc Properties")]
    public float wingRootLateralOffset;

    [Header("Cruise (default) configuration")]
    public WingData cruiseWingData;
    public TailData cruiseTailData;

    [Header("Maneuver configuration")]
    public WingData maneuverWingData;
    public TailData maneuverTailData;
    public float maneuverWingInterpPower = 1;
    public float maneuverTailInterpPower = 1;

    [Header("Tucked configuration")]
    public WingData tuckedWingData;
    public TailData tuckedTailData;
    public float tuckedWingInterpPower = 1;
    public float tuckedTailInterpPower = 1;

    [Space]
    [Header("Mass Properties")]
    public float singleWingMass;
    [Space]
    public float bodyMass;
    public float bodyDiameter;
    [Tooltip("Distance from wing root to head")]
    public float bodyForeDist;
    [Tooltip("Distance from wing root to butt")]
    public float bodyAftDistance;
    [Space]
    [Tooltip("Mass of both legs")]
    public float legsMass;
    public float legsLength;
    [Tooltip("Distance from wing root to base of legs (backwards)")]
    public float legsAftDistance;
    


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


    public List<WingSection> InterpolateWingSections(float configurationInput, bool isLeft, out float localAR) {
        // Interpolate between given wing configurations to create wing sections

        float wingInterpPower = configurationInput >= 0 ? tuckedWingInterpPower : maneuverWingInterpPower;
        float absConfigInput = Mathf.Abs(configurationInput);
        float absConfigInputPower = Mathf.Pow(absConfigInput, wingInterpPower);

        WingData otherWingData = configurationInput >= 0 ? tuckedWingData : maneuverWingData;

        int numSections = cruiseWingData.wingSectionData.Length;
        List<WingSection> wingSections = new List<WingSection>(numSections);
        for (int i = 0; i < numSections; i++) {

            // Get section data for 'from' and 'to' sections to interpolate
            WingData.SectionData fromSection = cruiseWingData.wingSectionData[i];
            WingData.SectionData toSection = otherWingData.wingSectionData[i];


            // Interpolate chord and LE length
            float currChord = Mathf.Lerp(fromSection.chord, toSection.chord, absConfigInputPower);
            float currLengthLE = Mathf.Lerp(fromSection.lengthLE, toSection.lengthLE, absConfigInputPower);

            // Interpolate angles between given configurations
            float currDihedral = Mathf.Lerp(fromSection.dihedralLocal, toSection.dihedralLocal, absConfigInputPower);
            float currSweep = Mathf.Lerp(fromSection.sweepLocalLE, toSection.sweepLocalLE, absConfigInputPower);
            float currTwist = Mathf.Lerp(fromSection.twistLocal, toSection.twistLocal, absConfigInputPower);

            // FUTURE - INTERPOLATE AIRFOIL PROPERTIES SMOOTHLY
            // For now, switch airfoils when halfway between
            AirfoilData currAirfoil = absConfigInputPower >= 0.5f ? toSection.airfoil : fromSection.airfoil;

            // Use higher number of panels
            int currNumPanels = Mathf.Max(fromSection.numPanelsInward, toSection.numPanelsInward);

            wingSections.Add(
                new WingSection(currAirfoil, currChord, currLengthLE,
                currDihedral, currSweep, currTwist,
               currNumPanels));
        }

        // delete AR stuff later pls
        wingSections = PositionWingSections(wingSections, isLeft, out float AR);
        localAR = AR;

        return wingSections;
    }


    // delet ar
    public List<WingSection> PositionWingSections(List<WingSection> wingSections, bool isLeft, out float localAR) {

        int numSections = wingSections.Count;
        int sign = isLeft ? 1 : -1;

        // Store accumulated position at each section
        float x = wingRootLateralOffset;
        float y = 0;
        float z = 0;

        // Store accumulated angles across wing
        float twistTotal = 0;
        float sweepTotal = 0;
        float dihedralTotal = 0;

        // Store mirrored area of the wing (assume trapezoidal sections)
        float totalArea = 0;


        // Calculate position and orientation of each section relative to the root
        for (int s = 0; s < numSections; s++) {

            WingSection currSection = wingSections[s];

            // Accumulate angles from this section for later use
            twistTotal += currSection.twistLocal + currSection.twistControlOffset;
            sweepTotal += currSection.sweepLocalLE;
            dihedralTotal += currSection.dihedralLocal;

            currSection.SetPositionAndTwist(new Vector3(x, y, z - (0.25f * currSection.chord)), twistTotal);


            // Use accumulated angles to updatenext section position
            float lengthLE = currSection.lengthLE;
            float spanwiseLength = lengthLE * Mathf.Cos(sweepTotal * Mathf.Deg2Rad);

            z += lengthLE * Mathf.Sin(-sweepTotal * Mathf.Deg2Rad);
            y += lengthLE * Mathf.Sin(dihedralTotal * Mathf.Deg2Rad);
            x += spanwiseLength * Mathf.Cos(dihedralTotal * Mathf.Deg2Rad) * -sign;


            // Calculate mirrored area of the trapezoid between this section and next section
            if (s != numSections - 1) totalArea += spanwiseLength * (currSection.chord + wingSections[s + 1].chord);
        }


        // Calculate local aspect ratio of this wing
        // for now, constant across this half
        float totalSpan = x * 2;
        localAR = totalSpan * totalSpan / totalArea;

        // STORE THIS SOMEWHERE FOR LATER

        return wingSections;
    }
}
