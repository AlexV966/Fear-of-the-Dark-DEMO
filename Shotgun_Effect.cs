using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_Effect : MonoBehaviour
{
    public string cylinderTag = "Shotgun_Cylinder";
    public string volumeRTPC = "OrientationVolumeRTPC";
    public float maxDistance = 15.0f; // Adjust this based on your scene and preferences

    private bool isColliding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(cylinderTag))
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(cylinderTag))
        {
            isColliding = false;
            //AkSoundEngine.SetRTPCValue(volumeRTPC, 100); // Set RTPC to 100 when out of bounds
        }
    }

    void Update()
    {
        //if (isColliding)
        //{
            float distance = CalculateDistanceToCenter(transform.position);

            // Map the distance to an RTPC value between 0 and 100
            float mappedRTPCValue = Mathf.InverseLerp(0f, maxDistance, distance) * 100f;

            // Set the RTPC value
            AkSoundEngine.SetRTPCValue(volumeRTPC, Mathf.Clamp(mappedRTPCValue, 0, 100));


            Debug.Log("RTPC-Shotgun" + Mathf.Clamp(mappedRTPCValue, 0, 100));
        //}
    }

    float CalculateDistanceToCenter(Vector3 position)
    {
        Collider cylinderCollider = GameObject.FindGameObjectWithTag(cylinderTag).GetComponent<Collider>();

        // Calculate the local position of the emitter within the bounds of the cylinder
        Vector3 localPosition = cylinderCollider.transform.InverseTransformPoint(position);

        // Calculate the distance to the center of the cylinder on the X and Z axes
        float distanceX = Mathf.Abs(localPosition.x);
        float distanceZ = Mathf.Abs(localPosition.z);

        // Use Pythagorean theorem to calculate the distance in 2D (X and Z)
        return Mathf.Sqrt(distanceX * distanceX + distanceZ * distanceZ);
    }
}

