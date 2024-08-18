using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Ambience_Extras_Emitter : MonoBehaviour
{
    public GameObject prefabDrips;
    public GameObject prefabBubbles;

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

    public int dripProbability = 4;
    private int randomProbabilityValueDrip;
    private bool waterDripsPlayed = false;

    public int bubbleProbability = 10;
    private int randomProbabilityValueBubble;
    private bool bubblesEnvironmentalPlayed = false;

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

            // Reset the transform to its original position.
            transform.position = originalPosition;
            transform.rotation = originalRotation;

            // Generate random offsets for X and Y within the specified range.
            randomXOffset = Random.Range(-minXOffset, maxXOffset);
            randomZOffset = Random.Range(-minZOffset, maxZOffset);

            // Calculate the modified position for spawning the prefab
            Vector3 spawnPosition = originalPosition + new Vector3(randomXOffset, 0.0f, randomZOffset);

            // Update the box's position based on the calculated spawnPosition
            //transform.position = spawnPosition;

            // Generate random rotation offsets within the specified range.
            Quaternion randomRotationOffset = Quaternion.Euler(
                Random.Range(minRotationOffset.x, maxRotationOffset.x),
                Random.Range(minRotationOffset.y, maxRotationOffset.y),
                Random.Range(minRotationOffset.z, maxRotationOffset.z)
            );

            // Apply the random rotation offsets to the current rotation.
            Quaternion newRotation = transform.rotation * randomRotationOffset;

            // Apply the random rotation offsets to the current rotation.
            //transform.Rotate(randomRotationOffset);

            randomProbabilityValueDrip = Random.Range(1, dripProbability + 1); // Generate a random number between 1 and the specified probability.

            if(!waterDripsPlayed)
            {
                if (randomProbabilityValueDrip == 1)
                {
                    // Spawn the prefab at the hit point with the same rotation as this GameObject.
                    Instantiate(prefabDrips, spawnPosition, newRotation);
                    waterDripsPlayed = true; // Mark the event as played
                    bubblesEnvironmentalPlayed = false;
                }
            }
            

            randomProbabilityValueBubble = Random.Range(1, bubbleProbability + 1); // Generate a random number between 1 and the specified probability.

            if(!bubblesEnvironmentalPlayed)
            {
                if (randomProbabilityValueBubble == 1)
                {
                    // Spawn the prefab at the hit point with the same rotation as this GameObject.
                    Instantiate(prefabBubbles, spawnPosition, newRotation);
                    bubblesEnvironmentalPlayed = true; // Mark the event as played
                    waterDripsPlayed = false;
                }
            }
           



        }

    }
}
