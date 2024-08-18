using UnityEngine;

public class ScriptedFall : MonoBehaviour
{
    private bool isExecuting = false;

    public ParticleSystem dirtParticleSystem; //Dirt particle system

    //Pre-Timer co-routine
    private float delay;
    private Vector3 dipStartPosition = new Vector3(-127.0f, -6.0f, -81.0f); 
    private Vector3 dipEndPosition = new Vector3(-127.0f, -6.5f, -81.0f);
    private Quaternion rotateStartPosition = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    private Quaternion rotateEndPosition = new Quaternion(0.2f, 0.0f, 0.0f, 1.0f);
    Quaternion coreRotation = Quaternion.Euler(0.0f, 0.0f, 1.0f);
    private float durationPre = 0.05f;  
    private float elapsedTimePre = 0.0f; 
    float normalizedTimePre;

    //Initial timer
    private bool startTimer = false;
    private bool startMovement = false;
    private float duration; // The duration of the timer in seconds
    private float elapsedTime = 0.0f; // The time elapsed since the timer started


    //Movement over time
    private Vector3 startPosition = new Vector3 (-127.0f, -6.5f, -81.0f);
    private Vector3 endPosition = new Vector3(-127.0f, -26.68f, -81.0f);

    private float durationTimerTwo = 0.5f; // The duration of the movement in seconds
    private float elapsedTimeTimerTwo = 0.0f; //The time elapsed since the movememt started
    float normalizedTime;

    private void Start()
    {
        duration = Random.Range(10.0f, 20.0f);

    }

    public void CheckDetection()
    {
        if (!isExecuting)
        {
            startTimer = true;

            //Start coroutine to rotate and dip
            StartCoroutine(DipANDRotate());

            //makes sure it will only fire once
            isExecuting = true;
        }
    }

    void Update()
    {
        if (startTimer)
        {
            // Increment the elapsed time by the time since the last frame
            elapsedTime += 1.0f * Time.deltaTime;

            // Check if the timer has reached the duration
            if (elapsedTime >= duration)
            {
                // Timer has finished
                TimerFinished();

            }

        }

        //================================

        if (startMovement)
        {
             // Calculate the current progress of the movement
             normalizedTime = elapsedTimeTimerTwo / durationTimerTwo;
               
             // Update the object's position based on the current progress
             transform.position = Vector3.Lerp(startPosition, endPosition, normalizedTime);
                
             // Increment the elapsed time
             elapsedTimeTimerTwo += 1.0f * Time.deltaTime;

            

            if(elapsedTimeTimerTwo >= durationTimerTwo)
            {
                SecureDestination();
                startMovement = false;
                // Trigger the AK event.
                AkSoundEngine.PostEvent("Play_Water_high_impact", gameObject);
            }
        }


    }

    private System.Collections.IEnumerator DipANDRotate()
    {
        delay = Random.Range(3.0f, 5.0f);
        yield return new WaitForSeconds(delay);

        //============================

        //DIP and ROTATE

        while (elapsedTimePre < durationPre)
        {
            // Calculate the current progress of the movement
            normalizedTimePre = elapsedTimePre / durationPre;

            // Apply the core rotation to the end rotation
            Quaternion rotatedEndRotation = rotateEndPosition * coreRotation;

            // Update the object's position based on the current progress
            transform.position = Vector3.Lerp(dipStartPosition, dipEndPosition, normalizedTimePre);

            // Update the object's position based on the current progress
            transform.rotation = Quaternion.Slerp(rotateStartPosition, rotatedEndRotation, normalizedTimePre);

            // Increment the elapsed time
            elapsedTimePre += 1.0f * Time.deltaTime;

            yield return null;

        }

        dirtParticleSystem.Play(); // Start the particle system

        // Ensure the final position and rotation are set correctly
        transform.position = dipEndPosition;
        transform.rotation = rotateEndPosition;

    }

    private void TimerFinished()
    {
        startTimer = false;
        startMovement = true;
        dirtParticleSystem.Stop(); // Stop the particle system

    }

    private void SecureDestination()
    {
        // Ensure the object reaches the final position exactly
        transform.position = endPosition;
    }

}
