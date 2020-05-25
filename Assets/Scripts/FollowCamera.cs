using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.position + offset;

        // Smoothly move the camera towards that target position
        // Smooth damp is the only effective way to achieve smoothing; tried: Slerp and Lerp, both caused jerky movement in the camera
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}