using System.Collections.Generic;
using UnityEngine;

public class WingPanelCreator : MonoBehaviour
{

    public GameObject wingPrefab;
    public GameObject tailPrefab;
    public List<GameObject> quadObjects;




    public List<WingPanel> CreateWingPanels(List<WingSection> wingSections, Transform rootTransform, bool isLeft, float localAR) {
        // Returns a list of Wing Panels oriented and located along the wing

        int numSections = wingSections.Count;
        int sign = isLeft ? 1 : -1;

        //float xPrev = 0;
        //float yPrev = 0;
        //float zPrev = 0;

        //// Set initial angles for root section
        //float twistTotal = 0;
        //float sweepTotal = 0;
        //float dihedralTotal = 0;

        //// Store mirrored area of the wing (assume trapezoidal sections)
        //float totalArea = 0;


        //// Calculate and store relative position and orientation of each section relative to the root
        //for (int s = 0; s < numSections; s++) {
        //    WingSection currSection = wingSections[s];
            
        //    twistTotal += currSection.twistLocal + currSection.twistControlOffset;
        //    sweepTotal += currSection.sweepLocalLE;
        //    dihedralTotal += currSection.dihedralLocal;

        //    //float boneLength = wingSections[s].boneLength;
        //    float lengthLE = wingSections[s].lengthLE;
        //    float spanwiseLength = boneLength * Mathf.Cos(sweepTotal * Mathf.Deg2Rad);

        //    // Calculate mirrored area of the trapezoid between this section and next section
        //    if (s != numSections-1) totalArea += spanwiseLength * (currSection.chord + wingSections[s+1].chord);

        //    //float x = xPrev + horizontalLength * Mathf.Sin(sweepTotal * Mathf.Deg2Rad);
        //    //float y = yPrev + currSection.boneLength * Mathf.Sin(dihedralTotal * Mathf.Deg2Rad);
        //    //float z = zPrev + horizontalLength * Mathf.Cos(sweepTotal * Mathf.Deg2Rad) * sign;

        //    float z = zPrev + boneLength * Mathf.Sin(-sweepTotal * Mathf.Deg2Rad);
        //    float y = yPrev + boneLength * Mathf.Sin(dihedralTotal * Mathf.Deg2Rad);
        //    float x = xPrev + spanwiseLength * Mathf.Cos(dihedralTotal * Mathf.Deg2Rad) * -sign;

        //    wingSections[s].SetPositionAndTwist(new Vector3(x,y,z), twistTotal);

        //    xPrev = x;
        //    yPrev = y;
        //    zPrev = z;
        //}


        // Calculate local aspect ratio of this wing
        // for now, constant across this half
        //float totalSpan = xPrev * 2;
        //float localAR = totalSpan * totalSpan / totalArea;
        


        // Interpolate between sections to create panels
        List<WingPanel> wingPanels = new List<WingPanel>();   
        for (int s = 0; s < numSections-1; s++) {

            WingSection inSection = wingSections[s];
            WingSection outSection = wingSections[s+1];

            int numPanels = outSection.numPanels; //numPanelsPerSection[s];

            Vector3 quarterChordAxis = outSection.quarterChordPosition - inSection.quarterChordPosition;
            Vector3 perpendicularAxis = new Vector3(quarterChordAxis.x, quarterChordAxis.y, 0);

            float panelWidth = (float) perpendicularAxis.magnitude / numPanels;


            // Create p number of panels interpolating between the two sections
            for (int p = 0; p < numPanels; p++) {

                float positionGradient = (float) (p+0.5f) / numPanels;
                float chordGradient = (float) p / Mathf.Max(numPanels - 1, 1);

                Vector3 panelPosition = Vector3.Lerp(inSection.quarterChordPosition, outSection.quarterChordPosition, positionGradient);
                float panelChord = Mathf.Lerp(inSection.chord, outSection.chord, chordGradient);
                float panelTwist = Mathf.Lerp(inSection.twistAbsolute, outSection.twistAbsolute, chordGradient);
                float panelArea = panelChord * panelWidth;
                

                Vector3 initialForward = new Vector3(0, 0, 1);
                Vector3 initialUp = Vector3.Cross(initialForward, quarterChordAxis).normalized * -sign;

                // Twist around axis perpendicular to body, NOT the quarter chord line (don't skew panels along diagonal)
                Quaternion twistRotation = Quaternion.AngleAxis(panelTwist * sign, perpendicularAxis);

                Vector3 newForward = twistRotation * initialForward;
                Vector3 newUp = twistRotation * initialUp;

                WingPanel wingPanel = new WingPanel(inSection.airfoil, panelChord, panelArea, localAR, panelPosition, newForward, newUp);
                wingPanels.Add(wingPanel);

                CreateWingQuad(wingPanel, rootTransform);
            }
        }

        return wingPanels;
    }


    public void CreateWingQuad(WingPanel panel, Transform rootTransform) {
        float chord = panel.chord;
        float width = panel.area / chord;
        Quaternion panelRotation = Quaternion.LookRotation(panel.forward, panel.up);

        GameObject quad = Instantiate(wingPrefab, rootTransform);
        quad.transform.SetLocalPositionAndRotation(panel.positionRoot, panelRotation);
        quad.transform.localScale = new Vector3(width, 1, chord);

        quadObjects.Add(quad);
    }

    public void CreateTailQuad(TailPanel panel, Transform rootTransform) {
        Quaternion panelRotation = Quaternion.LookRotation(panel.forward, panel.up);

        GameObject quad = Instantiate(tailPrefab, rootTransform);
        quad.transform.SetLocalPositionAndRotation(Vector3.zero, panelRotation);
        quad.transform.localScale = new Vector3(panel.span, 1, panel.tailData.mainChord);

        quadObjects.Add(quad);
    }



    public void DestroyDebugQuads() {
        foreach (GameObject quad in quadObjects){
            Destroy(quad);
        }
        quadObjects.Clear();
    }

}
