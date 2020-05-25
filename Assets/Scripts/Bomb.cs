using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float bombSpeed = 5.0f;
    public GameObject explosion;
    [SerializeField]
    float timer = 3.0f;

    private void Update()
    {
        //Need to figure out why intensity isn't working as well
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
}
