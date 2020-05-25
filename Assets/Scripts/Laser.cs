using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 5.0f;
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
        //destroy gameObject after 3 seconds
        Destroy(gameObject, 3.0f);
    }
}
