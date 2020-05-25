using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public float dimLight = 3f; //how much to decrease the light intensity
    private float timer = 1.0f; //delay timer for light intensity
    public float growExplosion = 5f; //how much to grow the explosion scale size
    public float maxGrowthSize = 1000f; //how large the explosion should be
    public GameObject parentGO; //parent game object is called to be destroyed by this script

    private void Start()
    {
        //destroy parent game object to remove it from the scene after 3 seconds
        Destroy(parentGO, 3.0f);
    }
    private void Update()
    {
            //if maxGrowthSize has not been reached, exponentially grow the local scale size
        transform.localScale += new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z)
            * growExplosion * growExplosion * Time.deltaTime;

        //if max growth size has been reached, lock it to the max size
        if (transform.localScale.x >= maxGrowthSize ||
            transform.localScale.y >= maxGrowthSize ||
            transform.localScale.z >= maxGrowthSize)
        {
            transform.localScale = new Vector3(maxGrowthSize, maxGrowthSize, maxGrowthSize);
        }
        //when timer reaches zero, decrease the intensity of the light by dimlight, frame rate independent
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GetComponentInChildren<Light>().intensity -= dimLight * Time.deltaTime;
        }
    }
}
