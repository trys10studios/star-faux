using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float forwardSpeed = 20f; //this value is negative in the update to make it move toward the player

    private void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    /*NOTE: collision event not occuring, not sure why, but player is not losing health
     * Also need to make sure the enemy is lag following the player (like the camera) for a more
     * annoying enemy effect*/

    //handle collision events
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerMovement>().health -= 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * -forwardSpeed;
    }
}
