using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Portal : MonoBehaviour
{
    public Transform destination; // The destination where the player will be transported

    public Image image;
    private bool isFading = false; // Flag to track if fading is in progress
    private float elapsedTimeOne = 0.0f;
    private float DurationOne = 5.0f;
    private float elapsedTimeTwo = 0.0f;
    private float DurationTwo = 5.0f;

    float startAlpha;

    private void Start()
    {
        startAlpha = image.color.a;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            isFading = true;
            StartCoroutine(TransitionToDestination(other.transform));

            //Start camera shake earthquake effect
            
            CameraShake earthquake = FindObjectOfType<CameraShake>();

            if (earthquake != null)
            {
                earthquake.ShakeCamera();
            }
        }
    }

    public void StopEarthquake()
    {
        AkSoundEngine.PostEvent("Stop_Earthquake", gameObject);
    }

    private System.Collections.IEnumerator TransitionToDestination(Transform player)
    {
        //stop player movement
        FirstPersonMovement playerController = FindObjectOfType<FirstPersonMovement>();
        if (playerController != null)
        {
            playerController.DisableScript(); // Disable the script on the player
        }

        AkSoundEngine.PostEvent("Play_Earthquake", gameObject);
        StartCoroutine(EarthquakeExtras());


        //Fade to Black over 5 seconds

        while (elapsedTimeOne < DurationOne)
        {
            float progressOne = elapsedTimeOne / DurationOne;
            float alpha = Mathf.Lerp(startAlpha, 1.0f, progressOne);

            Color imageColor = image.color;
            imageColor.a = alpha;
            image.color = imageColor;

            elapsedTimeOne += 1.0f * Time.deltaTime;
            yield return null;
        }

        AkSoundEngine.PostEvent("Play_Stone_platform_crumble", gameObject);

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);

        // Teleport the player to the destination
        player.position = destination.position;

        startAlpha = image.color.a;

        //start player movement
        if (playerController != null)
        {
            playerController.EnableScript(); // Disable the script on the player
        }

        //Fade back over  5 seconds

        while (elapsedTimeTwo < DurationTwo)
        {
            float progressTwo = elapsedTimeTwo / DurationTwo;
            float alpha = Mathf.Lerp(startAlpha, 0.0f, progressTwo);

            Color imageColor = image.color;
            imageColor.a = alpha;
            image.color = imageColor;

            elapsedTimeTwo += 1.0f * Time.deltaTime;
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);

        //TEST
        // Reset elapsed times for potential future triggers
        elapsedTimeOne = 0.0f;
        elapsedTimeTwo = 0.0f;

        isFading = false;

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



}
