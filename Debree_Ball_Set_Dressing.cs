using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debree_Ball_Set_Dressing : MonoBehaviour
{

    private float elapsedTime = 0.0f;
    private float duration = 3.0f;
    private float normalizedTime;
    private Vector3 dipStartPosition;
    private Vector3 dipEndPosition;
    public bool deSpawnAllowed = true;


    private void Start()
    {
        if(deSpawnAllowed)
        {
            StartCoroutine(timedDestruction());
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the object you want to react to.
        if (collision.gameObject.CompareTag("Gravel"))
        {
            // Trigger the AK event.
            AkSoundEngine.PostEvent("Play_Debree_Ball_Fall_Impact_Medium_01", gameObject);
            AkSoundEngine.PostEvent("Play_Player_Jump_Impact_Gravel", gameObject);
            
        }

        // Check if the collision involves the object you want to react to.
        if (collision.gameObject.CompareTag("Stone"))
        {

            // Trigger the AK event.
            AkSoundEngine.PostEvent("Play_Debree_Ball_Fall_Impact_Medium_01", gameObject);

        }

        // Check if the collision involves the object you want to react to.
        if (collision.gameObject.CompareTag("DebreeBall"))
        {
            // Trigger the AK event.
            AkSoundEngine.PostEvent("Play_Debree_Ball_Fall_Impact_Medium_01", gameObject);

        }

        // Check if the collision involves the object you want to react to.
        if (collision.gameObject.CompareTag("Semi_Gravel"))
        {
            // Trigger the AK event.
            AkSoundEngine.PostEvent("Play_Debree_Ball_Fall_Impact_Medium_01", gameObject);
            AkSoundEngine.PostEvent("Play_Player_Jump_Impact_Gravel", gameObject);
        }

        // Check if the collision involves the object you want to react to.
        if (collision.gameObject.CompareTag("Plank"))
        {
            // Trigger the AK event.
            AkSoundEngine.PostEvent("Play_Player_Jump_Impact_Wood", gameObject);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object has the "Water" tag.
        if (other.CompareTag("Water"))
        {
            // Trigger the AK event.
            AkSoundEngine.PostEvent("Play_Stones_hit_water", gameObject);
            AkSoundEngine.PostEvent("Play_Water_Sizzle_Sequence_fade", gameObject);
        }
    }

    private System.Collections.IEnumerator timedDestruction()
    {
        yield return new WaitForSeconds(30.0f);

        dipStartPosition = transform.position;
        dipEndPosition = new Vector3(dipStartPosition.x, dipStartPosition.y - 5.0f, dipStartPosition.z);

        while (elapsedTime < duration)
        {
            // Calculate the current progress of the movement
            normalizedTime = elapsedTime / duration;

            // Update the object's position based on the current progress
            transform.position = Vector3.Lerp(dipStartPosition, dipEndPosition, normalizedTime);


            // Increment the elapsed time
            elapsedTime += 1.0f * Time.deltaTime;

            yield return null;
        }

        //Respawn all objects in the Scene
        Ceiling_Emitter ceilingEmitter = FindObjectOfType<Ceiling_Emitter>();

        if (ceilingEmitter != null)
        {
            ceilingEmitter.DecrementSpawnedPrefabCount();
        }

        Destroy(gameObject);
    }
}
