using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    public bool flashlightStatus = false;
    private bool soundActive;
    private float flickerDelay;

    //scripted light kill
    public bool killSwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        FlashlightLight.gameObject.SetActive(flashlightStatus);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && !killSwitch)
        {
            AkSoundEngine.PostEvent("Play_Flashlight_On_Off", gameObject);

            if (!flashlightStatus)
            {
                flashlightStatus = true;
                FlashlightLight.gameObject.SetActive(flashlightStatus);
            }

            else 
            {
                flashlightStatus = false;
                FlashlightLight.gameObject.SetActive(flashlightStatus);
            }
        }

        if(flashlightStatus == true)
        {
            // Perform the raycast
            RaycastHit hit;

            // Use the camera's position as the ray's origin
            Vector3 rayOrigin = Camera.main.transform.position;

            // Use the camera's forward direction as the ray's direction
            Vector3 rayDirection = Camera.main.transform.forward;

            Ray ray = new Ray(rayOrigin, rayDirection); // Shoots the ray forward from the Camera's perspective.

            if (Physics.Raycast(ray, out hit, 15.0f))
            {
                if (hit.collider.CompareTag("Audio_Shard"))
                {
                    Debug.Log("Hit Audio_Shard");

                    Audio_Shard currentAudioShard = hit.collider.GetComponent<Audio_Shard>();
                    if (currentAudioShard != null)
                    {
                        currentAudioShard.HitByRaycast();

                    }

                }

                if (hit.collider.CompareTag("Radio_Audio"))
                {
                    
                    Radio_Audio radioAudioObject = hit.collider.GetComponent<Radio_Audio>();
                    if (radioAudioObject != null)
                    {
                        radioAudioObject.HitByRaycast();
                    }

                }
                
            }

            // Visualize the reflected ray (optional)
            Debug.DrawRay(rayOrigin, rayDirection * 15.0f, Color.green);
        }
        
    }

    public void MonsterCall(bool _soundActive)
    {
        if(!killSwitch)
        {
            //When a monster sound or call is heard, during it's sustain period, it will create a flicker in the flashlight
            soundActive = _soundActive;
            StartCoroutine(FlashlightFlicker());
        }
        
    }

    private System.Collections.IEnumerator FlashlightFlicker()
    {
        while(soundActive)
        {
            flickerDelay = Random.Range(0.05f, 0.1f);
            flashlightStatus = true;
            FlashlightLight.gameObject.SetActive(flashlightStatus);
            yield return new WaitForSeconds(flickerDelay);
            flashlightStatus = false;
            FlashlightLight.gameObject.SetActive(flashlightStatus);

            yield return null;
        }
    }

    public void triggerKillSwitchOn()
    {
        killSwitch = true;
        flashlightStatus = false;
        FlashlightLight.gameObject.SetActive(flashlightStatus);
    }


}
