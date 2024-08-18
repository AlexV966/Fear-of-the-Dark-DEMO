using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Emitter_1_A : MonoBehaviour
{

    private void Start()
    {
        // Generate a random delay between 2 and 5 seconds.
        float randomDelay = Random.Range(0.1f, 0.7f);

        // Wait for the random delay.
        StartCoroutine(DestroyAfterDelay(randomDelay));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(delay);

        // Fire a raycast downward.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // Check if the hit object has a tag.
            if (hit.collider.CompareTag("Water"))
            {
                //// Perform actions based on the tag.
                //Debug.Log("Hit object with tag: " + hit.collider.tag);

                // Trigger the event.
                AkSoundEngine.PostEvent("Play_Grit_Hits_Water", gameObject);
                AkSoundEngine.PostEvent("Play_Stones_hit_water_switch", gameObject);
            }
            else
            {
                // Trigger the event.
                AkSoundEngine.PostEvent("Play_Pebbles_hit_floor_sequence", gameObject);
            }
        }

        // Destroy this object.
        Destroy(gameObject);
    }
}
    


