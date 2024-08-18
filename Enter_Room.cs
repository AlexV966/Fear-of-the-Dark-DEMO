using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter_Room : MonoBehaviour
{
    // This function is called when the player enters the trigger zone.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player (you can use tags or layers for this check).
        if (other.CompareTag("Player"))
        {
            AkSoundEngine.SetState("General_Reverb", "Off");
            Debug.Log("Reverb_OFF");
        }
    }
}
