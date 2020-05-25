using UnityEngine;
using System.Collections;

public class Arch : MonoBehaviour
{
    public Material matColor;

    private void Start()
    {
        //set the player material color to blue at start
        matColor.color = Color.blue;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //change player ship color to red
            matColor.color = Color.red;
            //set Environment Damage CoRoutine boolean to true on PlayerMovement script
            other.gameObject.GetComponentInParent<PlayerMovement>().startEnvironmentDamageCoRoutine = true;
            //play damage sound attached to arch
            GetComponentInChildren<AudioSource>().Play();
        }
        if(other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ReturnColor());
            //stop Environment Damage CoRoutine boolean to false on PlayerMovement script
            other.gameObject.GetComponentInParent<PlayerMovement>().startEnvironmentDamageCoRoutine = false;
        }
    }
    IEnumerator ReturnColor()
    {
        //wait one quarter second to change the player jet back to its original color
        yield return new WaitForSeconds(0.25f);
        matColor.color = Color.blue;
    }
}
