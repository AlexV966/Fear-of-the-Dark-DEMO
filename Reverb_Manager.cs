using UnityEngine;
using System.Collections.Generic;

public class Reverb_Manager : MonoBehaviour
{
    public float maxDistance = 40.0f; // The amximum value up to which the parameters are changed

    private List<GameObject> soundEmitters = new List<GameObject>();

    public float updateInterval = 1.0f; // Update interval in seconds

    private void Start()
    {
        // Start a coroutine to update reverb parameters at the specified interval
        StartCoroutine(UpdateReverbParameters());
    }

    private System.Collections.IEnumerator UpdateReverbParameters()
    {
        while (true) // Run indefinitely
        {
            foreach (GameObject soundEmitter in soundEmitters)
            {
                float distance = Vector3.Distance(transform.position, soundEmitter.transform.position);

                // Map the distance to a value between 0 and 1
                float scaledDistance = Mathf.Clamp01(1.0f - (distance / maxDistance));

                // Scale the value to an appropriate range
                float sendToAux = Mathf.Lerp(0.0f, 100.0f, scaledDistance);

                // Set the Wwise parameter controlling the reverb send for each emitter
                AkSoundEngine.SetRTPCValue("Wet_Level", sendToAux, soundEmitter);
            }

            // Wait for the specified update interval before updating parameters again
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sound_Emitter"))
        {

            soundEmitters.Add(other.gameObject);

            //Debug.Log("LayerMask hit");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sound_Emitter"))
        {
            soundEmitters.Remove(other.gameObject);
            //Debug.Log("LayerMask exit: " + other.gameObject.name);
        }
    }

    //for all emitters, calculate the distance to all and set the RTPCs for all. - expensive computationally.

    //regular reverb zones, but can set an rtpc

    //do this with an rtpc on reverb send direct to

    //Scale the value to an appropriate range
    //float reverbTime = Mathf.Lerp(0.0f, 100.0f, nonScaledDistance);
    //This is the function of the room size

    // Set the Wwise parameter controlling thwe reverb time
    //AkSoundEngine.SetRTPCValue("Reverb_Time", reverbTime, soundEmitter);

    // Set the Wwise parameter controlling the pre delay
    //AkSoundEngine.SetRTPCValue("Pre_Delay", preDelay, soundEmitter);
}
