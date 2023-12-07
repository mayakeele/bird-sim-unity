using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
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
    Vector3 position;
    Vector3 velocityWorld;
    Vector3 velocityBody;

    float yaw;
    float pitch;
    float roll;

    float yawRate;
    float pitchRate;
    float rollRate;

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






    // Start is called before the first frame update
    void Start() {
        // Create wing sections from geometry data
        wingSectionsL = wingData.CreateWingSections();
        wingSectionsR = wingData.CreateWingSections();

        wingPanelCreator.CreateWingPanels(wingSectionsL, wingRoot, true);
        wingPanelCreator.CreateWingPanels(wingSectionsR, wingRoot, false);

    }

    // Update is called once per frame
    void Update() {

    }




    void UpdateStateVariables() {
        // Updates system state variables
        position = cg.transform.position;
        velocityWorld = rb.velocity;
        velocityBody = transform.InverseTransformDirection(velocityWorld);

        Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity * rad2Deg);
        yawRate = -localAngularVelocity.y;
        pitchRate = localAngularVelocity.x;
        rollRate = localAngularVelocity.z;
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
