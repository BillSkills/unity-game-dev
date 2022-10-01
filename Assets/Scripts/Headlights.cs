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
    public AudioClip onSound;
    public AudioClip offSound;

    public float angle;
    private float targetAngle;
    private float lightIntensity;
    private float targetIntensity;
    private AudioSource audioSource;
    private Light[] lights;

    private void Start() {
        lights = GetComponentsInChildren<Light>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && on == targetOn) // only run the toggle method when q is pressed AND the headlights arent transitioning
        {
            Toggle();
        }

        angle = angle + 5f * Time.deltaTime * (targetAngle - angle); // smoothly interpolate between current headlight angle and target headlight angle

        on = -targetAngle + angle < 10 && targetOn; // figure out if the light is on, and not transitioning between states

        if (on)
        {
            targetIntensity = onIntensity;
        }
        else
        {
            targetIntensity = 0;
        }

        lightIntensity = lightIntensity + 10f * Time.deltaTime * (targetIntensity - lightIntensity);  // interpolate the light intenisty

        foreach (Light light in lights)
        {
            light.intensity = lightIntensity; // set the intensity of the lights
        }

        transform.localRotation = Quaternion.Euler(angle, 0, 0); // rotate the headlights
    }

    void Toggle()
    {

        targetOn = !targetOn; // toggle the headlights when the q key gets pressed

        if (targetOn)
        {
            targetAngle = onAngle;
            audioSource.PlayOneShot(onSound);
        }
        else
        {
            targetAngle = offAngle;
            audioSource.PlayOneShot(offSound);
        }
    }
}
