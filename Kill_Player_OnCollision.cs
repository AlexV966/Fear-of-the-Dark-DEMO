using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Player_OnCollision : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            FirstPersonMovement player = FindObjectOfType<FirstPersonMovement>();

            if (player != null)
            {
                player.triggerDeath();
            }

        }

        
        if (collision.gameObject.CompareTag("Stone"))
        {
            // Trigger the AK event.
            AkSoundEngine.PostEvent("Play_Debree_Ball_Fall_Impact_Medium_01", gameObject);

        }
    }


}
