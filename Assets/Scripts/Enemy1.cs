using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public GameObject explosion;
    public const byte forwardSpeed = 20; /*NOTE: only using byte since there is no calculation, 
    there will be no byte to int conversion, thus saving memory, otherwise int is preferred*/
    public GameObject detectPlayer;
    public GameObject enemyLaser;
    public float timer = 1.0f;
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        timer -= Time.deltaTime;
        bool enemyAttack = detectPlayer.GetComponent<DetectPlayer>().enemyAttack;
        if(enemyAttack)
        {
            //make sure forward speed matches player speed, so the enemy stays with the player
            transform.position += transform.forward * Time.deltaTime * forwardSpeed;
            if (timer <= 0)
            {
                Instantiate(enemyLaser, transform.position, transform.rotation);
                timer = 1.0f;
            }
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.position + offset;

        // Smoothly move the camera towards that target position
        // Smooth damp is the only effective way to achieve smoothing; tried: Slerp and Lerp, both caused jerky movement in the camera
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
