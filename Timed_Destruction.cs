using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timed_Destruction : MonoBehaviour
{
    private float elapsedTime = 0.0f;
    private float duration = 3.0f;
    private float normalizedTime;
    private Vector3 dipStartPosition;
    private Vector3 dipEndPosition;

    private bool playRocksReload;
    private bool deathTransitionIsOver;

    public Rigidbody rb; // Reference to the Rigidbody component of the object.
    public float velocityThreshold = 10f; // The velocity threshold.
    private bool hasCrossedThreshold = false;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the Rigidbody component is assigned.
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        StartCoroutine(timedDestruction());
    }


    private void FixedUpdate()
    {
        //Check if death transition is over
        FirstPersonMovement firstPersonMovement = FindObjectOfType<FirstPersonMovement>();

        // Check if the collision is with the Particle System
        if (firstPersonMovement != null)
        {
            deathTransitionIsOver = firstPersonMovement.deathTansitionRelaoded;

            if(deathTransitionIsOver)
            {
                AkSoundEngine.PostEvent("Stop_Rocks_hitting_floor", gameObject);
            }
           
        }

        //Code for triggering the falling whistle based on velocity

        // Check the magnitude of the velocity (speed).
        float currentVelocity = rb.velocity.magnitude;

        // Check if the velocity exceeds the threshold and if the threshold has not been crossed yet.
        if (currentVelocity > velocityThreshold && !hasCrossedThreshold)
        {

            AkSoundEngine.PostEvent("Play_Stroking_Pillow", gameObject);

            // Set the flag to true to prevent further triggering.
            hasCrossedThreshold = true;
        }
        else if (currentVelocity <= velocityThreshold)
        {
            // Reset the flag when the velocity drops below the threshold.
            hasCrossedThreshold = false;
        }

    }

    private System.Collections.IEnumerator timedDestruction()
    {
        yield return new WaitForSeconds(10.0f);

        dipStartPosition = transform.position;
        dipEndPosition = new Vector3(dipStartPosition.x, dipStartPosition.y - 5.0f, dipStartPosition.z);

        while (elapsedTime < duration)
        {
            // Calculate the current progress of the movement
            normalizedTime = elapsedTime / duration;

            // Update the object's position based on the current progress
            transform.position = Vector3.Lerp(dipStartPosition, dipEndPosition, normalizedTime);

            
            // Increment the elapsed time
            elapsedTime += 1.0f * Time.deltaTime;

            yield return null;

        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the object you want to react to.
        if (collision.gameObject.CompareTag("Gravel"))
        {

            // Trigger the AK event.
            AkSoundEngine.PostEvent("Play_Debree_Ball_Fall_Impact_Low", gameObject);

            if(playRocksReload)
            {

                playRocksReload = false;
                AkSoundEngine.PostEvent("Play_Rocks_hitting_floor", gameObject);
            }
            else
            {
                playRocksReload = true;
            }
           
        }
    }

    
}
