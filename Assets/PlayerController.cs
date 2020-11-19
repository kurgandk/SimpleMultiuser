using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


public class PlayerController : NetworkBehaviour
{

    public float rotationsSpeed = 2f;
    private Rigidbody2D rbd;
    private Transform engineFlameTransform;
    private GameObject cameraLink;
    private int health = 100;
    public GameObject bulletPrefab;
    public int score;
    public Text scoreText;
    public GameObject nozzleLink;
    public AudioSource engineSound;
    public AudioSource hitSound;
    public float maxDiameter = 15f;

    // Use this for initialization
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();

        rbd.transform.position = new Vector3(Random.Range(-maxDiameter * 0.6f, maxDiameter*0.6f) , Random.Range(-maxDiameter * 0.6f, maxDiameter * 0.6f), 0f);
        // only do this for my own player/ship
        if (isLocalPlayer)
        {
            score = 0;
            // make my own ship red
            GetComponent<SpriteRenderer>().color = Color.red;
            // find local camera
            cameraLink = GameObject.Find("CameraHolder");
            // find my engine flame
            engineFlameTransform = this.gameObject.transform.GetChild(0);
        }
        takeDMG();

    }

    // Update is called once per frame
    void Update()
    {
        // only do this for my own player/ship
        if (isLocalPlayer)
        {
            // get keyboard input wasd + space
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            if (Input.GetKeyDown(KeyCode.Space)) {
                // call server bulletspawner
                CmdFireBullet();
            }

            // move and rotate ship
            rbd.AddForce(transform.up * y);
            engineFlameTransform.localScale = (Vector3.one) * (y+1f) * 0.3f;
            engineSound.volume = Mathf.Abs(y);
            rbd.rotation -= x * rotationsSpeed;

            // update camera
            cameraLink.transform.position = transform.position;
        }


    }
    [Command]
    void CmdFireBullet()
    {
        // instantiate bullet
        var bullet= Instantiate(bulletPrefab, nozzleLink.transform.position, transform.rotation);

        // set the speed
        //bullet.GetComponent<Rigidbody2D>().velocity = transform.up * 6;

        // spawn on network
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public void takeDMG()
    {
        Debug.Log("take dmg");
        hitSound.Play();
        health--;
        scoreText.text = health.ToString();
    }

}
