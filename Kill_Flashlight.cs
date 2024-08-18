using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Flashlight : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Scripted Flashlight killswitch
            Flashlight flashlight = FindObjectOfType<Flashlight>();

            if (flashlight != null)
            {
                flashlight.triggerKillSwitchOn();
                AkSoundEngine.PostEvent("Play_Flashlight_Glass_Burst", gameObject);
                Debug.Log("kill switch!");
            }

            Panic_Manager panicManager;

            panicManager = FindObjectOfType<Panic_Manager>();
            panicManager.setAccelerationMultiplier(30.0f);
        }

    }

}