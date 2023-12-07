using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform;

    public float followDistance;


    private Vector3 velocity;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = followTransform.position - (velocity.normalized * followDistance);
        transform.LookAt(followTransform, Vector3.up);
    }



    public void SetVelocity(Vector3 playerVelocity){
        velocity = playerVelocity;
    }
}
