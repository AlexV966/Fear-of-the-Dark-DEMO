using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection_Point : MonoBehaviour
{
    private Transform fatherTransform;

    private Vector3 followThisPosition;

    private string eventName;

    private float distanceFromOriginalEmitter;

    private float delayTime;

    private float reflectionVolume;

    public float minDistance = 0.0f;  // Minimum distance (0.0f)
    public float maxDistance = 30.0f; // Maximum distance (30.0f)
    public float minDelayTime = 0.02f;  // Minimum delay time (0.05f)
    public float maxDelayTime = 0.25f;  // Maximum delay time (0.5f)

    private void Start()
    {
        // Play the Wwise Event 
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the original emitter is destroyed
        if (fatherTransform == null)
        {
            // The original emitter is destroyed, so destroy this gameObject
            Destroy(gameObject);
            return; // Exit the Update method to avoid further processing
        }

        //Calculate the distance between the prefab and the emitter
        distanceFromOriginalEmitter = Vector3.Distance(fatherTransform.position, transform.position);

        //Keep the reflection prefab position the same as the ray hit point position even if the player moves
        transform.position = followThisPosition;

        //Calculate the delay time based on distance
        delayTime = MapValue(distanceFromOriginalEmitter, minDistance, maxDistance, minDelayTime, maxDelayTime);

        // Calculate the reflectionVolume based on distance
        reflectionVolume = MapValue(distanceFromOriginalEmitter, minDistance, maxDistance, 0.0f, 80.0f);

        // Ensure that reflectionVolume is clamped within the range [0.1, 0.8]
        reflectionVolume = Mathf.Clamp(reflectionVolume, 0.0f, 80.0f);
 
    }

    private IEnumerator DelayedActionCoroutine(float _delayTime)
    {
        // Wait for the specified _delayTime
        yield return new WaitForSeconds(_delayTime);

        // Set the Game Parameter value (0.0f to 1.0f) right before posting the event
        AkSoundEngine.SetRTPCValue("Early_Reflection_Fire", reflectionVolume, gameObject);
        
    }

    public void setAndTriggerTheEvent(string _eventName)
    {
        eventName = _eventName;

        // Start the coroutine to trigger the action after 3 seconds (adjust the time as needed)
        StartCoroutine(DelayedActionCoroutine(delayTime));
    }

    // Linear interpolation function
    private float MapValue(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }

    public void setOriginalEmitterLocation(Transform _fatherTransform)
    {
        fatherTransform = _fatherTransform;
    }

    public void setPositionToBeFollowed(Vector3 _followThisPosition)
    {
        followThisPosition = _followThisPosition;
    }


}
