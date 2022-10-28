using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    // global variables
    private Rigidbody rb;

    public bool front;
    public bool right;

    [Header("Suspension")]
    public float restLength;
    public float springTravel;
    public float strutStiffness;
    public float damperStiffness;

    private float minLength;
    private float maxLength;
    private float springLength;
    private float previousSpringLength;
    private float springForce;
    private float springVelocity;
    private float damperForce;
    private Vector3 strutForceVector;
    private float strutForce;

    [Header("Wheels")]
    public float wheelDiameter;
    public Transform visualWheel;
    public float offsetY;
    public float steerAngle;
    public float frictionStatic;
    public float frictionKinetic;
    public float maxGrip;
    public bool isSlipping;

    private float wheelRadius;
    private Vector3 localWheelVelocity;
    private float fx;
    private float fy;



    void Start()
    {
        rb = transform.parent.parent.GetComponent<Rigidbody>(); // use 2 ".parent"s because this script goes on the wheels, which are in the "wheels" object, which are a child of the car which has the rigidbody

        // calculations to get the more usefull versions of the paramaters
        wheelRadius = wheelDiameter / 2;
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;

        maxGrip = frictionStatic;
    }

    void Update()
    {
        transform.localRotation = Quaternion.Euler(transform.up * steerAngle);
    }

    void FixedUpdate() // use fixedupdate instead of update for physics so the time interval is consistent
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius)) // see if the wheel would be touching the ground
        {
            previousSpringLength = springLength;

            springLength = hit.distance - wheelRadius; // calculate the distance from the wheel to the car based on the size of the wheel and the distance from the car to the ground
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (previousSpringLength - springLength) / Time.fixedDeltaTime; // calculate velocity of the spring, literally delta d / delta t
            springForce = strutStiffness * (restLength - springLength); // calculate the force the spring is applying by hooke's law
            damperForce = damperStiffness * springVelocity; // yep
            strutForce = springForce + damperForce;
            strutForceVector = strutForce * transform.up; // combine spring and damper forces, and convert to a vector

            localWheelVelocity = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point)); // get the velocity of the wheel in local space

            fx = (!front)? Input.GetAxis("Vertical") * strutForce: 0; // calculate a sort of "thrust", will replace this with something else later
            fy = localWheelVelocity.x * strutForce; // simulate lateral friction on the wheel

            isSlipping = Mathf.Sqrt(fx*fx + fy*fy)>maxGrip; // figure out if the wheels are slipping
            maxGrip = (isSlipping)? frictionKinetic*strutForce: frictionStatic*strutForce; 

            fy = Mathf.Clamp(fy, -maxGrip, maxGrip); // limit forces as to not exceed grip
            fx = Mathf.Clamp(fx, -maxGrip, maxGrip);

        
            rb.AddForceAtPosition(strutForceVector + (fx * transform.forward) + (fy * -transform.right), hit.point); // apply the forces

            visualWheel.position = transform.position + new Vector3(0, wheelDiameter-springLength+offsetY, 0); // move the visual wheel
        }
    }
}
