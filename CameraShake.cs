using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 10.0f; // Duration of the shake effect
    [SerializeField] private float shakeIntensity = 0.5f; // Intensity of the shake effect

    private bool startShake = false;

    private Vector3 originalPosition; // Original position of the camera
    private float shakeTimer; // Timer for tracking shake duration

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if(startShake)
        {
            if (shakeTimer > 0.0f)
            {
                // Generate a random offset within the specified intensity
                Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;

                // Apply the random offset to the camera's position
                transform.localPosition = originalPosition + randomOffset;

                // Decrease the shake timer
                shakeTimer -= Time.deltaTime;
            }
            else
            {
                // Reset the camera position to the original position
                transform.localPosition = originalPosition;

                //Making sure it won't repeat upon retrigger
                startShake = false;

            }

            if(!startShake)
            {
                Portal portal = FindObjectOfType<Portal>();
                if (portal != null)
                {
                    portal.StopEarthquake();
                    startShake = true;

                }
            }
        }
       
    }

    //Is called in the Portal Script's OnTriggerEnter
    public void ShakeCamera()
    {
        shakeTimer = shakeDuration;
        startShake = true;
    }

    public void stopCameraShake()
    {
        shakeTimer = 0.0f;
    }
}