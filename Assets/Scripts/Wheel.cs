using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    // global variables
    private Rigidbody wheelRb;

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
    private Vector3 strutForce;

    [Header("Wheels")]
    public float wheelDiameter;

    private float wheelRadius;


    void Start()
    {
        wheelRb = transform.parent.parent.GetComponent<Rigidbody>(); // use 2 ".parent"s because this script goes on the wheels, which are in the "wheels" object, which are a child of the car which has the rigidbody

        // calculations to get the more usefull versions of the paramaters
        wheelRadius = wheelDiameter/2;
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    void FixedUpdate() // use fixedupdate instead of update so the time interval is consistent
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius)) // see if the wheel would be touching the ground
        {
            previousSpringLength = springLength;

            springLength = hit.distance - wheelRadius; // calculate the distance from the wheel to the car based on the size of the wheel and the distance from the car to the ground
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (previousSpringLength - springLength)/Time.fixedDeltaTime; // calculate velocity of the spring, literally delta d / delta t
            springForce = strutStiffness * (restLength - springLength); // calculate the force the spring is applying by hooke's law
            damperForce = damperStiffness * springVelocity; // yep
            strutForce = (springForce + damperForce) * transform.up; // combine spring and damper forces, and convert to a vector

            wheelRb.AddForceAtPosition(strutForce, hit.point); // apply the force
        }
    }
}
