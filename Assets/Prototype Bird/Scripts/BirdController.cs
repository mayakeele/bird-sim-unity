using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{

    //Debug Variables
    public Vector3 testVelocity;
    public Vector3 testAngularVelocity;


    // Wing Data
    public WingData wingData;

    // Component references
    public Rigidbody rb;
    public Transform cg;

    public WingPanelCreator wingPanelCreator;
    public BirdAnimator animator;
    public CameraFollow cameraFollow;
    


    // Wing geometry
    public Transform wingRoot;
    public List<WingSection> wingSectionsL;
    public List<WingSection> wingSectionsR; 


    // Simulation Constants
    const float g = 9.81f;
    const float rho = 1.225f; // Sea level air density (kg/m^3)
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



    // State variable storage
    List<WingPanel> wingPanels;

    Vector3 velocityWorld;
    Vector3 velocityBody;

    float yaw;
    float pitch;
    float roll;

    //float yawRate;
    //float pitchRate;
    //float rollRate;
    Vector3 localAngularVelocityRad;

    float bodyAlpha;
    float bodyBeta;


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

        // Create panels from wing sections
        RebuildWingPanels();

        UpdateCG();

        //rb.velocity = cg.forward * initialSpeed;
    }




    void FixedUpdate() {

        UpdateStateVariables();

        RebuildWingPanels();


        Vector3[] aeroForces = CalculateAerodynamics(wingPanels, velocityBody, localAngularVelocityRad);
        Vector3 netLiftForce = aeroForces[0];
        Vector3 netDragForce = aeroForces[1];
        Vector3 netMoment = aeroForces[2];

        rb.AddRelativeForce(netLiftForce + netDragForce);
        rb.AddRelativeTorque(netMoment);

        // Draw big vectors for lift and drag forces
        Debug.DrawRay(cg.position, cg.TransformVector(netLiftForce), Color.green);
        Debug.DrawRay(cg.position, cg.TransformVector(netDragForce), Color.red);

        Debug.Log("Lift (N): " + netLiftForce.magnitude);
        Debug.Log("Drag (N): " + netDragForce.magnitude);

    }




    private void RebuildWingPanels() {

        // Clear old debug quads
        wingPanelCreator.DestroyDebugQuads();

        // Clear old panels and create new ones, populate list
        wingPanels = new List<WingPanel>();
        wingPanels.AddRange(wingPanelCreator.CreateWingPanels(wingSectionsL, wingRoot, true));
        //wingPanels.AddRange(wingPanelCreator.CreateWingPanels(wingSectionsR, wingRoot, false));
    }




    private Vector3[] CalculateAerodynamics(List<WingPanel> wingPanels, Vector3 cgVelocity, Vector3 rotationRate) {
        // Runs aerodynamic calculations for all panels given.
        // Returns array of three vectors: [0] net lift force, [1] net drag force, [2] net moment

        Vector3 netLiftForce = Vector3.zero;
        Vector3 netDragForce = Vector3.zero;
        Vector3 netMoment = Vector3.zero;

        for(int i = 0; i < wingPanels.Count; i++) {
            WingPanel panel = wingPanels[i];

            Vector3 cgToRoot = wingRoot.localPosition - cg.localPosition;

            Vector3[] aeroForces = panel.CalculateLiftDragPitch(cgToRoot, cgVelocity, rotationRate, rho);
            Vector3 liftForce = aeroForces[0];
            Vector3 dragForce = aeroForces[1];
            Vector3 pitchMoment = aeroForces[2];

            // add to running total of force and moment
            netLiftForce += liftForce;
            netDragForce += dragForce;
            // calculate moment around cg as cross product of lift&drag w panel position
            netMoment += -Vector3.Cross(liftForce + dragForce, panel.positionRoot + cgToRoot);
            netMoment += pitchMoment;
        }

        return new Vector3[] { netLiftForce, netDragForce, netMoment };
    }




    void UpdateStateVariables() {
        // Updates system state variables

        velocityWorld = testVelocity;//rb.velocity;
        velocityBody = cg.InverseTransformDirection(velocityWorld);

        //localAngularVelocityRad = transform.InverseTransformDirection(rb.angularVelocity);
        localAngularVelocityRad = transform.InverseTransformDirection(testAngularVelocity);


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
