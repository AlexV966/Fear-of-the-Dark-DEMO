using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_Room : MonoBehaviour
{
    private void Start()
    {
        AkSoundEngine.SetState("General_Reverb", "On");
        //Debug.Log("Reverb_ON");
    }

    // This function is called when the player enters the trigger zone.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player (you can use tags or layers for this check).
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Reverb_ON");
            AkSoundEngine.SetState("General_Reverb", "On");
        }
    }
}
