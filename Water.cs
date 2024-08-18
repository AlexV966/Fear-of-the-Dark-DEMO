using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Water : MonoBehaviour
{
    FirstPersonMovement movement;
    private GroundCheck groundCheck;
    Jump jump;
    Crouch crouch;
    private Volume postProcessVolume;
    private Vignette vignette;
    FirstPersonAudio firstPersonAudio;

    private float timer = 30f;
    private Coroutine countdownCoroutine;
    private float initialVignetteIntensity = 0.0f;

    private bool isInTriggerZone = false;

    [SerializeField] private Volume attachedVolume;

    public bool isUnderwater = false;

    private bool hasTriggeredDrowningSound = false;

    private bool isGrounded;

    private void Start()
    {
        postProcessVolume = attachedVolume;
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGet(out vignette);
            // Store the initial vignette intensity
            initialVignetteIntensity = vignette.intensity.value;
        }

        firstPersonAudio = FindObjectOfType<FirstPersonAudio>();

        // Check if the components were found.
        if (firstPersonAudio == null)
        {
            Debug.LogError("firstPersonAudio component not found.");
        }

        groundCheck = FindObjectOfType<GroundCheck>();

        // Check if the components were found.
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck component not found.");
        }

    }

    private void FixedUpdate()
    {
        // Check if the player is currently grounded.
        isGrounded = groundCheck.isGrounded;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<FirstPersonMovement>() != null && !isGrounded)
        {
            movement = other.GetComponent<FirstPersonMovement>();
            movement.isSwimming = true;
            other.GetComponent<Jump>().enabled = false;
            other.GetComponent<Crouch>().enabled = false;

            //Debug.Log("Trigger_Enter_Player");

        }

        if (other.CompareTag("Eye Level"))
        {
            other.GetComponentInParent<Rigidbody>().useGravity = false;
           

            if (movement != null)
            {
                movement.ResetVelocity();
            }

            // Enable the PostProcessVolume component for drowning
            postProcessVolume.enabled = true;

            if (countdownCoroutine == null)
            {
                countdownCoroutine = StartCoroutine(CountdownCoroutine());
            }

            isUnderwater = true;
            //Debug.Log("Player is Underwater?: " + isUnderwater);

            movement.setStateSubmergedFPSMovement(isUnderwater);

            if(firstPersonAudio != null)
            {
                firstPersonAudio.setStateSubmerged();
            }
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<FirstPersonMovement>() != null)
        {
            movement = other.GetComponent<FirstPersonMovement>();
            jump = other.GetComponent<Jump>();
            crouch = other.GetComponent<Crouch>();

            // Use a coroutine to delay the OnTriggerExit event.
            StartCoroutine(ExitDelayCoroutinePlayer());

        }

        if (other.CompareTag("Eye Level"))
        {
            // Use a coroutine to delay the OnTriggerExit event.
            StartCoroutine(ExitDelayCoroutineEyeLevel());
            
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timer > 0f)
        {
            timer -= 1.0f * Time.deltaTime;

            // Adjust the vignette intensity based on the timer
            vignette.intensity.Override(Mathf.Lerp(initialVignetteIntensity, 1f, 1f - (timer / 30f)));

            // Sound
            if (timer < 15.0f && isUnderwater && !hasTriggeredDrowningSound)
            {
                AkSoundEngine.PostEvent("Play_Player_Drowning", gameObject);
                //Debug.Log("Player drowning Start");
                hasTriggeredDrowningSound = true; // Set the flag to true
            }

            Debug.Log("Vignette Intensity: " + vignette.intensity.value);

            yield return null;
        }

        

        FirstPersonMovement player = FindObjectOfType<FirstPersonMovement>();

        if (player != null)
        {
            player.triggerDeath();
        }


        ResetTimer();
    }

    private void ResetTimer()
    {
        timer = 30f;
    }

    // Coroutine to delay OnTriggerExit.
    private IEnumerator ExitDelayCoroutinePlayer()
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(0.0f);

        Debug.Log("Exit");

        // Only trigger exit logic if the object is still not in another trigger zone.
        if (!isInTriggerZone && isGrounded)
        {
            movement.ResetVelocity();
            jump.enabled = true;
            crouch.enabled = true;
            movement.isSwimming = false;
           
        }
    }

    // Coroutine to delay OnTriggerExit.
    private IEnumerator ExitDelayCoroutineEyeLevel()
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(0.0f);

        // Only trigger exit logic if the object is still not in another trigger zone.
        if (!isInTriggerZone)
        {
            isUnderwater = false;

            movement.setStateSubmergedFPSMovement(isUnderwater);

            //Debug.Log("Trigger_Exit_Player");
            //Debug.Log("Player is Underwater?: " + isUnderwater);

            if (firstPersonAudio != null)
            {
                firstPersonAudio.setStateSurfaced();
            }

            // Disable the PostProcessVolume component for drowning
            postProcessVolume.enabled = false;

            if (countdownCoroutine != null)
            {
                if (timer <= 20.0f && timer > 8.0f && !isUnderwater)
                {
                    AkSoundEngine.PostEvent("Play_Player_Gasps_For_Air_Medium", gameObject);
                }
                else if (timer <= 8.0f && !isUnderwater)
                {
                    AkSoundEngine.PostEvent("Play_Player_Gasps_For_Air_Fast", gameObject);
                }

                AkSoundEngine.PostEvent("Stop_Player_Drowning", gameObject);
                Debug.Log("Player drowning Stop");

                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
                ResetTimer();

                // Reset the vignette intensity
                vignette.intensity.Override(initialVignetteIntensity);

                // Reset the drowning sound trigger flag
                hasTriggeredDrowningSound = false;

            }
            
        }
    }



}
