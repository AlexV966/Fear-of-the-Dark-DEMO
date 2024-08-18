using UnityEngine;

public class Player_Sliding : MonoBehaviour
{
    public ParticleSystem dustParticleSlide; //Dust particle system

    public float slidingForce; // Magnitude of the sliding force
    public float additionalForce; // Magnitude of the additional force on the x-axis

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // Adjust the tag according to your player object
        {
            //get player to crouch
            Crouch crouchingScript = FindObjectOfType<Crouch>();

            if (crouchingScript != null)
            {
                crouchingScript.setIsSlidingTrue();
            }

            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody)
            {
                Vector3 slidingDirection = playerRigidbody.velocity.normalized; // Get the direction of player's velocity
                Vector3 slidingForceVector = slidingDirection * slidingForce * Time.fixedDeltaTime;
                playerRigidbody.AddForce(slidingForceVector, ForceMode.VelocityChange);

                Vector3 additionalForceVector = new Vector3(additionalForce, 0f, 0f); // Additional force in the x-axis direction
                playerRigidbody.AddForce(additionalForceVector, ForceMode.Acceleration);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Adjust the tag according to your player object
        {
            dustParticleSlide.Play();
            additionalForce = 220.0f;

            //get player to crouch
            Crouch crouchingScript = FindObjectOfType<Crouch>();

            if (crouchingScript != null)
            {
                crouchingScript.setIsSlidingFalse();
            }
        }
    }
}

