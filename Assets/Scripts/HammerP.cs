using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class HammerP : MonoBehaviour
{
    /*
     TODO
     -Sounds (background music, hit sound (boink and toungue) )
     -Shwin animation on hit (tounge?)
     -Better shwin texture 
     -Arcade game console texture
     -Better hammer (smoothing + texture) 
     -Camera 
     -Lighting effects
     -Improve star particle
     -Score counter 
     -Make shwin flash red then disapear on hit  
     -Menu (for the hell of it)
     -Back to menu button
     -Power ups (like huge hammer power up)
     -Shwin violence.
     -Rude (random) shwin voice lines
     -Player health? Use raycasting from shwins to damage player hammer.
     -Hammer crack animation 
     
    */
    public Text scoreText;
    public Text timeText;
    public AudioSource hitSound;
    public AudioSource[] shwinHurtSounds;
    public AudioSource[] shwinComments;


    GameObject[] shwins;
    bool[] activeShwins = { false, false, false, false };
   

    float angleSpeed = 360.0f; //per second
    float hPos = 0;
    float fPos = 0;
    bool toAnimate = false;
    BoxCollider bc;
    float angle = 0;
    float activeLength = 0;
    float activeTime = 0;
    int prevActive = 0;
    System.Random rnd = new System.Random();
    Renderer render;
    Color hit;
    Color normal;
    float lastHurtSoundTime = 0;
    int prevHurtSound = 0;
    int gameTime = 30;
    bool gotHit = false;
    
    int score;
 

    // Use this for initialization

    void Start()
    {
        shwins = GameObject.FindGameObjectsWithTag("Shwin");
        Cursor.lockState = CursorLockMode.Locked;
        bc = GetComponent<BoxCollider>();
        //render = GetComponent<Renderer>();
        hit = new Color(255, 0, 0, 0.5f);
        normal = render.material.color;
    }

    void OnTriggerEnter(Collider o)
    {
        GameObject parent = o.gameObject;

        for (int i = 0; i < 4; i++)
        {
            if (parent.Equals(shwins[i]))
            {

                print("hit");
                GameObject particle = Instantiate(Resources.Load("stars"), parent.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
                ParticleSystem p = particle.GetComponent<ParticleSystem>();
                p.Play();

                activeShwins[i] = false;
                score += 10;
                scoreText.text = "Score: " + score;

                if (Time.time - lastHurtSoundTime >= 0.2)
                {
                    lastHurtSoundTime = Time.time;
                    hitSound.Stop();
                    hitSound.Play();
                    int r = rnd.Next(0, shwinHurtSounds.Length);
                    while(r == prevHurtSound) r = rnd.Next(0, shwinHurtSounds.Length);
                    prevHurtSound = r;
                    shwinHurtSounds[r].Stop();
                    shwinHurtSounds[r].Play();
                }
               // shwins[i].transform.position = new Vector3(shwins[i].transform.position.x, -100, shwins[i].transform.position.z);

                
               // render.material.color = hit;
               // yield return new WaitForSeconds(0.5f);
                //render.material.color = normal;
                

              //  activeLength = 0;
                break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        gameTime = (int)(30.0 - Time.time);
        if (gameTime <= 0) ; //
        timeText.text = "Time: " + gameTime + "";
        hPos = Input.GetAxis("Mouse X") * -0.7f;
        fPos = Input.GetAxis("Mouse Y") * 0.7f;
        transform.position += new Vector3(fPos, transform.position.y, hPos);

        if (toAnimate == true)
        {
            if (angle >= 90.0f)
            {

                angleSpeed = angleSpeed * -1;
                angle = 0;
                transform.rotation = Quaternion.Euler(0, 0, 360 - 90);
            }
            if (angle <= -90f)
            {
                toAnimate = false;
                angle = 0;
                angleSpeed = angleSpeed * -1;
                transform.rotation = Quaternion.Euler(0, 0, 0);

            }
            else {
                float tangle = Time.deltaTime * angleSpeed;
                angle += tangle;
                transform.Rotate(new Vector3(0, 0, -1 * tangle));
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.F))
        {
            toAnimate = true;


        }
    }

    void FixedUpdate()
    {
        shwinManager();
    }

    void shwinManager()
    {
        if (Time.time - activeTime >= activeLength)
        {
            for (int i = 0; i < 4; i++) activeShwins[i] = false;
            activeTime = Time.time;
            activeLength = (float)(rnd.NextDouble() * 1.5) + 0.5f;
            int t = rnd.Next(0, 4);
            while (t == prevActive) t = rnd.Next(0, 4);
            activeShwins[t] = true;
            prevActive = t;
        }
        for (int i = 0; i < 4; i++)
        {

            if (activeShwins[i] == true)
            {
                shwins[i].transform.position = new Vector3(shwins[i].transform.position.x, 1, shwins[i].transform.position.z);
            }
            else
            {
                shwins[i].transform.position = new Vector3(shwins[i].transform.position.x, -100, shwins[i].transform.position.z);
            }
        }
    }

}
