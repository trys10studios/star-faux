using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float playerHealth = 100;
    public GameObject explosion;
    public GameObject playerGO;
    public bool playerDead = false;
    public bool playerHit = false;
    public Material matColor; // player ship's color
    public AudioClip[] playerSounds; //array for player ship sounds

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // makes sure to not have a duplicate game manager
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHit();
        Health();
    }
    private void Health()
    {
        if (playerHealth <= 0 && !playerDead)
        {
            Instantiate(explosion, playerGO.transform.position, explosion.transform.rotation);
            playerDead = true;
            playerGO.SetActive(false);
        }
    }
    void PlayerHit()
    {
        if(playerHit)
        {
            // change player ship color to red
            matColor.color = Color.red;

            StartCoroutine(ReturnColor());
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = playerSounds[0];
            audio.Play();
            playerHit = false;
        }
    }
    public IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("TestLevel");
    }
    IEnumerator ReturnColor()
    {
        //wait one quarter second to change the player jet back to its original color
        yield return new WaitForSeconds(0.25f);
        matColor.color = Color.blue;
    }
}