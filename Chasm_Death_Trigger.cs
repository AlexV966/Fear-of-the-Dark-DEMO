using UnityEngine;

public class Chasm_Death_Trigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            FirstPersonMovement player = FindObjectOfType<FirstPersonMovement>();

            if (player != null)
            {
                player.triggerDeath();
            }
        }
    }
}
