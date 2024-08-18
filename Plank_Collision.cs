using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank_Collision : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the object you want to react to.
        if (collision.gameObject.CompareTag("Stone")) 
        {

            // Trigger the AK event.
            AkSoundEngine.PostEvent("Play_Player_Jump_Impact_Wood", gameObject);
        }
    }
}
