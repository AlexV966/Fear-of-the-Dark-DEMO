using System.Collections;
using UnityEngine;

public class Radio_Audio : MonoBehaviour
{
    public GameObject radioObject;
    Transform originalParent;
    private Vector3 destination;
    private bool isMoving = false;
    private bool hasReachedDestination = false;
    private bool audioObjectWithPlayer = true;


    public float duration = 10.0f;

    private Panic_Manager panicManager;

    private void Start()
    {
        // Store the current parent
        originalParent = transform.parent;

        // Initialize the destination with the current position
        destination = radioObject.transform.position;

        AkSoundEngine.SetState("Radio_Moving", "Normal");

        panicManager = FindObjectOfType<Panic_Manager>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Radio_Audio_Trigger") && !isMoving)
        {

            // Check if the other object has a child
            if (other.transform.childCount > 0)
            {
                // Accessing the first child's position
                Vector3 childPosition = other.transform.GetChild(0).position;
                SetPositionB(childPosition);

            }

            if(audioObjectWithPlayer)
            {
                StartRadioAudioMovement();
                Destroy(other.gameObject);

                audioObjectWithPlayer = false;
            }


        }

        if (other.CompareTag("Player"))
        {

            if (!isMoving && hasReachedDestination)
            {
                // Reparent the object
                transform.parent = originalParent;

                // Set the gameObject's position back to the radioPosition
                transform.position = transform.parent.position;

                // Reset the destination reached to false
                hasReachedDestination = false;

                AkSoundEngine.SetState("Radio_Moving", "Normal");

                audioObjectWithPlayer = true;

                //Reward the player with a drop in panic
                panicManager.DecreasePanic(80.0f);

                AkSoundEngine.PostEvent("Stop_Pre_Transmission_and_Cuts_Last_Section", gameObject);
                AkSoundEngine.PostEvent("Play_Flares", gameObject);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Radio_Audio_Trigger") && !isMoving)
        {

            // Check if the other object has a child
            if (other.transform.childCount > 0)
            {
                // Accessing the first child's position
                Vector3 childPosition = other.transform.GetChild(0).position;
                SetPositionB(childPosition);

            }

            if (audioObjectWithPlayer)
            {
                StartRadioAudioMovement();
                Destroy(other.gameObject);

                audioObjectWithPlayer = false;
            }


        }
    }

    // Trigger movement coroutine
    public void StartRadioAudioMovement()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveOverTime());
        }
    }

    // Set function to set the destination position
    public void SetPositionB(Vector3 newPosition)
    {
        destination = newPosition;

    }

    // Coroutine to move the gameObject to the destination position smoothly
    IEnumerator MoveOverTime()
    {
        isMoving = true;
        AkSoundEngine.SetState("Radio_Moving", "Unnatural");
        float elapsedTime = 0f;

        // Unparent the object
        transform.parent = null;

        // Convert starting position to local coordinates
        Vector3 startingPos = transform.position;

        while (elapsedTime < duration) // Move over time
        {
            // Calculate the position to move to using Lerp
            Vector3 newPosition = Vector3.Lerp(startingPos, destination, (elapsedTime / duration));

            // Update the position of the gameObject
            transform.position = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isMoving = false;
        hasReachedDestination = true;
        AkSoundEngine.PostEvent("Play_Pre_Transmission_and_Cuts_Last_Section", gameObject);

    }


    public void HitByRaycast()
    {
        if (!isMoving && hasReachedDestination)
        {
            // Reparent the object
            transform.parent = originalParent;

            // Set the gameObject's position back to the radioPosition
            transform.position = transform.parent.position;

            // Reset the destination reached to false
            hasReachedDestination = false;

            AkSoundEngine.SetState("Radio_Moving", "Normal");

            audioObjectWithPlayer = true;

        }
    }
}

