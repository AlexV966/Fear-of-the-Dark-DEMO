using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panic_Manager : MonoBehaviour
{
    public AK.Wwise.Event panicEvent;

    public float initialPanic = 0.0f; // Initial panic level.
    public float minPanic = 0.0f;     // Minimum panic level.
    public float maxPanic = 100.0f;   // Maximum panic level.
    public float panicChangeRate = 15.0f; // Rate at which panic changes per second

    private float currentPanic;  // Current panic level.

    private bool scarySoundUnchecked = false;

    private bool flashlightOn = false;

    private Flashlight flashlight;

    private FirstPersonMovement firstPersonMovement;

    public float panicAccelerationInSeconds = 100.0f;

    private void Start()
    {
        // Initialize the current panic level.
        currentPanic = initialPanic;

        if (panicEvent != null)
        {
            panicEvent.Post(gameObject);
        }

        flashlight = FindObjectOfType<Flashlight>();

        firstPersonMovement = FindObjectOfType<FirstPersonMovement>();

        //Adjusting the change rate
        panicChangeRate = 1.0f * (maxPanic / panicAccelerationInSeconds);
    }

    private void Update()
    {
        if (flashlight != null)
        {
            flashlightOn = flashlight.flashlightStatus;
        }

        if (scarySoundUnchecked)
        {
            IncreasePanicOverTime();
        }

        if (!scarySoundUnchecked && flashlightOn)
        {
            DecreasePanicOverTime();
        }

        // Ensure panic stays within the defined bounds.
        currentPanic = Mathf.Clamp(currentPanic, minPanic, maxPanic);

        // Update the Wwise RTPC with the current panic level.
        AkSoundEngine.SetRTPCValue("Player_Panic", currentPanic);

        Debug.Log("Panic: " + currentPanic);

        if (currentPanic >= 100.0f)
        {
            StartCoroutine(TriggerAfterDelay());
        }

    }

   //Start decreasing the panic over time
    private void DecreasePanicOverTime()
    {
        currentPanic -= panicChangeRate * Time.deltaTime;
    }

    //Start increasing the panic over time
    private void IncreasePanicOverTime()
    {
        currentPanic += panicChangeRate * Time.deltaTime;
    }

    public void IncreasePanic(float amount)
    {
        currentPanic += amount;
    }

    public void DecreasePanic(float amount)
    {
        currentPanic -= amount;
    }

    public void setScarySoundBool(bool _scarySound)
    {
        scarySoundUnchecked = _scarySound;
    }

    //Reset the panic meter to its initial value.
    public void ResetPanic()
    {
        currentPanic = initialPanic;
    }

    private IEnumerator TriggerAfterDelay()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5.0f);

        firstPersonMovement.triggerDeath();
    }

    public void setAccelerationMultiplier(float _panicAccelerationInSeconds)
    {
        panicAccelerationInSeconds = _panicAccelerationInSeconds;
    }
}