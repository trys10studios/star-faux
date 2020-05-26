using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float bombSpeed = 5.0f;
    public GameObject explosion;
    [SerializeField]
    float timer = 3.0f;

    private void Update()
    {
        //NOTE: Might be better to move the bomb with force but for now it does work, possibly may not work and "teleport" since it's not using physics to move
        transform.position += transform.forward * Time.deltaTime * bombSpeed;
        if(Input.GetButtonDown("Fire2"))
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);            
        }
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Arch"))
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
