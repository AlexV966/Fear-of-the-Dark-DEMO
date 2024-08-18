using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drips_Prefab : MonoBehaviour
{
    private float randomDelay;
    private bool spawnedInObject = false;

    private void Start()
    {
        // Generate a random delay between 2 and 5 seconds.
        randomDelay = Random.Range(20.0f, 30.0f);

        // Wait for the random delay.
        StartCoroutine(DestroyAfterDelay(randomDelay));
          
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        if (spawnedInObject)
        {
            Destroy(gameObject);
        }
        else
        {
            // Trigger the event.
            AkSoundEngine.PostEvent("Play_Water_Drips", gameObject);

            // Wait for the specified delay.
            yield return new WaitForSeconds(delay);

            // Trigger the event.
            AkSoundEngine.PostEvent("Stop_Water_Drips", gameObject);

            // Destroy this object.
            Destroy(gameObject);
        }
         
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stone"))
        {
            spawnedInObject = true;
        }
        else
        {
            spawnedInObject = false;
        }

    }
}