using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    private Rigidbody rb;
    public Wheel[] steeringWheels;

    [Header("Dimentions")]
    public float wheelBase;
    public float trackWidth;
    public float turningRadius;

    private float steerInput;
    private float steerRight;
    private float steerLeft;

    [Header("Headlights")]
    public Headlights headlights;
    [Header("Drag")]
    public float dragHeadlightsDown;
    public float dragHeadlightsUp;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Steer();

        if (headlights.on)
        {
            rb.drag = dragHeadlightsUp;
        }
        else
        {
            rb.drag = dragHeadlightsDown;
        }


    }

    void Steer()
    {
        steerInput = Input.GetAxis("Horizontal");

        if (steerInput > 0) // turning right
        {
            steerRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turningRadius - (trackWidth / 2))) * steerInput;
            steerLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turningRadius + (trackWidth / 2))) * steerInput;
        }
        else if (steerInput < 0) // turning left
        {
            steerRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turningRadius + (trackWidth / 2))) * steerInput;
            steerLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turningRadius - (trackWidth / 2))) * steerInput;
        }
        else
        {
            steerRight = 0;
            steerLeft = 0;
        }

        foreach (Wheel wheel in steeringWheels)
        {
            if (wheel.right) wheel.steerAngle = steerRight;
            else wheel.steerAngle = steerLeft;
        }
    }
}

