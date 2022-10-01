using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headlights : MonoBehaviour
{
    public float onIntensity;
    public bool targetOn;
    public bool on;
    public float onAngle;
    public float offAngle;

    public float angle;
    private float targetAngle;
    private float lightIntensity;
    private float targetIntensity;

    void Update()
    {
        
        if (targetOn)
        {
            targetAngle = onAngle;
        }
        else
        {
            targetAngle = offAngle;
        }
        angle = angle + 5f * Time.deltaTime * (targetAngle - angle); // smoothly interpolate between current headlight angle and target headlight angle

        on = -targetAngle+angle < 10 && targetOn; // figure out if the light is on, and not transitioning between states

        if (on)
        {
            targetIntensity = onIntensity;
        } else {
            targetIntensity = 0;
        }

        lightIntensity = lightIntensity + 10f * Time.deltaTime * (targetIntensity - lightIntensity);  // interpolate the light intenisty

        foreach (Light light in GetComponentsInChildren<Light>())
        {
            light.intensity = lightIntensity; // set the intensity of the lights
        }

        transform.localRotation = Quaternion.Euler(angle, 0, 0); // rotate the headlights
    }
}
