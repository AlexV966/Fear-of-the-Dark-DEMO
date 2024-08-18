using UnityEngine;

public class Checkpoint_1 : MonoBehaviour
{
    private Transform checkpointLocation;

    private void Start()
    {
        checkpointLocation = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            FirstPersonMovement player = FindObjectOfType<FirstPersonMovement>();

            if (player != null)
            {
                player.setCheckpoint(checkpointLocation);
                Debug.Log("Checkpoint Set");
            }
        }
    }
}
