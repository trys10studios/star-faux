using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class EnemyLaser : MonoBehaviour
{
    private Rigidbody rb; // rigidbody attached to enemy laser
    public Vector3 thrustForce; // thrust vector for enemy laser

    // Added awake because rigidbody wasn't being assigned before being called
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    //handle trigger events
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // subtract 5 health points
            GameManager.instance.playerHit = true;
            GameManager.instance.playerHealth -= 5;

            GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move enemy laser with rigidbody physics
        rb.AddForce(thrustForce, ForceMode.Impulse);
    }
}
