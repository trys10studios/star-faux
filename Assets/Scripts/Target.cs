using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject explosion;
    public GameObject explosionForBomb; //no explosion sounds
    private GameObject parentGO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Bomb"))
        {
            Instantiate(explosionForBomb, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
