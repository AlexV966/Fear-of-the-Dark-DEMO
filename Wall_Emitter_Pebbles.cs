using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Emitter_Pebbles : MonoBehaviour
{
    public GameObject prefabToSpawn;  // The prefab to spawn when an object is detected underneath.
    public float maxRaycastDistance = 10f;  // The maximum distance to cast the ray.

    [SerializeField]
    private LayerMask detectionLayer;

    [SerializeField]
    private float minTimeframe;
    [SerializeField]
    private float maxTimeframe;

    // Define the range for random X and Y offsets.
    [SerializeField]
    private float minXOffset = 0.0f;
    [SerializeField]
    private float maxXOffset = 0.0f;
    [SerializeField]
    private float minYOffset = 0.0f;
    [SerializeField]
    private float maxYOffset = 0.0f;
    [SerializeField]
    private float minZOffset = 0.0f;
    [SerializeField]
    private float maxZOffset = 0.0f;

    private Vector3 originalPosition; // Store the original position of the GameObject.

    private void Start()
    {
        originalPosition = transform.position;

        // Start the coroutine to trigger the event randomly
        StartCoroutine(TriggerEventRandomly());
    }

    private IEnumerator TriggerEventRandomly()
    {
        while(true) // infinite loop
        {
            // Generate a random delay between 5 and 120 seconds
            float randomDelay = Random.Range(minTimeframe, maxTimeframe);

            // Generate random offsets for X and Y within the specified range.
            float randomXOffset = Random.Range(-minXOffset, maxXOffset);
            float randomYOffset = Random.Range(-minYOffset, maxYOffset);
            float randomZOffset = Random.Range(-minZOffset, maxZOffset);

            // Wait for the random delay
            yield return new WaitForSeconds(randomDelay);

            // Reset the transform to its original position.
            transform.position = originalPosition;

            // Modify the transform of the current GameObject with the random offsets.
            transform.position += new Vector3(randomXOffset, randomYOffset, randomZOffset);

            // Trigger the event on the specified object 
            AkSoundEngine.PostEvent("Play_Pebbles_Dislodging_Sum", gameObject);

            // Cast a ray directly downward to detect objects underneath.
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, maxRaycastDistance, detectionLayer, QueryTriggerInteraction.Ignore))
            {

                 // Spawn the prefab at the hit point with the same rotation as this GameObject.
                 Instantiate(prefabToSpawn, hit.point, Quaternion.identity);

            }
        }
        
    }
}
