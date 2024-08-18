using UnityEngine;

public class Emitter_First_Reflections : MonoBehaviour
{
    Vector3 hitPosition;

    private GameObject[] instancesFirst = new GameObject[6]; // Array to store the spawned initial instances
    private GameObject[] instancesReflection = new GameObject[6]; // Array to store the spawned reflection instances
    private int instanceFirst = 0;
    private int instanceReflection = 0;

    public Transform player;
    private Transform rayOrigin;
    public GameObject reflectionPrefab;
    private float rayDistance;

    private float distance;

    private Vector3 hitPoint25Left;
    private Vector3 hitPoint25Right;
    private Vector3 hitPoint50Left;
    private Vector3 hitPoint50Right;
    private Vector3 hitPoint75Left;
    private Vector3 hitPoint75Right;

    private Vector3 point25;
    private Vector3 point50;
    private Vector3 point75;

    private bool alreadyInstantiated = false;

    private string eventName;

    void Start()
    {
        rayOrigin = transform; // Assuming this script is attached to an object in the scene.
    }

    void FixedUpdate()
    {

        sendRayFromEmitterToPlayer();


        // Calculate the distance from the player or obstruction to the ray's origin.
        distance = Vector3.Distance(hitPosition, rayOrigin.position);

        //Debug.Log("Distance value: " + distance);

        // Limit the maximum ray distance to 40 units
        rayDistance = 40.0f;


        // Calculate the points along the ray using Lerp (linear interpolation).
        point25 = Vector3.Lerp(hitPosition, rayOrigin.position, 0.25f);
        point50 = Vector3.Lerp(hitPosition, rayOrigin.position, 0.5f);
        point75 = Vector3.Lerp(hitPosition, rayOrigin.position, 0.75f);

        //Debug.Log("Point 25: " + point25);
        //Debug.Log("Point 50: " + point50);
        //Debug.Log("Point 75: " + point75);

        // Cast rays to the left and right from each of the three points.
        hitPoint25Left = CastRaysLeft(point25);
        hitPoint25Right = CastRaysRight(point25);
        hitPoint50Left = CastRaysLeft(point50);
        hitPoint50Right = CastRaysRight(point50);
        hitPoint75Left = CastRaysLeft(point75);
        hitPoint75Right = CastRaysRight(point75);
        // Now you can use hitPoint25, hitPoint50, and hitPoint75 as needed.

        CastRayToReflectionPoint(hitPoint25Left, "25_Left", transform);
        CastRayToReflectionPoint(hitPoint25Right, "25_Right", transform);
        CastRayToReflectionPoint(hitPoint50Left, "50_Left", transform);
        CastRayToReflectionPoint(hitPoint50Right, "50_Right", transform);
        CastRayToReflectionPoint(hitPoint75Left, "75_Left", transform);
        CastRayToReflectionPoint(hitPoint75Right, "75_Right", transform);
        alreadyInstantiated = true;

        //Debug.Log("Hit_Point_25_Left: " + hitPoint25Left);
        //Debug.Log("Hit_Point_25_Right: " + hitPoint25Right);
       
    }

    Vector3 CastRaysLeft(Vector3 origin)
    {
        Vector3 hitPoint = Vector3.zero;

        // Calculate the direction from the emitter to the player
        Vector3 emitterToPlayerDirection = (player.position - rayOrigin.position).normalized;

        // Calculate the direction for the right ray (rotated 90 degrees clockwise)
        Vector3 leftRayDirection = Quaternion.Euler(0, -90, 0) * emitterToPlayerDirection;


        // Cast a ray to the left (negative 90 degrees).
        RaycastHit hitLeft;
        if (Physics.Raycast(origin, leftRayDirection, out hitLeft, rayDistance))
        {
            if (hitLeft.collider.CompareTag("Stone") || hitLeft.collider.CompareTag("Stone_Wall"))
            {
                hitPoint = hitLeft.point;
            }

            else
            {
                //Debug.Log("Ray didn't hit anything to the left.");
            }

        }

        // Visualize the reflected ray (optional)
        Debug.DrawRay(origin, leftRayDirection * rayDistance, Color.magenta);

        return hitPoint;
    }

    Vector3 CastRaysRight(Vector3 origin)
    {
        Vector3 hitPoint = Vector3.zero;

        // Calculate the direction from the emitter to the player
        Vector3 emitterToPlayerDirection = (player.position - rayOrigin.position).normalized;

        // Calculate the direction for the left ray (90 degrees to the left of the player)
        Vector3 rightRayDirection = Quaternion.Euler(0, 90, 0) * emitterToPlayerDirection;


        // Cast a ray to the right (positive 90 degrees).
        RaycastHit hitRight;
        if (Physics.Raycast(origin, rightRayDirection, out hitRight, rayDistance))
        {
            if (hitRight.collider.CompareTag("Stone") || hitRight.collider.CompareTag("Stone_Wall"))
            {
                hitPoint = hitRight.point;
            }

            else
            {
                //Debug.Log("Ray didn't hit anything to the right.");
            }

        }

        //Debug.Log("Hit on the right at: " + hitPoint);
        Debug.DrawRay(origin, rightRayDirection * rayDistance, Color.blue);

        return hitPoint;
    }


