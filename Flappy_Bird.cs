using UnityEngine;
using System.Collections;

public class Flappy_Bird : MonoBehaviour
{
    public AK.Wwise.Event flappyBirdOne;
    //public int scaleFactor;

    private Vector3 startPosition; // The starting position of the movement.
    private Vector3 endPosition;   // The ending position of the movement.
    private float randomDuration;  // The duration of the movement in seconds.
    //private float eventDuration;
    private float elapsedTime; 

    private float minTimeDuration = 2.0f;
    private float maxTimeDuration = 5.0f;

    // Define the range for random X, Y and Z offsets.
    private float minXOffset = 30;
    private float maxXOffset = 60;
    private float minYOffset = 15;
    private float maxYOffset = 15;
    private float minZOffset = 30;
    private float maxZOffset = 60;

    private float randomXOffset;
    private float randomYOffset;
    private float randomZOffset;

    private Vector3 originalPosition; // Store the original position of the GameObject.

    private void Start()
    {
        originalPosition = transform.position;

        // Trigger the sound
        AkSoundEngine.PostEvent("Play_Bird_flapping_sequence", gameObject);

        // Start the movement coroutine.
        StartCoroutine(MoveObjectCoroutine());

    }

    

    private IEnumerator MoveObjectCoroutine()
    {
        elapsedTime = 0f; // The time elapsed since the movement started.

        // Generate a random delay between 5 and 120 seconds
        randomDuration = Random.Range(minTimeDuration, maxTimeDuration);

        startPosition = originalPosition;

        // Generate random offsets for X and Y within the specified range.
        randomXOffset = Random.Range(-minXOffset, maxXOffset);
        randomYOffset = Random.Range(-minYOffset, maxYOffset);
        randomZOffset = Random.Range(-minZOffset, maxZOffset);

        // Calculate the modified position for spawning the prefab
        endPosition = originalPosition + new Vector3(randomXOffset, randomYOffset, randomZOffset);

        while (elapsedTime < randomDuration)
        {
       
            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            // Calculate the interpolation factor (0 to 1) based on the elapsed time and duration.
            float t = Mathf.Clamp01(elapsedTime / randomDuration);

            // Use Vector3.Lerp to interpolate between the start and end points.
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // Wait for the next frame.
            yield return null;
        }

        Destroy(gameObject);

    }

}