using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlightController : MonoBehaviour
{


    // Component references
    public Rigidbody rb;
    public Transform cg;

    public BirdAnimator animator;
    public CameraFollow cameraFollow;



    // Simulation Constants
    const float g = 9.81f;
    const float rho = 1.225f; // Sea level air density (kg/m^3)
    const float deg2Rad = Mathf.Deg2Rad;
    const float rad2Deg = Mathf.Rad2Deg;



    // Wing Parameters
    float wingspan = 1.2f;
    float wingArea;
    private float wingAspectRatio;

    float wristLength; // Distance from the wrist joint to the wingtip
    float wristArea;
    float wristTwistMin;
    float wristTwistNeutral;
    float wristTwistMax;

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
    float pitch;
    float roll;
    float yaw;

    float pitchRate;
    float rollRate;
    float yawRate;

    float alpha;
    float beta;



    // Input Storage
    float pitchInput = 0;
    float rollInput = 0;
    float yawInput = 0;
    bool flapInput = false;
    bool brakeInput = false;
    Vector2 lookInput = Vector2.zero;






    void Awake()
    {


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        // Pass data to BirdAnimator
        //animator.SetWingAlpha(alpha, BirdAnimator.WingSide.Left);
        //animator.SetWingAlpha(alpha, BirdAnimator.WingSide.Right);

        cameraFollow.SetVelocity(rb.velocity);
    }





    // Input Events
    public void OnPitchInput(InputAction.CallbackContext context)
    {
        float i = -1 * context.action.ReadValue<float>();
        i = Mathf.Abs(i) > pitchDeadzone ? i : 0;

        pitchInput = Mathf.Abs(Mathf.Pow(i, pitchSmoothingPower)) * Mathf.Sign(i);
    }
    public void OnRollInput(InputAction.CallbackContext context)
    {
        float i = context.action.ReadValue<float>();
        i = Mathf.Abs(i) > rollDeadzone ? i : 0;

        rollInput = Mathf.Abs(Mathf.Pow(i, rollSmoothingPower)) * Mathf.Sign(i);
    }
    public void OnYawInput(InputAction.CallbackContext context)
    {
        float i = context.action.ReadValue<float>();
        i = Mathf.Abs(i) > yawDeadzone ? i : 0;

        yawInput = Mathf.Abs(Mathf.Pow(i, yawSmoothingPower)) * Mathf.Sign(i);
    }
    public void OnFlapInput(InputAction.CallbackContext context)
    {
        flapInput = context.action.IsPressed();
    }
    public void OnBrakeInput(InputAction.CallbackContext context)
    {
        flapInput = context.action.IsPressed();
    }
    public void OnLookInput(InputAction.CallbackContext context)
    {
        Vector2 i = context.action.ReadValue<Vector2>();
        lookInput = i.sqrMagnitude > lookDeadzoneSqr ? i : Vector2.zero;
    }





    float CalcWingArea(float span, float AR)
    {
        return Mathf.Pow(span, 2) / AR;
    }
    float CalcAR(float span, float wingArea)
    {
        return Mathf.Pow(span, 2) / wingArea;
    }
    float CalcChord(float span, float AR)
    {
        return span / AR;
    }



    float CalcLiftForce(float v, float S, float CL)
    {
        return 0.5f * rho * Mathf.Pow(v, 2) * S * CL;
    }
    float CalcCL(float lift, float v, float S)
    {
        return lift / (0.5f * rho * Mathf.Pow(v, 2) * S);
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


}