    ///Up to here the skeleton is formed and the first points are established. Past this point we need to cast rays that calculate the distance from the rayOrigin to each of those points

    void CastRayToReflectionPoint(Vector3 _firstHitPoint, string _prefabID, Transform _fatherTransform)
    {
        //// Create a ray from the current object's position to the target point
        Ray ray = new Ray(transform.position, _firstHitPoint - transform.position);

        // Calculate the direction from the emitter to the player
        Vector3 emitterToFirstHitDirection = (_firstHitPoint - transform.position).normalized;

        //// Initialize a RaycastHit variable to store information about the hit
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit))
        {
            // Calculate the direction vector from the object to the hit point
            Vector3 hitDirection = (hit.point - transform.position).normalized;

            // Visualize the reflected ray (optional)
            Debug.DrawRay(_fatherTransform.position, hitDirection * rayDistance * 3, Color.green);

            // Calculate the reflected direction
            Vector3 reflectedDirection = Vector3.Reflect(emitterToFirstHitDirection, hit.normal);

            // Cast a new ray from the contact point in the reflected direction
            Ray reflectedRay = new Ray(hit.point, reflectedDirection);

            // Visualize the reflected ray 
            Debug.DrawRay(hit.point, reflectedDirection * rayDistance, Color.red);

            if(!alreadyInstantiated)
            {
                if(instanceFirst < instancesFirst.Length)
                {
                    // Instantiate a prefab at the first hit point
                    instancesFirst[instanceFirst] = Instantiate(reflectionPrefab, hit.point, Quaternion.identity);

                }
            }

            if(instanceFirst < instancesFirst.Length)
            {
                if (instancesFirst[instanceFirst] != null)
                {
                    Reflection_Point prefabScript = instancesFirst[instanceFirst].GetComponent<Reflection_Point>();

                    if (prefabScript != null)
                    {
                        prefabScript.setOriginalEmitterLocation(_fatherTransform);
                        prefabScript.setPositionToBeFollowed(_firstHitPoint);
                        prefabScript.setAndTriggerTheEvent(eventName);
                    }
                }

                instanceFirst++;
            }
            

            // Initialize a Vector3 to store the hit point for the reflected ray
            Vector3 reflectedRayHitPoint = Vector3.zero;

            // Check if the reflected ray hits something
            if (Physics.Raycast(reflectedRay, out hit) && (hit.collider.CompareTag("Stone") || hit.collider.CompareTag("Stone_Wall")))
            {
                reflectedRayHitPoint = hit.point;

                if(!alreadyInstantiated)
                {
                    if(instanceReflection < instancesReflection.Length)
                    {
                        // Instantiate a prefab at the reflected ray's hit point
                        instancesReflection[instanceReflection] = Instantiate(reflectionPrefab, hit.point, Quaternion.identity);

                    }
                }

                if(instanceReflection < instancesReflection.Length)
                {
                    if (instancesReflection[instanceReflection] != null)
                    {
                        // Access the Reflection_Point script for the specific instance
                        Reflection_Point prefabScriptReflect = instancesReflection[instanceReflection].GetComponent<Reflection_Point>();

                        prefabScriptReflect.setOriginalEmitterLocation(_fatherTransform);
                        prefabScriptReflect.setPositionToBeFollowed(reflectedRayHitPoint);
                        prefabScriptReflect.setAndTriggerTheEvent(eventName);
                    }

                    instanceReflection++;
                }

                //Resetting the value
                if (instanceFirst >= instancesFirst.Length)
                {
                    instanceFirst = 0;
                }

                //Resetting the value
                if (instanceReflection >= instancesReflection.Length)
                {
                    instanceReflection = 0;
                }

            }
        }
    }



    //Called only once from the Emitter Event Post Script
    public void setEventNameFirstReflections(string _eventName)
    {
        eventName = _eventName;

    }

    private void sendRayFromEmitterToPlayer()
    {
        // Define a ray from the emitter (this object) to the player.
        Ray ray = new Ray(transform.position, player.position - transform.position);

        RaycastHit hit; // This variable will store information about the hit if any.

        // Cast the ray and check if it hits something.
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Check if the hit object has a transform.
            Transform hitTransform = hit.transform;

            // Access the name of the object that was hit.
            string hitObjectName;

            if (hitTransform.CompareTag("Stone_Wall") || hitTransform.CompareTag("Stone"))
            {
                // Get the position of the hit object.
                hitPosition = hit.point;

                // Access the name of the object that was hit.
                hitObjectName = hit.transform.gameObject.name;

                // You can now use hitObjectName, which contains the name of the hit object.
                //Debug.Log("Hit object name: " + hitObjectName);
            }

            if (hitTransform.CompareTag("Player"))
            {
                // If it doesn't hit an object with the specified tags, set hitPosition to the player's position.
                hitPosition = player.position;

                // Access the name of the object that was hit.
                hitObjectName = hit.transform.gameObject.name;

                // You can now use hitObjectName, which contains the name of the hit object.
                //Debug.Log("Hit object name: " + hitObjectName);
            }

            Debug.DrawRay(transform.position, (hitPosition - transform.position) * rayDistance, Color.magenta);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPosition, radius: 0.2f);
    }

}