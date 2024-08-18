using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling_Emitter : MonoBehaviour
{
    public GameObject prefabToSpawn;  // The prefab to spawn when an object is detected underneath.

    [SerializeField]
    private float minTimeframe;
    [SerializeField]
    private float maxTimeframe;

    // Define the range for random X and Y offsets.
    [SerializeField]
    private float minXOffset;
    [SerializeField]
    private float maxXOffset;
    [SerializeField]
    private float minZOffset;
    [SerializeField]
    private float maxZOffset;

    private float randomXOffset;
    private float randomZOffset;

    // Define the range for random rotation offsets.
    [SerializeField]
    private Vector3 minRotationOffset;
    [SerializeField]
    private Vector3 maxRotationOffset;


    private Vector3 originalPosition; // Store the original position of the GameObject.
    private Quaternion originalRotation; // Store the original rotation of the GameObject.

    private int spawnedPrefabCount = 0; // Counter for spawned prefabs.
    private int maxPrefabCount = 10; // Maximum number of allowed spawned prefabs.

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Start the coroutine to trigger the event randomly
        StartCoroutine(TriggerEventRandomly());
    }

    private IEnumerator TriggerEventRandomly()
    {
        while (true) // infinite loop
        {

            // Generate a random delay between 5 and 120 seconds
            float randomDelay = Random.Range(minTimeframe, maxTimeframe);

            // Wait for the random delay
            yield return new WaitForSeconds(randomDelay);

            if (spawnedPrefabCount < maxPrefabCount)
            {
                // Reset the transform to its original position.
                transform.position = originalPosition;
                transform.rotation = originalRotation;

                // Generate random offsets for X and Y within the specified range.
                randomXOffset = Random.Range(-minXOffset, maxXOffset);
                randomZOffset = Random.Range(-minZOffset, maxZOffset);

                // Calculate the modified position for spawning the prefab
                Vector3 spawnPosition = originalPosition + new Vector3(randomXOffset, 0.0f, randomZOffset);

                // Update the box's position based on the calculated spawnPosition
                transform.position = spawnPosition;

                // Generate random rotation offsets within the specified range.
                Vector3 randomRotationOffset = new Vector3(
                    Random.Range(minRotationOffset.x, maxRotationOffset.x),
                    Random.Range(minRotationOffset.y, maxRotationOffset.y),
                    Random.Range(minRotationOffset.z, maxRotationOffset.z)
                );

                // Apply the random rotation offsets to the current rotation.
                transform.Rotate(randomRotationOffset);

                // Trigger the event on the specified object 
                AkSoundEngine.PostEvent("Play_Pebbles_Dislodging_Sum", gameObject);


                // Spawn the prefab at the hit point with the same rotation as this GameObject.
                Instantiate(prefabToSpawn, spawnPosition, transform.rotation);

                // Increment the spawned prefab counter.
                spawnedPrefabCount++;
            }
           
        }

    }

    // Function to decrement the spawned prefab counter when a prefab is destroyed or despawned.
    public void DecrementSpawnedPrefabCount()
    {
        spawnedPrefabCount--;
    }
}
