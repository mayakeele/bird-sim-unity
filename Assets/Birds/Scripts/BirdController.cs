using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{

    //Debug Variables
    public bool inWindTunnel;
    public Vector3 testVelocity;
    public Vector3 testAngularVelocity;
    [Space]


    // Control Ranges
    public ControlRange tailPitchRange;
    public ControlRange tailRollRange;
    public ControlRange wingtipTwistRange;
    public int wingtipSectionIndex;
    [Space]

    // Control Change Rates (degrees/second)
    public float tailPitchRate;
    public float tailRollRate;
    public float wingtipTwistRate;

    // Wing & Tail Data
    public FlightConfigurations flightConfigurations;
    //public WingData wingData;
    //public TailData tailData;
    [Space]

    // Geometry
    public Rigidbody rb;
    public Transform cg;
    [Space]
    public Transform wingRoot;
    public Transform tailRoot;
    [Space]
    public BirdInputActions inputActionAsset;
    public BirdInputActions.GameplayActions input;
    public WingPanelCreator wingPanelCreator;
    public BirdAnimator animator;
    public CameraFollow cameraFollow;
    public TestGUI GUI;
    [Space]





    // Simulation Constants
    const float g = 9.81f;
    const float localAirDensity = 1.225f; // Sea level air density (kg/m^3)
    const float deg2Rad = Mathf.Deg2Rad;
    const float rad2Deg = Mathf.Rad2Deg;
    const float maxGLoading = 10;
    float maxForce;


    // Input Settings
    public float initialSpeed;




    // Aerodynamic section storage

    List<WingSection> wingSectionsL;
    List<WingSection> wingSectionsR;
    List<WingPanel> wingPanels;

    TailPanel tailPanel;

    // LORD FORGIVE ME DELETE LATER PLEASE
    float AR_L;
    float AR_R;


    // Velocity storage
    Vector3 velocityWorld;
    Vector3 velocityLocal;

    Vector3 angularVelocityWorld;
    Vector3 angularVelocityLocal;
    //float yawRate;
    //float pitchRate;
    //float rollRate;


    // Position storage
    float bodyAlpha;
    float bodyBeta;

    float yaw;
    float pitch;
    float roll;


    // Net aerodynamic loads
    Vector3 wingLiftForce;
    Vector3 wingDragForce;
    Vector3 wingMoment;

    Vector3 tailLiftForce;
    Vector3 tailDragForce;
    Vector3 tailSlipForce;
    Vector3 tailMoment;



    // Input Storage
    float pitchInput = 0;
    float rollInput = 0;
    float yawInput = 0;
    float flapInput = 0;
    float flightConfigurationInput = 0;
    float cameraHorizontalInput = 0;

    float wingtipTwistL;
    float wingtipTwistR;
    float tailPitch;
    float tailRoll;


    // temp area
    Vector3 windVelocity = Vector3.zero;




    private void Awake() {
        // Setup input system
        inputActionAsset = new BirdInputActions();
        input = inputActionAsset.gameplay;

        
        int numSections = flightConfigurations.cruiseWingData.wingSectionData.Length;
        if (flightConfigurations.maneuverWingData.wingSectionData.Length != numSections || flightConfigurations.tuckedWingData.wingSectionData.Length != numSections) {
            Debug.LogError("All wing configurations must have the same number of sections.");
        }
    }


    void Start() {
        maxForce = maxGLoading * g * rb.mass;

        // Create wing sections from geometry data
        wingSectionsL = flightConfigurations.InterpolateWingSections(0, true, out float AR_L);
        wingSectionsR = flightConfigurations.InterpolateWingSections(0, false, out float AR_R);

        // Create tail panel
        tailPanel = new TailPanel(flightConfigurations.cruiseTailData, tailRoot);

        // Create wing panels from wing sections
        RecalculateWingPanels();

        UpdateCG();

        rb.velocity = cg.forward * initialSpeed;
    }




    void FixedUpdate() {

        UpdateVelocities();

        ReadInputs();

        wingSectionsL = flightConfigurations.InterpolateWingSections(flightConfigurationInput, true, out float AR_L);
        wingSectionsR = flightConfigurations.InterpolateWingSections(flightConfigurationInput, false, out float AR_R);

        // Move target angles towards target
        wingtipTwistL = wingtipTwistL.MoveTowards(wingtipTwistRange.GetAngle(rollInput), wingtipTwistRate * Time.fixedDeltaTime);
        wingtipTwistR = wingtipTwistR.MoveTowards(wingtipTwistRange.GetAngle(-rollInput), wingtipTwistRate * Time.fixedDeltaTime);
        tailPitch = tailPitch.MoveTowards(tailPitchRange.GetAngle(pitchInput), tailPitchRate * Time.fixedDeltaTime);
        tailRoll = tailRoll.MoveTowards(tailRollRange.GetAngle(-yawInput), tailRollRate * Time.fixedDeltaTime);

        // Update surface angles
        wingSectionsL[wingtipSectionIndex].twistControlOffset = wingtipTwistL;
        wingSectionsR[wingtipSectionIndex].twistControlOffset = wingtipTwistR;
        tailPanel.SetAngles(tailPitch, tailRoll);



        RecalculateWingPanels();



        // Perform aerodynamic calculations
        Vector3[] wingLoads = CalculateWingAerodynamics(wingPanels, velocityLocal, angularVelocityLocal);
        Vector3[] tailLoads = CalculateTailAerodynamics(tailPanel, velocityLocal, angularVelocityLocal);

        wingLiftForce = wingLoads[0];
        wingDragForce = wingLoads[1];
        wingMoment = wingLoads[2];

        tailLiftForce = tailLoads[0];
        tailDragForce = tailLoads[1];
        tailSlipForce = tailLoads[2];
        tailMoment = tailLoads[3];

        Vector3 thrustForce = flapInput * rb.mass * g * Vector3.forward;

        Vector3 netForce = wingLiftForce + wingDragForce + tailLiftForce + tailDragForce + tailSlipForce + thrustForce;
        netForce = (netForce.magnitude > maxForce) ? netForce.normalized * maxForce : netForce;
        Vector3 netMoment = wingMoment + tailMoment;
        netMoment = (netMoment.magnitude > maxForce) ? netMoment.normalized * maxForce : netMoment;

        rb.AddRelativeForce(netForce);
        rb.AddRelativeTorque(netMoment);

        // Calculate stability properties
        Vector3 netLift = wingLiftForce + tailLiftForce;
        Vector3 netDrag = wingDragForce + tailDragForce;
        float liftDragRatio = netLift.magnitude / netDrag.magnitude;
        

        UpdateCG();


        // Send data to GUI
        GUI.SetAirspeed(rb.velocity.magnitude);
        GUI.SetLD(liftDragRatio);
        
    }


    void Update() {
        // Wing lift drag moment vectors
        /*Debug.DrawRay(cg.position, cg.TransformVector(wingLiftForce), Color.green);
        Debug.DrawRay(cg.position, cg.TransformVector(wingDragForce), Color.red);
        Debug.DrawRay(cg.position, cg.TransformVector(wingMoment), Color.cyan);

        // Tail lift drag moment vectors
        Vector3 tailPosition = tailRoot.position + tailRoot.TransformVector(tailPanel.ACPosition);
        Debug.DrawRay(tailPosition, tailRoot.TransformVector(tailLiftForce), Color.green);
        Debug.DrawRay(tailPosition, tailRoot.TransformVector(tailDragForce), Color.red);
        Debug.DrawRay(tailPosition, tailRoot.TransformVector(tailSlipForce), Color.magenta);
        Debug.DrawRay(tailPosition, tailRoot.TransformVector(tailMoment), Color.cyan);*/
    }




    private void RecalculateWingPanels() {

        // Clear old debug quads
        wingPanelCreator.DestroyDebugQuads();

        // Create new ones, populate list
        wingPanels = new List<WingPanel>();
        wingPanels.AddRange(wingPanelCreator.CreateWingPanels(wingSectionsL, wingRoot, true, AR_L));
        wingPanels.AddRange(wingPanelCreator.CreateWingPanels(wingSectionsR, wingRoot, false, AR_R));

        // also make tail panel
        wingPanelCreator.CreateTailQuad(tailPanel, tailRoot);
    }





    private Vector3[] CalculateWingAerodynamics(List<WingPanel> wingPanels, Vector3 cgVelocityLocal, Vector3 rotationRateLocal) {
        // Runs aerodynamic calculations for all panels given.
        // Returns array of three vectors: [0] net lift force, [1] net drag force, [2] net moment

        Vector3 netLiftForce = Vector3.zero;
        Vector3 netDragForce = Vector3.zero;
        Vector3 netMoment = Vector3.zero;

        for(int i = 0; i < wingPanels.Count; i++) {

            WingPanel panel = wingPanels[i];

            // Get position of panel relative to cg
            Vector3 cgToRoot = wingRoot.localPosition - cg.localPosition;
            Vector3 panelPosition = panel.positionRoot + cgToRoot;

            // Calculate velocity at panel due to rotation
            Vector3 rotationVelocity = Vector3.Cross(rotationRateLocal, panelPosition);


            Vector3[] wingLoads = panel.CalculateAerodynamicLoads(cgVelocityLocal + rotationVelocity, localAirDensity);
            Vector3 liftForce = wingLoads[0];
            Vector3 dragForce = wingLoads[1];
            Vector3 pitchMoment = wingLoads[2];

            // add to running total of force and moment
            netLiftForce += liftForce;
            netDragForce += dragForce;
            // calculate moment around cg as cross product of lift&drag w panel position
            netMoment += Vector3.Cross(panel.positionRoot + cgToRoot, liftForce + dragForce);
            netMoment += pitchMoment;
        }

        return new Vector3[] { netLiftForce, netDragForce, netMoment };
    }


    private Vector3[] CalculateTailAerodynamics(TailPanel tail, Vector3 cgVelocityLocal, Vector3 rotationRateLocal) {
        // Runs aerodynamic calculations for the tail.
        // Returns array of three vectors: [0] net lift force, [1] net drag force, [2] net moment

        // Get position of panel relative to cg
        Vector3 cgToRoot = tailRoot.localPosition - cg.localPosition;
        Vector3 panelPosition = tailPanel.ACPosition + cgToRoot;

        // Calculate velocity at tail panel due to rotation
        Vector3 rotationVelocity = Vector3.Cross(rotationRateLocal, panelPosition);


        Vector3[] tailLoads = tail.CalculateAerodynamicLoads(cgVelocityLocal + rotationVelocity, localAirDensity);
        Vector3 liftForce = tailLoads[0];
        Vector3 dragForce = tailLoads[1];
        Vector3 slipForce = tailLoads[2];


        // calculate moment around cg as cross product of lift&drag w panel position
        Vector3 netMoment = Vector3.Cross(panelPosition, liftForce + dragForce + slipForce);
        

        return new Vector3[] { liftForce, dragForce, slipForce, netMoment };
    }





    void UpdateVelocities() {
        // Updates system state variables

        //velocityWorld = rb.velocity;
        velocityWorld = inWindTunnel ? testVelocity : rb.velocity;
        velocityLocal = cg.InverseTransformVector(velocityWorld);

        angularVelocityWorld = rb.angularVelocity;
        angularVelocityLocal = cg.InverseTransformVector(angularVelocityWorld);
        //localAngularVelocityRad = transform.InverseTransformDirection(testAngularVelocity);


        //yawRate = -localAngularVelocity.y;
        //pitchRate = localAngularVelocity.x;
        //rollRate = localAngularVelocity.z;
    }

    void UpdateCG() {
        rb.centerOfMass = cg.localPosition;
    }




    void ReadInputs() {
        pitchInput = input.Pitch.ReadValue<float>();
        rollInput = input.Roll.ReadValue<float>();
        yawInput = input.Yaw.ReadValue<float>();
        flapInput = input.Flap.ReadValue<float>();
        flightConfigurationInput = input.FlightConfiguration.ReadValue<float>();
        cameraHorizontalInput = input.CameraHorizontal.ReadValue<float>();
    }


    private void OnEnable() {
        input.Enable();
    }
    private void OnDisable() {
        input.Disable();
    }

    
    

}
