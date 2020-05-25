using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour
{
    public Material matColor;
    private GameObject playerGO;

    private void Start()
    {
        //set the player material color to blue at start
        matColor.color = Color.blue;
        playerGO = GameObject.FindWithTag("Player");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //change player ship color to red
            matColor.color = Color.red;
            //set Environment Damage CoRoutine boolean to true on PlayerMovement script
            other.gameObject.GetComponentInParent<PlayerMovement>().startGroundDamageCoRoutine = true;
            //play damage sound attached to player
            AudioSource audio = other.gameObject.GetComponentInParent<AudioSource>();
            audio.clip = other.gameObject.GetComponentInParent<PlayerMovement>().playerSounds[0];
            audio.Play();
            StartCoroutine(ResetPlayer());
            StartCoroutine(ReturnColor());
        }
        if(other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject, 0.5f);
        }
    }
    IEnumerator ReturnColor()
    {
        //wait one quarter second to change the player jet back to its original color
        yield return new WaitForSeconds(0.25f);
        matColor.color = Color.blue;
    }
    IEnumerator ResetPlayer()
    {
        //wait one quarter second to change the player jet back to its original rotation
        yield return new WaitForSeconds(0.25f);
        //stop Environment Damage CoRoutine boolean to false on PlayerMovement script
        playerGO.GetComponentInParent<PlayerMovement>().startGroundDamageCoRoutine = false;
        //NOTE: try adding the camera's smoothing effect to make sure it's at 0, 0, 0 rotation
    }
}
