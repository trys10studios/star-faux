using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float _h = 0; //horizontal input
    private float _v = 0; //vertical input
    public float playerMoveSpeedHoriz = 5; //smoothing factor for player horizontal input
    public float playerMoveSpeedVert = 5; //smoothing factor for the vertical input
    public float smoothRot = 10f; //smoothing factor for ship rotation
    public int forwardSpeed = 5; //the ships speed on the Z axis
    public int boundaryTopNum = 10; //top bounds in Unity units
    public float boundaryBottomNum = 0.25f; //bottom bounds in Unity units
    public int boundarySidesNum = 7; //side bounds in Unity units
    public float rollAngle = 45f; //how much to roll the ship in degrees
    public float pitchAngle = 45f; //how much to pitch the ship in degrees
    public GameObject laser;
    public Transform defaultLaserSpawnTrans;
    public Camera cam; //main camera
    private Transform camTrans;
    public float shakeSpeed = 5.0f; //speed the ship shakes after taking damage from environment
    public float shakeAngle = 15f; //angle the ship shakes after taking damage from the environment
    [HideInInspector]
    public bool startEnvironmentDamageCoRoutine = false;
    [HideInInspector]
    public bool startGroundDamageCoRoutine = false;
    public float upwardSpeed = 1f; //speed at which player bounces off ground
    private bool playerHitGround = false;
    [HideInInspector]
    public float playerHitGroundBounceHeight = 4.0f; //height player bounces off ground
    public int health = 100; // player health
    public GameObject explosion; //player explosion effect
    public GameObject bomb; //player bomb
    public AudioClip [] playerSounds; //array for player ship sounds
    private float shotTimer = 0f; //time to wait for bombs to reload before player can shoot them again

    private void Start()
    {
        camTrans = cam.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //order matters here, check for player input, restrict player movmement, rotate the ship, then fire weapons
        PlayerInput();
        RestrictMovement();
        RotateShip();
        ShootLasers();
        ShootBombs();
        Health();

        if (startEnvironmentDamageCoRoutine == true)
        {
            StartCoroutine(TakeDamageFromEnvironment());
        }
        if (startGroundDamageCoRoutine == true)
        {
            StartCoroutine(TakeDamageFromGround());
        }
        if (startEnvironmentDamageCoRoutine == false)
        {
            StopCoroutine(TakeDamageFromEnvironment());
            //reset the camera and player ship's rotation to 0
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.identity, Time.deltaTime * smoothRot);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * smoothRot);
        }
        if (startGroundDamageCoRoutine == false)
        {
            StopCoroutine(TakeDamageFromGround());
            //reset the camera and player ship's rotation to 0
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.identity, Time.deltaTime * smoothRot);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * smoothRot);
        }
        if (playerHitGround == true)
        {
            transform.position += Vector3.up * Time.deltaTime * upwardSpeed;
            if (transform.position.y >= playerHitGroundBounceHeight)
            {
                transform.position = new Vector3(transform.position.x, playerHitGroundBounceHeight, transform.position.z);
            }
        }
    }

    private void RotateShip()
    {
        //Note: The "smoothRot" variable acts a step amount, getting closer to the desired angle each frame
        //I.e, I have it set to .1f, so it gets 1/10th as close each frame

        //if player pushes right on the R analog stick
        if (_h > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -rollAngle), smoothRot);
        }
        //if player pushes left on the R analog stick
        else if (_h < 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, rollAngle), smoothRot);
        }
        //if player pushes up on the R analog stick
        else if (_v > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-pitchAngle, 0, 0), smoothRot);
        }
        //if player pushes down on the R analog stick
        else if (_v < 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(pitchAngle, 0, 0), smoothRot);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, smoothRot);
        }
    }

    private void ShootLasers()
    {
        //code for lasers;
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(laser, defaultLaserSpawnTrans.position, transform.rotation);
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = playerSounds[1];
            audio.Play();
        }
    }
    private void ShootBombs()
    {
        /*shot timer is so the player can only fire every 3 seconds
        this allows the player to decide when they want to detonate the bomb
        and it creates a "reload" effect for the plasma bombs*/

        //NOTE: Needs a damage blast radius, needs the intensity to be changed when instantiated
        //certain "weak" enemies are instantly destroyed by it, can be used to damage the bosses too
        shotTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Fire2") && shotTimer <= 0)
        {
            Instantiate(bomb, defaultLaserSpawnTrans.position, transform.rotation);
            shotTimer = 3.0f;
        }  
    }

    //restrict movement is called in late update to avoid jitter
    void RestrictMovement()
    {
        if (transform.position.x >= boundarySidesNum)
        {
            //keep player in bounds right
            transform.position = new Vector3(boundarySidesNum, transform.position.y, transform.position.z);
            {
                if (transform.position.y >= boundaryTopNum)
                {
                    transform.position = new Vector3(transform.position.x, boundaryTopNum, transform.position.z);
                }
            }
        }
        else if (transform.position.x <= -boundarySidesNum)
        {
            //keep player in bounds left
            transform.position = new Vector3(-boundarySidesNum, transform.position.y, transform.position.z);
            {
                if (transform.position.y >= boundaryTopNum)
                {
                    transform.position = new Vector3(transform.position.x, boundaryTopNum, transform.position.z);
                }
            }
        }
        if (transform.position.y >= boundaryTopNum)
        {
            //keep player in bounds top
            transform.position = new Vector3(transform.position.x, boundaryTopNum, transform.position.z);
            {
                if (transform.position.x >= boundarySidesNum)
                {
                    transform.position = new Vector3(boundarySidesNum, transform.position.y, transform.position.z);
                }
            }
        }
        else if (transform.position.y <= boundaryBottomNum)
        {
            //keep player in bounds bottom
            transform.position = new Vector3(transform.position.x, boundaryBottomNum, transform.position.z);
            {
                if (transform.position.x <= -boundarySidesNum)
                {
                    transform.position = new Vector3(-boundarySidesNum, transform.position.y, transform.position.z);
                }
            }
        }
    }

    private void PlayerInput()
    {
        //Multiply by a negative to inverse only the vertical input to simulate traditional flying mechanics
        _h = Input.GetAxis("Horizontal") * Time.deltaTime * playerMoveSpeedHoriz;
        _v = Input.GetAxis("Vertical") * Time.deltaTime * -playerMoveSpeedVert;

        transform.position += transform.forward * Time.deltaTime * forwardSpeed;

        //if player is colliding with environment break out of this function
        //effectively the player loses control briefly
        if (startEnvironmentDamageCoRoutine == true)
        {
            return;
        }

        //handle movement
        if (_v > 0 || _v < 0 || _h > 0 || _h < 0)
        {
            transform.Translate(_h, _v, 0, Space.World);
        }
    }
    void Health()
    {
        //player health
        if(health <= 0)
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            SceneManager.LoadScene("TestLevel");
        }
    }

    public IEnumerator TakeDamageFromGround()
    {
        //cause camera to shake
        //cause player ship to shake
        playerHitGround = true;
        shakeAngle = Time.deltaTime * shakeSpeed;
        yield return new WaitForSeconds(0.1f);
        camTrans.transform.Rotate(shakeAngle, shakeAngle, 0f);
        transform.Rotate(shakeAngle, shakeAngle, 0f);
        yield return new WaitForSeconds(0.1f);
        camTrans.transform.Rotate(shakeAngle, -shakeAngle, 0f);
        transform.Rotate(shakeAngle, shakeAngle, 0f);
        camTrans.rotation = Quaternion.Slerp(camTrans.rotation, Quaternion.identity, smoothRot);
        yield return new WaitForSeconds(0.3f);
        playerHitGround = false;
    }
    public IEnumerator TakeDamageFromEnvironment()
    {
        //cause camera to shake
        //cause player ship to shake
        shakeAngle = Time.deltaTime * shakeSpeed;
        yield return new WaitForSeconds(0.1f);
        camTrans.transform.Rotate(shakeAngle, shakeAngle, 0f);
        transform.Rotate(shakeAngle, shakeAngle, 0f);
        yield return new WaitForSeconds(0.1f);
        camTrans.transform.Rotate(shakeAngle, -shakeAngle, 0f);
        transform.Rotate(shakeAngle, shakeAngle, 0f);
        camTrans.rotation = Quaternion.Slerp(camTrans.rotation, Quaternion.identity, smoothRot);
    }
}
