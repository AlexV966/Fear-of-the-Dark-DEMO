using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebreeSpawner : MonoBehaviour
{
    public GameObject ballPrefab; // Prefab of the ball object to spawn

    public ParticleSystem dustParticleSystem; //Dust particle system

    private int ballsSpawned = 0; // Counter for the spawned balls
    private bool isSpawning = false;
    float delay;

    public void SpawnCoroutine()
    {
        if(!isSpawning)
        {
            StartCoroutine(StartDelayedSpawning());
        }
        
    }
    

    private System.Collections.IEnumerator StartDelayedSpawning()
    {
            isSpawning = true;

            dustParticleSystem.Play(); // Start the particle system

            // Spawn 50 balls
            for (ballsSpawned = 0; ballsSpawned < 50; ballsSpawned++)
            {
                // Generate a random position within the specified range
                Vector3 randomPosition = new Vector3(Random.Range(2796, 2799), -5.0f, Random.Range(-2992, -2999)); 

                // Instantiate a ball at the spawn point
                Instantiate(ballPrefab, randomPosition, Quaternion.identity);

                delay = Random.Range(0.05f, 0.2f);
                yield return new WaitForSeconds(delay);

            }

            yield return new WaitForSeconds(20.0f);

            dustParticleSystem.Stop(); // Stop the particle system
        
    }

}
