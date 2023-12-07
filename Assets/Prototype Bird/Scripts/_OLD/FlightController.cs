using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlightController : MonoBehaviour
{

    // Component references
    public Rigidbody rb;
    public Transform cg;

    public WingPanelCreator wingPanelCreator;
    public BirdAnimator animator;
    public CameraFollow cameraFollow;
    public List<WingSection> wingSectionsLR;


    // Transform References
    public Transform wingRootR;
    public Transform wingRootL;


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
    Vector2 wingDihedral = new Vector2(8,-8);
    Vector2 wingSweep = new Vector2(-4, 10);

    Vector4 wingTwistL = new Vector2(3, 3);
    Vector4 wingTwistR = new Vector2(3, 3);




    // temp area
    Vector3 windVelocity = Vector3.zero;





    void Awake()
    {


    }

    // Start is called before the first frame update
    void Start()
    {
        //wingGeometry.CalculateGeometry();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateCG();
        //UpdateStateVariables();


        //// Project velocity vector onto coordinate system
        //Transform sectionTransform = CreateSectionTransform(true, 10, 10, 3);

        //Vector3 relativeVelocity = sectionTransform.InverseTransformDirection(velocityWorld - windVelocity);
        //float alpha = VelocityToAlpha(relativeVelocity);
        //float beta = VelocityToBeta(relativeVelocity);



        //// Pass data to BirdAnimator
        ////animator.SetWingAlpha(alpha, BirdAnimator.WingSide.Left);
        ////animator.SetWingAlpha(alpha, BirdAnimator.WingSide.Right);

        //cameraFollow.SetVelocity(rb.velocity);
    }




    void UpdateStateVariables() {
        // Updates system state variables
        position = cg.transform.position;
        velocityWorld = rb.velocity;
        velocityBody = transform.InverseTransformDirection(velocityWorld);

        Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity*rad2Deg);
        yawRate = -localAngularVelocity.y;
        pitchRate = localAngularVelocity.x;
        rollRate = localAngularVelocity.z;
    }

    void UpdateCG() {
        rb.centerOfMass = cg.localPosition;
    }




    float VelocityToAlpha(Vector3 localVelocity) {
        float alpha = Mathf.Atan(-localVelocity.y / localVelocity.z);
        return alpha * rad2Deg;
    }
    float VelocityToBeta(Vector3 localVelocity) {
        float beta = Mathf.Asin(-localVelocity.x / localVelocity.magnitude);
        return beta * rad2Deg;
    }


    Transform CreateSectionTransform(bool isLeft, float sweep, float dihedral, float twist) {

        float sign = isLeft ? 1 : -1;

        // Create a new coordinate system representing an airfoil transformed by sweep and dihedral
        Transform sectionTransform = gameObject.AddComponent<Transform>();
        sectionTransform.SetParent(transform);
        sectionTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        sectionTransform.RotateAround(transform.position, transform.forward, dihedral * sign);
        sectionTransform.RotateAround(transform.position, transform.up, sweep * sign);

        return sectionTransform;
    }




    float CalculateLift(float v, float S, float CL)
    {
        return 0.5f * rho * Mathf.Pow(v, 2) * S * CL;
    }


    Vector3 CalcLiftDirection(Vector3 velocityDirection)
    {
        // Returns a direction vector perpendicular to velocity, in the same plane as the up vector
        Vector3 liftDir = Vector3.ProjectOnPlane(transform.up, velocityDirection).normalized;
        return liftDir;
    }

    

    float CalcAlpha()
    {
        return 0;
    }

    float CalcBeta()
    {
        return 0;
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
