using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birdbox : MonoBehaviour
{
    private bool scriptEnabled = false; 

    private bool reloadedBird = true;
    public AK.Wwise.Event birdOneCall;
    public AK.Wwise.Event birdTwoCall;
    public AK.Wwise.Event birdOneResponse;
    public AK.Wwise.Event birdTwoResponse;

    [SerializeField]
    private float minTimeframeMain;
    [SerializeField]
    private float maxTimeframeMain;

    [SerializeField]
    private float minTimeframeIntercall;
    [SerializeField]
    private float maxTimeframeIntercall;

    private int callCount = 2;

    // Define the range for random X, Y and Z offsets.
    [SerializeField]
    private float minXOffset;
    [SerializeField]
    private float maxXOffset;
    [SerializeField]
    private float minYOffset;
    [SerializeField]
    private float maxYOffset;
    [SerializeField]
    private float minZOffset;
    [SerializeField]
    private float maxZOffset;

    private float randomXOffset;
    private float randomYOffset;
    private float randomZOffset;

    private Vector3 originalPosition; // Store the original position of the GameObject.

    //Probability Value - calls/responses
    float randomValue;
    //Defines a threshold for the 50/50 split (0.5)
    float threshold = 0.5f;

    //Checks which of the events has played last
    int lastPlayedEvent = 1;

    //Flappy Bird To Instantiate
    public GameObject flappyBird;
    public int birdFlapProbability = 4;
    int randomProbabilityValue;


    void Start()
    {
        originalPosition = transform.position;

        // Start the coroutine to trigger the event randomly
        StartCoroutine(TriggerEventRandomly());

    }

    private IEnumerator TriggerEventRandomly()
    {
        while (true) // infinite loop
        {
            yield return null; // Yield to allow Unity to update and respond to events

            if (scriptEnabled)
            {
                
                // Generate a random delay between 5 and 120 seconds
                float randomDelayOne = Random.Range(minTimeframeMain, maxTimeframeMain);

                // Generate a random delay between 5 and 120 seconds
                float randomDelayTwo = Random.Range(minTimeframeIntercall, maxTimeframeIntercall);

                if (callCount > 1)
                {
                    // Wait for the random delay
                    yield return new WaitForSeconds(randomDelayOne);

                    callCount = 0;
                }
                else
                {
                    // Wait for the random delay
                    yield return new WaitForSeconds(randomDelayTwo);
                }

                //Flappy bird probability check
                
                randomProbabilityValue = Random.Range(1, birdFlapProbability + 1); // Generate a random number between 1 and the specified probability.

                // Check if the randomProbabilityValue equals 1 for a 1 in 4 chance (adjust the probability as needed).
                if (randomProbabilityValue == 1)
                {
                    // Instantiate the prefab at the spawn point.
                    Instantiate(flappyBird, transform.position, Quaternion.identity);
                    
                }

                // Reset the transform to its original position.
                transform.position = originalPosition;


                // Generate random offsets for X and Y within the specified range.
                randomXOffset = Random.Range(-minXOffset, maxXOffset);
                randomYOffset = Random.Range(-minYOffset, maxYOffset);
                randomZOffset = Random.Range(-minZOffset, maxZOffset);

                // Calculate the modified position for spawning the prefab
                Vector3 spawnPosition = originalPosition + new Vector3(randomXOffset, randomYOffset, randomZOffset);

                // Update the box's position based on the calculated spawnPosition
                transform.position = spawnPosition;

                if (lastPlayedEvent == 1)
                {
                    BirdCalls();

                    callCount++;
                }
                else
                {
                    BirdResponse();

                    callCount++;
                }
            }

        }

    }

    void BirdCalls()
    {
        if(reloadedBird)
        {
            // Generate a random number between 0 (inclusive) and 1 (exclusive)
            randomValue = Random.Range(0f, 1f);

            if (randomValue < threshold)
            {
                birdOneCall.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, MyEndOfEventCallback);
            }
            else
            {
                birdTwoCall.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, MyEndOfEventCallback);
            }

            reloadedBird = false;
            lastPlayedEvent = 0;
        }
       

    }

    void BirdResponse()
    {
        if(reloadedBird)
        {
            // Generate a random number between 0 (inclusive) and 1 (exclusive)
            randomValue = Random.Range(0f, 1f);

            if (randomValue < threshold)
            {
                birdOneResponse.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, MyEndOfEventCallback);
            }
            else
            {
                birdTwoResponse.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, MyEndOfEventCallback);
            }

            reloadedBird = false;
            lastPlayedEvent = 1;
        }
        

    }

    void MyEndOfEventCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent)
        {
            // Allows the next event to play now that the last one ended the next 
            reloadedBird = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player (you can customize the condition)
        if (other.CompareTag("Player"))
        {
            // Enable the script
            scriptEnabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the player (you can customize the condition)
        if (other.CompareTag("Player"))
        {
            // Disable the script
            scriptEnabled = false;
        }
    }
}






    

