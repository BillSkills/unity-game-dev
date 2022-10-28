using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drivetrain : MonoBehaviour
{
    public Wheel[] driveWheels;

    [Header("Engine")]
    public float redLine;
    public float engineRPM = 0;

    public float enginePower;
    
    public float decay;

    [Header("Transmission")]
    public float[] gearRatios;
    public float differentialRatio;

    private int gear = 0;
    private float totalRatio;
    private float throttle;
    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) gear = 0; // theres probably a better way to do this
        if(Input.GetKeyDown(KeyCode.Alpha2)) gear = 1;
        if(Input.GetKeyDown(KeyCode.Alpha3)) gear = 2;
        if(Input.GetKeyDown(KeyCode.Alpha4)) gear = 3;
        if(Input.GetKeyDown(KeyCode.Alpha5)) gear = 4;

        totalRatio = gearRatios[gear] * differentialRatio;

        throttle = 2 * Input.GetAxis("Vertical") - 1; // map the thing from -1 to +1 into 0 to + 1, this would mean that the engine rpm decreases at the same rate that it increases, this code will probably be different for cars that have a turbocharger or supercharger
        engineRPM = engineRPM + throttle * enginePower;

        engineRPM = Mathf.Clamp(engineRPM, 0, redLine);
    }
}
