using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles_Prefab : MonoBehaviour
{
    private float randomDelay;
    private bool spawnedInObject = false;

    private void Start()
    {
        // Generate a random delay between 2 and 5 seconds.
        randomDelay = Random.Range(5.0f, 8.0f);

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
            AkSoundEngine.PostEvent("Play_Bubbles_environmental", gameObject);

            // Wait for the specified delay.
            yield return new WaitForSeconds(delay);

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