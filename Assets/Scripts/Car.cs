using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    public Wheel[] wheels;

    [Header("Dimentions")]
    public float wheelBase;
    public float trackWidth;
    public float turningRadius;

    private float steerInput;
    private float steerRight;
    private float steerLeft;

    void Update()
    {
        Steer();
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

        foreach (Wheel wheel in wheels)
        {
            if (wheel.front)
            {
                if (wheel.right)
                {
                    wheel.steerAngle = steerRight;
                }
                else
                {
                    wheel.steerAngle = steerLeft;
                }
            }
        }
    }
}

