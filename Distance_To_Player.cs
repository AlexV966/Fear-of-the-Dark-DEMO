using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance_To_Player : MonoBehaviour
{
    // Reference to the player
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform origin;

    // RTPC name in Wwise
    [SerializeField]
    private AK.Wwise.RTPC rtpcEvent; 

    // Maximum distance before RTPC is capped
    public float maxDistance = 100f;

    private float rtpcValue;


    // Update is called once per frame
    void Update()
    {
        // Check if the player reference is not null
        if (player != null)
        {
            // Calculate the distance between this object and the player
            float distance = Vector3.Distance(origin.position, player.position);

            // Cap the distance at maxDistance
            distance = Mathf.Min(distance, maxDistance);

            // Normalize the distance between 0 and 1
            float normalizedDistance = distance / maxDistance;

            // Calculate RTPC value (assuming RTPC ranges from 0 to 100)
            rtpcValue = normalizedDistance * 100f;

            // Cap the RTPC value at 100
            rtpcValue = Mathf.Min(rtpcValue, 100.0f);

            //Debug.Log("Distance to player: " + distance);
            //Debug.Log("RTPC value: " + rtpcValue);

            // Set the RTPC value in Wwise
            rtpcEvent.SetValue(gameObject, rtpcValue);
        }
        else
        {
            // If the player reference is null, print an error message
            Debug.LogError("Player reference not set in the inspector!");
        }
    }
}