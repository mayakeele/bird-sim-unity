using System.Collections.Generic;
using UnityEngine;

public class WingPanelCreator : MonoBehaviour
{

    public Transform wingRootR;
    public Transform wingRootL;

    public List<WingSection> wingSections;

    public int[] numPanelsPerSection = { 5, 5 };

    public GameObject quadPrefab;
    


    // Start is called before the first frame update
    void Start()
    {
        CreateWingPanels(true, wingRootL);
        CreateWingPanels(false, wingRootR);
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public List<WingPanel> CreateWingPanels(bool isLeft, Transform rootTransform) {
        // Returns a list of Wing Panels oriented and located along the wing 

        int numSections = wingSections.Count;

        WingSection rootSection = wingSections[0];

        float xPrev = 0;
        float yPrev = 0;
        float zPrev = 0;

        float twistTotal = rootSection.twistLocal;
        float sweepTotal = 0;
        float dihedralTotal = 0;

        rootSection.quarterChordPosition = Vector3.zero;
        rootSection.twistAbsolute = twistTotal;

        int sign = isLeft ? 1 : -1;

       
        // Calculate and store relative position and orientation of each section after the root
        for (int s = 1; s < numSections; s++) {
            WingSection currSection = wingSections[s];

            twistTotal += currSection.twistLocal;
            sweepTotal += currSection.sweepLocal;
            dihedralTotal += currSection.dihedralLocal;

            float horizontalLength = currSection.boneLength * Mathf.Cos(dihedralTotal * Mathf.Deg2Rad);

            //float x = xPrev + horizontalLength * Mathf.Sin(sweepTotal * Mathf.Deg2Rad);
            //float y = yPrev + currSection.boneLength * Mathf.Sin(dihedralTotal * Mathf.Deg2Rad);
            //float z = zPrev + horizontalLength * Mathf.Cos(sweepTotal * Mathf.Deg2Rad) * sign;

            float z = zPrev + horizontalLength * Mathf.Sin(-sweepTotal * Mathf.Deg2Rad);
            float y = yPrev + currSection.boneLength * Mathf.Sin(dihedralTotal * Mathf.Deg2Rad);
            float x = xPrev + horizontalLength * Mathf.Cos(sweepTotal * Mathf.Deg2Rad) * -sign;

            wingSections[s].quarterChordPosition = new Vector3(x,y,z);
            wingSections[s].twistAbsolute = twistTotal;

            xPrev = x;
            yPrev = y;
            zPrev = z;
        }


        // Interpolate between sections to create panels
        List<WingPanel> wingPanels = new List<WingPanel>();   
        for (int s = 0; s < numSections-1; s++) {

            WingSection inSection = wingSections[s];
            WingSection outSection = wingSections[s+1];
            
            int numPanels = numPanelsPerSection[s];

            Vector3 quarterChordAxis = outSection.quarterChordPosition - inSection.quarterChordPosition;
            Vector3 perpendicularAxis = new Vector3(quarterChordAxis.x, quarterChordAxis.y, 0);

            float panelWidth = perpendicularAxis.magnitude / numPanels;


            // Create p number of panels interpolating between the two sections
            for (int p = 0; p < numPanels; p++) {

                float positionGradient = (p+0.5f) / numPanels;
                float chordGradient = (float) p / (numPanels - 1);

                Vector3 panelPosition = Vector3.Lerp(inSection.quarterChordPosition, outSection.quarterChordPosition, positionGradient);
                float panelChord = Mathf.Lerp(inSection.chord, outSection.chord, chordGradient);
                float panelTwist = Mathf.Lerp(inSection.twistAbsolute, outSection.twistAbsolute, chordGradient);
                float panelArea = panelChord * panelWidth;

                Vector3 initialForward = new Vector3(0, 0, 1);
                Vector3 initialUp = Vector3.Cross(initialForward, quarterChordAxis).normalized * -sign;

                Quaternion twistRotation = Quaternion.AngleAxis(panelTwist * sign, perpendicularAxis);

                Vector3 newForward = twistRotation * initialForward;
                Vector3 newUp = twistRotation * initialUp;

                Quaternion panelRotation = Quaternion.LookRotation(newForward, newUp);

                Transform panelTransform = new GameObject().transform;
                panelTransform.SetParent(rootTransform);
                panelTransform.SetLocalPositionAndRotation(panelPosition, panelRotation);


                WingPanel wingPanel = new WingPanel(panelChord, panelArea, panelTransform);
                wingPanels.Add(wingPanel);

                CreateDebugQuad(wingPanel);
            }
        }

        return wingPanels;
    }


    private void CreateDebugQuad(WingPanel panel) {
        float chord = panel.chord;
        float width = panel.area / chord;

        GameObject quad = Instantiate(quadPrefab, panel.quarterChordTransform);
        quad.transform.localPosition = new Vector3(0,0,-0.25f * chord);
        quad.transform.localScale = new Vector3(width, 1, chord);
    }

}
