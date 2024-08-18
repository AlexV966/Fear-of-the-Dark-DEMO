using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big_Rock_Disable_RG : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private float disableDelay = 10.0f; // Set the delay in seconds.

    private void Start()
    {
        // Get the Rigidbody component attached to the GameObject.
        rb = GetComponent<Rigidbody>();

        // Start the coroutine to disable the Rigidbody after the specified delay.
        StartCoroutine(DisableRigidbodyDelayed());

        AkSoundEngine.PostEvent("Play_Stone_platform_crumble", gameObject);
    }

    private IEnumerator DisableRigidbodyDelayed()
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(disableDelay);

        // Check if the Rigidbody component exists and is not already disabled.
        if (rb != null && rb.gameObject.activeSelf)
        {
            // Disable the Rigidbody component.
            rb.isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the object you want to react to.
        if (collision.gameObject.CompareTag("Stone"))
        {
            // Trigger the AK event.
            AkSoundEngine.PostEvent("Play_Debree_Ball_Fall_Impact_Low", gameObject);

        }
    }

    public void resetBigRock()
    {
        // Destroy the GameObject.
        Destroy(gameObject);
    }
}
