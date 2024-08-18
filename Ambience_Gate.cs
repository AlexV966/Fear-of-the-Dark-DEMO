using UnityEngine;

public class Ambience_Gate : MonoBehaviour
{
    // Tag of the player GameObject
    public string playerTag = "Player";
    private bool inCollider = false;

    [SerializeField]
    private AK.Wwise.Event startAmbienceEvent;
    [SerializeField]
    private AK.Wwise.Event stopAmbienceEvent;
    [SerializeField]
    private AK.Wwise.RTPC rtpcReverbTime;

    [SerializeField]
    private float reverbTimeEntry;
    [SerializeField]
    private float reverbTimeExit;


    // Index of the child objects to check
    public int firstChildIndex = 0;
    public int secondChildIndex = 1;
    public int thirdChildIndex = 2;
    public int fourthChildIndex = 3;

    private bool firstChildTriggered = false;
    private bool secondChildTriggered = false;
    private bool thirdChildTriggered = false;
    private bool fourthChildTriggered = false;

    public GameObject generalRoomTone;

    private void Start()
    {
        rtpcReverbTime.SetGlobalValue(reverbTimeExit);
    }

    private void startAmbience()
    {
        if (!secondChildTriggered || !fourthChildTriggered)
        {
            startAmbienceEvent.Post(gameObject);
            secondChildTriggered = true;
            fourthChildTriggered = true;
            firstChildTriggered = false;
            thirdChildTriggered = false;

            if (generalRoomTone.activeSelf)
            {
                generalRoomTone.SetActive(false);
            }
        }
        
    }

    private void stopAmbience()
    {
        if(!firstChildTriggered || !thirdChildTriggered)
        {
            stopAmbienceEvent.Post(gameObject);
            firstChildTriggered = true;
            thirdChildTriggered = true;
            secondChildTriggered = false;
            fourthChildTriggered = false;

            if(!generalRoomTone.activeSelf)
            {
                generalRoomTone.SetActive(true);
            }
        }
        
    }

    // Check if a specific child object is colliding with the player
    private bool IsChildColliding(int childIndex, string playerTag)
    {
        if (childIndex >= 0 && childIndex < transform.childCount)
        {
            Transform child = transform.GetChild(childIndex);
            Collider childCollider = child.GetComponent<Collider>();

            if (childCollider != null)
            {
                return childCollider.bounds.Intersects(GameObject.FindGameObjectWithTag(playerTag).GetComponent<Collider>().bounds);
            }
        }

        return false;
    }

    private void Update()
    {
        // Check if the first child object is colliding with the player
        if (IsChildColliding(firstChildIndex, playerTag))
        {
            stopAmbience();

            // Set the RTPC value in Wwise
            rtpcReverbTime.SetGlobalValue(reverbTimeExit);
        }

        // Check if the third child object is colliding with the player
        if (IsChildColliding(thirdChildIndex, playerTag))
        {
            stopAmbience();

            // Set the RTPC value in Wwise
            rtpcReverbTime.SetGlobalValue(reverbTimeExit);
        }

        // Check if the second child object is colliding with the player
        if (IsChildColliding(secondChildIndex, playerTag))
        {
            startAmbience();

            // Set the RTPC value in Wwise
            rtpcReverbTime.SetGlobalValue(reverbTimeEntry);
        }

        // Check if the fourth child object is colliding with the player
        if (IsChildColliding(fourthChildIndex, playerTag))
        {
            startAmbience();

            // Set the RTPC value in Wwise
            rtpcReverbTime.SetGlobalValue(reverbTimeEntry);
        }

    }
}