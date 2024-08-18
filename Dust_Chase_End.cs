using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust_Chase_End : MonoBehaviour
{
    private bool noRepeat = false;

    //Cave-in Rocks
    public GameObject rockPrefab;
    [SerializeField]
    private float caveInDelay = 5.0f;
    [SerializeField]
    private float numberOfRocks = 5.0f;

    private void OnTriggerExit(Collider other)
    {
        // Check if the entering object is the player.
        if (other.CompareTag("Player"))
        {
            if(!noRepeat)
            {
                Smoke_Track smokeTrack = FindObjectOfType<Smoke_Track>();

                smokeTrack.endChase();

                noRepeat = true;

                StartCoroutine(InstantiatePrefabsWithDelay());
            }
           
        }
    }

    private IEnumerator InstantiatePrefabsWithDelay()
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(caveInDelay);

        // Instantiate the prefabs.
        for (int i = 0; i < numberOfRocks; i++)
        {

            Vector3 spawnPosition = new Vector3(2813, -16, Random.Range(-3089, -3116));

            Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(0.2f);

        }
    }

    public void resetDustChaseEnd()
    {
        noRepeat = false;
    }
      
    
}
