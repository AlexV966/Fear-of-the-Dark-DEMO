using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_Radio : MonoBehaviour
{

    private FirstPersonMovement firstPersonMovement;

    private GroundCheck groundCheck;

    private Crouch crouch;

    // Check if the player is currently grounded.
    private bool isGrounded;

    //Check if player is currently crouched
    private bool isCrouched;

    //Check if player is swimming
    private bool swimming;

    private bool radioStatus = false;

    private Vector3 originalPosition;
    private Vector3 targetPosition = new Vector3(0.08f, 1.35f, 0.27f);

    public GameObject walkieTalkie;

    public GameObject leftDial;

    public GameObject rightDial;

    // The rotation speed variable.
    public float rotationSpeed = 30f; // Adjust the speed as needed.

    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;

    private bool isRotatingLeftRD = false;
    private bool isRotatingRightRD = false;

    private float leftDialRTPC;
    private float rightDialRTPC;

    private float goalPostLeftDial;
    private float goalPostRightDial;

    public float finalRTPCLeft;
    public float finalRTPCRight;

    //Checkpoint event
    private string currentEvent;

    //Shatter instantiate
    public GameObject shatterPrefab;
    public int numberOfObjects = 3;
    public float circleRadius = 5.0f;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    public GameObject gameObjectRadioAudio;
    private BoxCollider radioAudioCollider;

    private bool alive;

    private bool signalFeasibleRTPCLeft = false;
    private bool signalFeasibleRTPCRight = false;

    private string stateChange;

    private string radioStatusOne;
    private string radioStatusTwo;
    private string radioBingoMessageOne;
    private string radioBingoMessageTwo;
    private bool bingoMessagesActive = false;

    private string signalDetected;

    private void Start()
    {
        originalPosition = transform.localPosition;

        // Attempt to find the GroundCheck component on the same gameObject.
        groundCheck = FindObjectOfType<GroundCheck>();

        // Attempt to find the Crouch component in the parent GameObject hierarchy.
        crouch = GetComponentInParent<Crouch>();

        // Attempt to find the FirstPersonMovement component in the parent GameObject hierarchy.
        firstPersonMovement = GetComponentInParent<FirstPersonMovement>(); 

        // Check if the components were found.
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck component not found.");
        }

        if (firstPersonMovement == null)
        {
            Debug.LogError("firstPersonMovement component not found.");
        }

        if (crouch == null)
        {
            Debug.LogError("Crouch component not found in parent GameObject hierarchy.");
        }

   
  
        // Get the Mesh Collider component on the child object
        radioAudioCollider = gameObjectRadioAudio.GetComponent<BoxCollider>();

        // Disable the Mesh Collider for the radio audio object by default
        radioAudioCollider.enabled = false;


        goalPostLeftDial = setRTPCDialGoalPost(70);
        goalPostRightDial = setRTPCDialGoalPost(30);

    }

    void Update()
    {

        // Check if the player is currently grounded.
        isGrounded = groundCheck.isGrounded;

        //Check if player is currently crouched
        isCrouched = crouch.IsCrouched;

        //Check if player is swimming
        swimming = firstPersonMovement.isSwimming;

        //Check if player is dying
        alive = firstPersonMovement.deathTansitionRelaoded;

        if(swimming || !alive)
        {
            DestroyAllObjects();
        }

        ///Bring up the radio, engage/disengage

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!radioStatus)
            {
                StartCoroutine(MoveRadio(targetPosition, 0.3f));
                radioStatus = true;

                stateChange = "On";

                // Set the Wwise state
                AkSoundEngine.SetState("Radio_Status", stateChange);
            }
           else if(radioStatus)
            {
                StopCoroutine("MoveRadio");
                StartCoroutine(MoveRadio(originalPosition, 0.3f));
                radioStatus = false;

                stateChange = "Off";
                AkSoundEngine.SetState("Radio_Status", stateChange);

            }
        }

        if (!isGrounded || isCrouched && swimming)
        {
            StopCoroutine("MoveRadio");
            StartCoroutine(MoveRadio(originalPosition, 0.3f));
            radioStatus = false;

            stateChange = "Off";
            AkSoundEngine.SetState("Radio_Status", stateChange);

        }

        ///Rotate the left dial
       
        // Check for "<" key press to start rotating left.
        if (Input.GetKey(KeyCode.Comma) && !isRotatingRight)
        {
            isRotatingLeft = true;
        }
        // Check for ">" key press to start rotating right.
        else if (Input.GetKey(KeyCode.Period) && !isRotatingLeft)
        {
            isRotatingRight = true;
        }

        // Check for key release to stop rotating.
        if (Input.GetKeyUp(KeyCode.Comma))
        {
            isRotatingLeft = false;
        }

        if (Input.GetKeyUp(KeyCode.Period))
        {
            isRotatingRight = false;
        }

        // Rotate the left dial based on the key inputs.
        if (isRotatingLeft)
        {

            float newRotation = leftDial.transform.localEulerAngles.z - rotationSpeed * Time.deltaTime;
            newRotation = Mathf.Clamp(newRotation, 0f, 100f);
            leftDial.transform.localEulerAngles = new Vector3(0, 0, newRotation);


        }
        else if (isRotatingRight)
        {
            float newRotation = leftDial.transform.localEulerAngles.z + rotationSpeed * Time.deltaTime;
            newRotation = Mathf.Clamp(newRotation, 0f, 100f);
            leftDial.transform.localEulerAngles = new Vector3(0, 0, newRotation);

        }

        leftDialRTPC = leftDial.transform.localEulerAngles.z;

        finalRTPCLeft = calculateRealRTPC(leftDialRTPC, goalPostLeftDial);

        // Connect Wwise RTPC here using the finalRTPCLeft
        AkSoundEngine.SetRTPCValue("Left_Knob", finalRTPCLeft, gameObjectRadioAudio);
        //Debug.Log("RTPCLeft: " + finalRTPCLeft);

        if(finalRTPCLeft <= 10.0f)
        {
            signalFeasibleRTPCLeft = true;
            
        }
        else
        {
            signalFeasibleRTPCLeft = false;
        }

        //Determine what the GUI message will be to let the player know they got it right
        if(signalFeasibleRTPCLeft && bingoMessagesActive)
        {
            radioBingoMessageOne = "Left knob calibrated!";
        }
        else
        {
            radioBingoMessageOne = null;
        }
        

        ///Rotate the right dial

        // Check for "[" key press to start rotating left.
        if (Input.GetKey(KeyCode.LeftBracket) && !isRotatingRightRD)
        {
            isRotatingLeftRD = true;
        }
        // Check for "]" key press to start rotating right.
        else if (Input.GetKey(KeyCode.RightBracket) && !isRotatingLeftRD)
        {
            isRotatingRightRD = true;
        }

        // Check for key release to stop rotating.
        if (Input.GetKeyUp(KeyCode.LeftBracket))
        {
            isRotatingLeftRD = false;
        }

        if (Input.GetKeyUp(KeyCode.RightBracket))
        {
            isRotatingRightRD = false;
        }

        // Rotate the left dial based on the key inputs.
        if (isRotatingLeftRD)
        {

            float newRotation = rightDial.transform.localEulerAngles.z - rotationSpeed * Time.deltaTime;
            newRotation = Mathf.Clamp(newRotation, 0f, 100f);
            rightDial.transform.localEulerAngles = new Vector3(0, 0, newRotation);


        }
        else if (isRotatingRightRD)
        {
            float newRotation = rightDial.transform.localEulerAngles.z + rotationSpeed * Time.deltaTime;
            newRotation = Mathf.Clamp(newRotation, 0f, 100f);
            rightDial.transform.localEulerAngles = new Vector3(0, 0, newRotation);

        }

        rightDialRTPC = rightDial.transform.localEulerAngles.z;

        finalRTPCRight = calculateRealRTPC(rightDialRTPC, goalPostRightDial);

        // Connect Wwise RTPC here using the finalRTPCRight
        AkSoundEngine.SetRTPCValue("Right_Knob", finalRTPCRight, gameObjectRadioAudio);

        if (finalRTPCRight <= 10.0f)
        {
            signalFeasibleRTPCRight = true;
        }
        else
        {
            signalFeasibleRTPCRight = false;
        }

        //Determine what the GUI message will be to let the player know they got it right
        if (signalFeasibleRTPCRight && bingoMessagesActive)
        {
            radioBingoMessageTwo = "Right knob calibrated!";
        }
        else
        {
            radioBingoMessageTwo = null;
        }

    }

    private IEnumerator MoveRadio(Vector3 targetPosition, float duration)
    {
        float startTime = Time.time;
        Vector3 currentPosition = transform.localPosition;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.localPosition = Vector3.Lerp(currentPosition, targetPosition, t);
            yield return null;
        }

        // Ensures the final position is exactly the target position.
        transform.localPosition = targetPosition;

        if(radioStatus)
        {
            walkieTalkie.SetActive(true);

            // Enable the Mesh Collider for the radio audio object
            radioAudioCollider.enabled = true;
        }
        else
        {
            walkieTalkie.SetActive(false);

           // Disable the Mesh Collider for the radio audio object
            radioAudioCollider.enabled = false;
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Reset the goalposts at invisible radio checkpoints
        if (other.CompareTag("Radio_Checkpoint"))
        {
            goalPostLeftDial = setRTPCDialGoalPost(leftDialRTPC);
            goalPostRightDial = setRTPCDialGoalPost(rightDialRTPC);
            AkSoundEngine.PostEvent("Play_Flares", gameObjectRadioAudio);
            Destroy(other.gameObject);

        }

        if (other.CompareTag("Shard_Trigger"))
        {
            SpawnObjectsWithinRadius();
            Destroy(other.gameObject);

        }

        if (other.CompareTag("Radio_Emitter"))
        {
            Destroy(other.gameObject);
        }


        if (other.CompareTag("Radio_Transmission_1"))
        {
            signalDetected = "Signal detected!";
            radioStatusOne = "Radio-Status: ";
            radioStatusTwo = "Radio-Status: ";
            bingoMessagesActive = true;

            if (signalFeasibleRTPCLeft && signalFeasibleRTPCRight)
            {
                signalDetected = null;
                radioStatusOne = null;
                radioStatusTwo = null;
                bingoMessagesActive = false;

                // Extract script component and call a function
                Radio_Transmission scriptComponent = other.GetComponent<Radio_Transmission>();
                if (scriptComponent != null)
                {
                    scriptComponent.stopEvent();
                }

                currentEvent = "Play_Line_1";
                PlayWwiseEvent(currentEvent);
                Destroy(other.gameObject);
            }      
        }
        else if (other.CompareTag("Radio_Transmission_2"))
        {
            signalDetected = "Signal detected!";
            radioStatusOne = "Radio-Status: ";
            radioStatusTwo = "Radio-Status: ";
            bingoMessagesActive = true;

            if (signalFeasibleRTPCLeft && signalFeasibleRTPCRight)
            {
                signalDetected = null;
                radioStatusOne = null;
                radioStatusTwo = null;
                bingoMessagesActive = false;

                // Extract script component and call a function
                Radio_Transmission scriptComponent = other.GetComponent<Radio_Transmission>();
                if (scriptComponent != null)
                {
                    scriptComponent.stopEvent();
                }

                currentEvent = "Play_Line_2";
                PlayWwiseEvent(currentEvent);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Radio_Transmission_3"))
        {
            signalDetected = "Signal detected!";
            radioStatusOne = "Radio-Status: ";
            radioStatusTwo = "Radio-Status: ";
            bingoMessagesActive = true;

            if (signalFeasibleRTPCLeft && signalFeasibleRTPCRight)
            {
                signalDetected = null;
                radioStatusOne = null;
                radioStatusTwo = null;
                bingoMessagesActive = false;

                // Extract script component and call a function
                Radio_Transmission scriptComponent = other.GetComponent<Radio_Transmission>();
                if (scriptComponent != null)
                {
                    scriptComponent.stopEvent();
                }

                currentEvent = "Play_Line_3";
                PlayWwiseEvent(currentEvent);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Radio_Transmission_4"))
        {

            signalDetected = "Signal detected!";
            radioStatusOne = "Radio-Status: ";
            radioStatusTwo = "Radio-Status: ";
            bingoMessagesActive = true;

            if (signalFeasibleRTPCLeft && signalFeasibleRTPCRight)
            {
                signalDetected = null;
                radioStatusOne = null;
                radioStatusTwo = null;
                bingoMessagesActive = false;

                // Extract script component and call a function
                Radio_Transmission scriptComponent = other.GetComponent<Radio_Transmission>();
                if (scriptComponent != null)
                {
                    scriptComponent.stopEvent();
                }

                currentEvent = "Play_Line_4";
                PlayWwiseEvent(currentEvent);
                Destroy(other.gameObject);

            }
        }
        else if (other.CompareTag("Radio_Transmission_5"))
        {
            signalDetected = "Signal detected!";
            radioStatusOne = "Radio-Status: ";
            radioStatusTwo = "Radio-Status: ";
            bingoMessagesActive = true;


            if (signalFeasibleRTPCLeft && signalFeasibleRTPCRight)
            {
                signalDetected = null;
                radioStatusOne = null;
                radioStatusTwo = null;
                bingoMessagesActive = false;

                // Extract script component and call a function
                Radio_Transmission scriptComponent = other.GetComponent<Radio_Transmission>();
                if (scriptComponent != null)
                {
                    scriptComponent.stopEvent();
                }

                currentEvent = "Play_Line_5";
                PlayWwiseEvent(currentEvent);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Radio_Transmission_6"))
        {
            signalDetected = "Signal detected!";
            radioStatusOne = "Radio-Status: ";
            radioStatusTwo = "Radio-Status: ";
            bingoMessagesActive = true;

            if (signalFeasibleRTPCLeft && signalFeasibleRTPCRight)
            {
                signalDetected = null;
                radioStatusOne = null;
                radioStatusTwo = null;
                bingoMessagesActive = false;

                // Extract script component and call a function
                Radio_Transmission scriptComponent = other.GetComponent<Radio_Transmission>();
                if (scriptComponent != null)
                {
                    scriptComponent.stopEvent();
                }

                currentEvent = "Play_Line_6";
                PlayWwiseEvent(currentEvent);
                Destroy(other.gameObject);
            }
        }
        //else
        //{
        //    signalDetected = null;
        //    radioStatusOne = null;
        //    radioStatusTwo = null;
        //    bingoMessagesActive = false;
        //}


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Radio_Transmission_1") || other.CompareTag("Radio_Transmission_2") || other.CompareTag("Radio_Transmission_3") || other.CompareTag("Radio_Transmission_4") || other.CompareTag("Radio_Transmission_5") || other.CompareTag("Radio_Transmission_6"))
        {
            signalDetected = null;
            radioStatusOne = null;
            radioStatusTwo = null;
            bingoMessagesActive = false;
        }
    }

    // Set function for the Wwise event
    public void PlayWwiseEvent(string _eventName)
    {
        // Trigger Wwise event using _eventName
        AkSoundEngine.PostEvent(_eventName, gameObjectRadioAudio);
    }

    //Set a different goalpost for the right dial.
    private float setRTPCDialGoalPost(float _rtpcDial)
    {
        float X;

        float offset = _rtpcDial / Random.Range(3, 6);

        if ((100.0f - _rtpcDial) > (_rtpcDial - 0.0f))
        {
            X = Random.Range(_rtpcDial + offset, 100.0f);
        }
        else
        {
            X = Random.Range(0.0f, _rtpcDial - offset);
        }

        return X;
    }

    private float calculateRealRTPC(float _rtpcDial, float _goalPost)
    {
        float distance;

        float a = 100.0f - _goalPost;
        float b = Mathf.Abs(0.0f - _goalPost);

        if (a >= b)
        {
            distance = a;
        }
        else
        {
            distance = b;
        }

        // Calculate RTPC as the absolute value of (_goalPost - _rtpcDial) scaled between 0.0f and 100.0f
        float RTPC = Mathf.Clamp(Mathf.Abs(_goalPost - _rtpcDial) * (100.0f / distance), 0.0f, 100.0f);

        //Debug.Log("RTPC" + RTPC);

        return RTPC;
    }

    private void SpawnObjectsWithinRadius()
    {
        numberOfObjects = Random.Range(2, 4);

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Generate a random angle within the circle.
            float angle = Random.Range(0f, 360f);

            // Calculate the position based on the angle and radius.
            float x = circleRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = circleRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

            // Instantiate the object at the calculated position.
            Vector3 spawnPosition = transform.position + new Vector3(x, 0f, z);
            GameObject spawnedObject = Instantiate(shatterPrefab, spawnPosition, Quaternion.identity);

            // Add the spawned object to the list.
            spawnedObjects.Add(spawnedObject);
        }
    }

    public void DestroyAllObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear(); // Clear the list after destroying all objects.
    }

    public bool getRadioStatus()
    {
        return radioStatus;
    }

    void OnGUI()
    {
        // Create a GUIStyle
        GUIStyle styleOne = new GUIStyle(GUI.skin.label);

        // Increase the font size
        styleOne.fontSize = 18;

        // Make the text bold (thicker)
        styleOne.fontStyle = FontStyle.Bold;

        GUI.Label(new Rect(10, 50, 300, 50), radioStatusOne + radioBingoMessageOne, styleOne);

        GUI.Label(new Rect(10, 20, 350, 50), radioStatusTwo + radioBingoMessageTwo, styleOne);

        // Calculate the center of the screen
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector2 screenCenter = new Vector2(screenWidth / 2f, screenHeight / 2f);

        // Create a GUIStyle
        GUIStyle styleTwo = new GUIStyle(GUI.skin.label);

        // Increase the font size
        styleTwo.fontSize = 18;

        // Make the text bold (thicker)
        styleTwo.fontStyle = FontStyle.Bold;

        // Set the text color to black
        styleTwo.normal.textColor = Color.black;

        // Calculate the position for the thiird label (centered)
        Rect labelRectThree = new Rect(screenCenter.x - 125, screenCenter.y - 50, 350, 50);
        GUI.Label(labelRectThree, signalDetected, styleTwo);

    }

}





