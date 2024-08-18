using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject[] objectsToReset;
    public GameObject prefab;

    private Dictionary<GameObject, Vector3> objectStartPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Quaternion> objectStartRotations = new Dictionary<GameObject, Quaternion>();

    private void Start()
    {

        // Store initial positions and rotations of objects
        foreach (GameObject obj in objectsToReset)
        {
            objectStartPositions[obj] = obj.transform.position;
            objectStartRotations[obj] = obj.transform.rotation;

        }
    }

    //call this in the FirstPersonMovement script, in the death method.
    public void Respawn()
    {

        // Reset objects to their initial positions and rotations
        foreach (GameObject obj in objectsToReset)
        {
            // Check if the object has been destroyed
            if (!obj || !obj.activeSelf)
            {
                // Instantiate a new object at the initial position and rotation based on its prefab
                GameObject newObj = Instantiate(prefab, objectStartPositions[obj], objectStartRotations[obj]);

                // Remove the old object from the dictionaries 
                objectStartPositions.Remove(obj);
                objectStartRotations.Remove(obj);

                // Add the new object to the dictionaries 
                objectStartPositions[newObj] = newObj.transform.position;
                objectStartRotations[newObj] = newObj.transform.rotation;

                // Find the index of the destroyed object in the array and replace it with the new object
                for (int i = 0; i < objectsToReset.Length; i++)
                 {
                     if (objectsToReset[i] == obj)
                     {
                         objectsToReset[i] = newObj;
                         break;
                     }
                 }
            }

            
            else
            {

                // Reset the existing object to its initial position and rotation
                obj.transform.position = objectStartPositions[obj];
                obj.transform.rotation = objectStartRotations[obj];

                // Reactivate any disabled objects
                obj.SetActive(true);
            }
        }
    }
    
}

