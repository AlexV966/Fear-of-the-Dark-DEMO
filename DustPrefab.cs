using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustPrefab : MonoBehaviour
{
    public float duration = 30.0f; // Set the duration after which the GameObject should be destroyed.

    private ParticleSystem particleSystem;
    private float timer = 0.0f;

    private void Start()
    {
        // Find the ParticleSystem component as a child of this GameObject.
        particleSystem = GetComponentInChildren<ParticleSystem>();

        // Start the ParticleSystem.
        particleSystem.Play();

        AkSoundEngine.PostEvent("Play_Crack_Gas_Release", gameObject);

    }

    private void Update()
    {

        // Increment the timer.
        timer += Time.deltaTime;

        // Check if the set duration has passed.
        if (timer >= duration)
        {
           
            particleSystem.Stop();

            resetDustPrefab();
        }
    }

    public void resetDustPrefab()
    {
        // Destroy the GameObject.
        Destroy(gameObject);
    }
}