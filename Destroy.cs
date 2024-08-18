using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject shatteredObjectPrefab; // Prefab of the shattered version
    public float explosionForce = 10f; // Force applied to the shattered bits
    public float explosionRadius = 1f; // Radius of the explosion


    // This method is called when another collider enters this object's collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            AkSoundEngine.PostEvent("Play_Stone_platform_crumble", gameObject);

            //Instantiate the shards and spread them
            ShardsShatter();
            
                // Destroy this object
                Destroy(gameObject);
        }
    }

    private void ShardsShatter()
    {

        if (shatteredObjectPrefab != null)
        {
            // Instantiate the shattered object at the same position and rotation
            GameObject shatteredObject = Instantiate(shatteredObjectPrefab, transform.position, transform.rotation);

            // Get the rigidbody of the shattered object
            Rigidbody[] rigidbodies = shatteredObject.GetComponentsInChildren<Rigidbody>();

            // Apply an explosion force to each shattered bit
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.AddExplosionForce(explosionForce, shatteredObject.transform.position, explosionRadius);
            }

        }
        
    }
}
