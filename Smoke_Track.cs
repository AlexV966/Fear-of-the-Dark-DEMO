using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke_Track : MonoBehaviour
{
    public Transform player;
    public GameObject dustPrefab;

    [SerializeField]
    private float distanceThreshold;
    private Vector3 trackingPoint;

    private float directionalDistance;

    private bool startChase = false;

    private bool earthquakeOver = false;

    private bool notARetrigger = true;

    //Cave-in Rocks
    public GameObject rockPrefab;
    [SerializeField]
    private float caveInDelay = 5.0f;
    [SerializeField]
    private float numberOfRocks = 5.0f;

    private List<GameObject> instantiatedDustPrefabs = new List<GameObject>();
    private List<GameObject> instantiatedRockPrefabs = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        // Set the initial tracking point to the current position of the AI box.
        trackingPoint = transform.position;
    }

    void Update()
    {
        if (startChase && earthquakeOver)
        {

            // Calculate the distance only in the -X +Z direction.
            directionalDistance = player.position.x - trackingPoint.x + trackingPoint.z - player.position.z;

            //Debug.Log("Player distance from tracking point is: " + Mathf.Abs(directionalDistance));

            // Check if the player has moved away by a certain distance.
            if (Mathf.Abs(directionalDistance) > distanceThreshold)
            {
                Debug.Log("Trigger Dust");

                // Instantiate the prefab at the tracking point.
                GameObject dustInstance = Instantiate(dustPrefab, trackingPoint, Quaternion.identity);

                instantiatedDustPrefabs.Add(dustInstance);

                // Update the tracking point to the player's current position.
                trackingPoint = player.position;
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player.
        if (other.CompareTag("Player"))
        {
            if (notARetrigger)
            {
                notARetrigger = false;

                startChase = true;
                StartCoroutine(InstantiatePrefabsWithDelay());
                StartCoroutine(EarthquakeTremor());

                Panic_Manager panicManager;
                panicManager = FindObjectOfType<Panic_Manager>();
                panicManager.setAccelerationMultiplier(15);
            }
            
        }

    }

    public void endChase()
    {
        startChase = false;
    }

    private IEnumerator InstantiatePrefabsWithDelay()
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(caveInDelay);

        // Instantiate the prefabs.
        for (int i = 0; i < numberOfRocks; i++)
        {

            Vector3 spawnPosition = new Vector3(Random.Range(2894, 2898), -15, Random.Range(-3166, -3170));

            GameObject rockInstance = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

            instantiatedRockPrefabs.Add(rockInstance);

            yield return new WaitForSeconds(0.2f);

        }
        
    }

    private IEnumerator EarthquakeTremor()
    {

        AkSoundEngine.PostEvent("Play_Earthquake", gameObject);
        StartCoroutine(EarthquakeExtras());

        //Start camera shake earthquake effect

        CameraShake earthquake = FindObjectOfType<CameraShake>();

        if (earthquake != null)
        {
            earthquake.ShakeCamera();
        }

        yield return new WaitForSeconds(10.0f);

        earthquake.stopCameraShake();
        
        AkSoundEngine.PostEvent("Stop_Earthquake", gameObject);
        earthquakeOver = true;

    }

    private IEnumerator EarthquakeExtras()
    {
        int minIterations = 2;
        int maxIterations = 4;
        int numIterations = Random.Range(minIterations, maxIterations + 1); // Random number of iterations between 2 and 4

        float minDelay = 1.5f;
        float maxDelay = 4.0f;
        float delay = Random.Range(minDelay, maxDelay);

        for (int i = 0; i < numIterations; i++)
        {

            AkSoundEngine.PostEvent("Play_Pebbles_hit_floor_sequence", gameObject);

            yield return new WaitForSeconds(delay);

        }
    }

    public void resetDustChase()
    {
         startChase = false;

         earthquakeOver = false;

        notARetrigger = true;

        trackingPoint = transform.position;

        ResetAllDustPrefabs();

        ResetAllRockPrefabs();
    }

    // Reset or destroy all instantiated prefabs.
    public void ResetAllDustPrefabs()
    {
        foreach (GameObject dustInstance in instantiatedDustPrefabs)
        {
            // You can reset or destroy the prefab instances here.
            Destroy(dustInstance);
        }

        // Clear the list since all instances are now reset or destroyed.
        instantiatedDustPrefabs.Clear();
    }

    // Reset or destroy all instantiated prefabs.
    public void ResetAllRockPrefabs()
    {
        foreach (GameObject rockInstance in instantiatedRockPrefabs)
        {
            // For example, to destroy them:
            Destroy(rockInstance);
        }

        // Clear the list since all instances are now reset or destroyed.
        instantiatedRockPrefabs.Clear();
    }

}