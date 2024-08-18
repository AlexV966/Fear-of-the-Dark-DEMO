using UnityEngine;

public class Cave_In : MonoBehaviour
{
    private bool playerDetected = false;

    public GameObject debreePrefab; //Prefab of the debree object to spawn
    private int debreeSpawned;
    private float fallDelay;

    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            StartCoroutine(ceilingCollapse());
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }
    }

private System.Collections.IEnumerator ceilingCollapse()
    {
        playerDetected = false;

        AkSoundEngine.PostEvent("Play_Rocks_hitting_floor", gameObject);

        // Spawn 150 debree bits
        for (debreeSpawned = 0; debreeSpawned < 150; debreeSpawned++)
        {
            // Generate a random position within the specified range
            Vector3 randomPosition = new Vector3(Random.Range(transform.position.x - 4.0f, transform.position.x + 4.0f), -16.0f, Random.Range(transform.position.z + 5.0f, transform.position.z - 5.0f));

            // Instantiate a ball at the spawn point
            Instantiate(debreePrefab, randomPosition, Quaternion.identity);

            fallDelay = Random.Range(0.001f, 0.02f);
            yield return new WaitForSeconds(fallDelay);

        }

        FirstPersonMovement playerController = FindObjectOfType<FirstPersonMovement>();
        if (playerController != null)
        {
            playerController.triggerDeath(); // Triggers death and transition
        }

    }

    

    

}