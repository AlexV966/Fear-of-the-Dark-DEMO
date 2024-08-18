using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerVelocity : MonoBehaviour
{
    private Rigidbody rb;
    private Volume volume;
    private MotionBlur motionBlur;

    public GameObject volumeObject;

    public float maxSpeed = 15.0f;
    public float maxIntensity = 1.0f;
    public float intensity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Get the MotionBlur from the URP Volume Profile
        volume = volumeObject.GetComponent<Volume>();
        volume.sharedProfile = volumeObject.GetComponent<Volume>().sharedProfile;
        volume.isGlobal = true;
        volume.weight = 1f;

        volume.profile.TryGet(out motionBlur);
    }

    private void Update()
    {
        // Calculate the speed velocity
        float speed = rb.velocity.magnitude;

        // Map the speed to the intensity
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);
        intensity = Mathf.Lerp(0f, maxIntensity, normalizedSpeed);

        // Set the motion blur intensity
        motionBlur.intensity.value = intensity;
    }
}
