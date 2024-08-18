using UnityEngine;

public class Sound_Material_Detector : MonoBehaviour
{
    [Tooltip("The layer(s) to consider for object detection.")]
    public LayerMask detectionLayer;

    public string detectedObjectTag; // Store the detected object's tag.
    private float maxRaycastDistance = 6.0f;

    private bool inWater = false;

    private void Update()
    {
        if (inWater)
        {
            detectedObjectTag = "Water"; // Prioritize "Water" detection when inside the trigger.
            return; // Exit early, no need to continue raycasting.
        }

        // Cast a ray downward to detect objects on the specified layer.
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, maxRaycastDistance, detectionLayer, QueryTriggerInteraction.Ignore);

        // Initialize variables for tracking the closest hit and its tag.
        float closestDistance = float.MaxValue;
        string closestTag = "";

        foreach (RaycastHit hit in hits)
        {
            // Check if the distance of this hit is closer than the previous closest hit.
            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                closestTag = hit.collider.gameObject.tag;
            }
        }

        // Check if the closest tag is not empty.
        if (!string.IsNullOrEmpty(closestTag))
        {

            // Update the detectedObjectTag with the new tag.
            detectedObjectTag = closestTag;

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            inWater = true;
            //Debug.Log("In water");
        }
 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            inWater = false;
           
        }
        
    }


}






