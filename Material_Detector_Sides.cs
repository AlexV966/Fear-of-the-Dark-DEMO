using UnityEngine;

public class Material_Detector_Sides : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with an object tagged as "Stone."
        if (other.CompareTag("Stone"))
        {
            Debug.Log("Side Hit On Wall");

            FirstPersonAudio firstPersonAudio = FindObjectOfType<FirstPersonAudio>();

            if(firstPersonAudio != null)
            {
                firstPersonAudio.playSideHitOnWall();

               
            }

        }
    }
}