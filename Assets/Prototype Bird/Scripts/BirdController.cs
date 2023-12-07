using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{

    //Debug Variables
    public bool inWindTunnel;
    public Vector3 testVelocity;
    public Vector3 testAngularVelocity;


    // Wing & Tail Data
    public WingData wingData;
    public TailData tailData;

    // Component references
    public Rigidbody rb;
    public Transform cg;

    public WingPanelCreator wingPanelCreator;
    public BirdAnimator animator;
    public CameraFollow cameraFollow;
    

    // Geometry
    public Transform wingRoot;
    public Transform tailRoot;



    // Simulation Constants
    const float g = 9.81f;
    const float localAirDensity = 1.225f; // Sea level air density (kg/m^3)
    const float deg2Rad = Mathf.Deg2Rad;
    const float rad2Deg = Mathf.Rad2Deg;



    // Tail Properties
    float tailMinArea;
    float tailMaxArea;


    // Input Settings
    public float initialSpeed;

    float pitchDeadzone = 0.01f;
    float pitchSmoothingPower = 1;

    float rollDeadzone = 0.01f;
    float rollSmoothingPower = 1;

    float yawDeadzone = 0.01f;
    float yawSmoothingPower = 1;

    float lookDeadzoneSqr = 0.01f * 0.01f;




    // Aerodynamic section storage

    List<WingSection> wingSectionsL;
    List<WingSection> wingSectionsR;
    List<WingPanel> wingPanels;

    TailPanel tailPanel;


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
    Vector3 tailMoment;



    // Input Storage
    float pitchInput = 0;
    float rollInput = 0;
    float yawInput = 0;
    bool flapInput = false;
    bool brakeInput = false;
    Vector2 lookInput = Vector2.zero;


    // Control variables
    float[] wingDihedral = new float[] { 8, -8 };
    float[] wingSweep = new float[] { -4, 10 };

    float[] wingTwistL = new float[] { 3, 3 };
    float[] wingTwistR = new float[] { 3, 3 };

    // temp area
    Vector3 windVelocity = Vector3.zero;






    void Start() {
        // Create wing sections from geometry data
        wingSectionsL = wingData.CreateWingSections();
        wingSectionsR = wingData.CreateWingSections();

        // Create wing panels from wing sections
        RebuildWingPanels();

        // Create tail panel
        tailPanel = new TailPanel(tailData, tailRoot);


        UpdateCG();

        rb.velocity = cg.forward * initialSpeed;
    }




    void FixedUpdate() {

        // Get user input and apply to geometry


        // Update the state of the bird
        UpdateVelocities();
        UpdateCG();

        RebuildWingPanels();


        // Perform aerodynamic calculations
        Vector3[] wingLoads = CalculateWingAerodynamics(wingPanels, velocityLocal, angularVelocityLocal);
        Vector3[] tailLoads = CalculateTailAerodynamics(tailPanel, velocityLocal, angularVelocityLocal);

        wingLiftForce = wingLoads[0];
        wingDragForce = wingLoads[1];
        wingMoment = wingLoads[2];

        tailLiftForce = tailLoads[0];
        tailDragForce = tailLoads[1];   
        tailMoment = tailLoads[2];

        rb.AddRelativeForce(wingLiftForce + wingDragForce + tailLiftForce + tailDragForce);
        rb.AddRelativeTorque(wingMoment + tailMoment);

        
        //Debug.Log("Lift (N): " + wingLiftForce.magnitude);
        //Debug.Log("Drag (N): " + netDragForce.magnitude);
        //Debug.Log("Moment (Nm): " + netMoment.magnitude);
    }


    void Update() {
        // Wing lift drag moment vectors
        Debug.DrawRay(cg.position, cg.TransformVector(wingLiftForce), Color.green);
        Debug.DrawRay(cg.position, cg.TransformVector(wingDragForce), Color.red);
        Debug.DrawRay(cg.position, cg.TransformVector(wingMoment), Color.blue);

        // Tail lift drag moment vectors
        Vector3 tailPosition = tailRoot.position + tailRoot.TransformVector(tailPanel.ACPosition);
        Debug.DrawRay(tailPosition, tailRoot.TransformVector(tailLiftForce), Color.green);
        Debug.DrawRay(tailPosition, tailRoot.TransformVector(tailDragForce), Color.red);
        Debug.DrawRay(tailPosition, tailRoot.TransformVector(tailMoment), Color.blue);
    }




    private void RebuildWingPanels() {

        // Clear old debug quads
        wingPanelCreator.DestroyDebugQuads();

        // Clear old panels and create new ones, populate list
        wingPanels = new List<WingPanel>();
        wingPanels.AddRange(wingPanelCreator.CreateWingPanels(wingSectionsL, wingRoot, true));
        wingPanels.AddRange(wingPanelCreator.CreateWingPanels(wingSectionsR, wingRoot, false));
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


        // calculate moment around cg as cross product of lift&drag w panel position
        Vector3 netMoment = Vector3.Cross(tail.ACPosition + cgToRoot, liftForce + dragForce);
        

        return new Vector3[] { liftForce, dragForce, netMoment };
    }





    void UpdateVelocities() {
        // Updates system state variables

        //velocityWorld = rb.velocity;
        velocityWorld = inWindTunnel ? testVelocity : rb.velocity;
        velocityLocal = cg.InverseTransformDirection(velocityWorld);

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





    // Input Events
    public void OnPitchInput(InputAction.CallbackContext context) {
        float i = -1 * context.action.ReadValue<float>();
        i = Mathf.Abs(i) > pitchDeadzone ? i : 0;

        pitchInput = Mathf.Abs(Mathf.Pow(i, pitchSmoothingPower)) * Mathf.Sign(i);
    }
    public void OnRollInput(InputAction.CallbackContext context) {
        float i = context.action.ReadValue<float>();
        i = Mathf.Abs(i) > rollDeadzone ? i : 0;

        rollInput = Mathf.Abs(Mathf.Pow(i, rollSmoothingPower)) * Mathf.Sign(i);
    }
    public void OnYawInput(InputAction.CallbackContext context) {
        float i = context.action.ReadValue<float>();
        i = Mathf.Abs(i) > yawDeadzone ? i : 0;

        yawInput = Mathf.Abs(Mathf.Pow(i, yawSmoothingPower)) * Mathf.Sign(i);
    }
    public void OnFlapInput(InputAction.CallbackContext context) {
        flapInput = context.action.IsPressed();
    }
    public void OnBrakeInput(InputAction.CallbackContext context) {
        flapInput = context.action.IsPressed();
    }
    public void OnLookInput(InputAction.CallbackContext context) {
        Vector2 i = context.action.ReadValue<Vector2>();
        lookInput = i.sqrMagnitude > lookDeadzoneSqr ? i : Vector2.zero;
    }


}
