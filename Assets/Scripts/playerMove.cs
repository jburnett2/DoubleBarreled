﻿using UnityEngine;
using System.Collections;

public class playerMove : Observable {

    public int playerNumber;
    public float walkSpeed;
    public float jumpHeight;
    public float wallPush;
    public float spawnPosX;
    public float spawnPosY;

    private float airSpeed = 15f;
    private int wallTouched = 0;
    private bool lastLeft;
    private bool lastRight;
    private bool doubleJump = true;
    public  bool dead = false;

    private Rigidbody2D theRigidBody;
    public GameObject groundCheckL;
    public GameObject groundCheckR;
    public GameObject wallCheckLT;
    public GameObject wallCheckRT;
    public GameObject wallCheckLB;
    public GameObject wallCheckRB;
    public GameObject legsSprite;
    public GameObject SoundManager;

    private SpriteRenderer sr;
    private SpriteRenderer lsr;

    public Sprite emptySpr;
    public Sprite uidle;
    public Sprite ujump;
    public Sprite ufall;
    public Sprite ulookup;
    public Sprite ulookdown;
    public Sprite uwallslide;
    public Sprite lidle;
    public Sprite ljump;
    public Sprite lwallslide;
    public Sprite hurt;
    public Sprite killed;
    public LayerMask groundMask;

    private playerAim pAim;
    private playerShoot pShoot;
    private soundManager smScript;

    // Use this for initialization
    void Start () {
        theRigidBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        lsr = legsSprite.GetComponent<SpriteRenderer>();
        pAim = GameObject.FindObjectOfType(typeof(playerAim)) as playerAim;
        smScript = SoundManager.GetComponent<soundManager>();
        pShoot = GameObject.FindObjectOfType(typeof(playerShoot)) as playerShoot; 
    }

    // Update is called once per frame
    void Update()
    {
        // declaring all booleans first
        bool jumpInput = Input.GetButtonDown("Jump" + playerNumber);
        bool groundedR = Physics2D.OverlapCircle(groundCheckR.transform.position, 0.1f, groundMask);
        bool groundedL = Physics2D.OverlapCircle(groundCheckL.transform.position, 0.1f, groundMask);
        bool wallRT = Physics2D.OverlapCircle(wallCheckRT.transform.position, 0.1f, groundMask);
        bool wallLT = Physics2D.OverlapCircle(wallCheckLT.transform.position, 0.1f, groundMask);
        bool wallRB = Physics2D.OverlapCircle(wallCheckRB.transform.position, 0.1f, groundMask);
        bool wallLB = Physics2D.OverlapCircle(wallCheckLB.transform.position, 0.1f, groundMask);
        Vector2 direction = new Vector2(Input.GetAxis("hAim" + playerNumber), Input.GetAxis("vAim" + playerNumber));
        playerAim.Facing aimDir = pAim.FindFacing(direction);

        levelLoop();

        if (dead)
        {
            //getShotAndDie();
            if (theRigidBody.velocity.y != 0)
            {
                sr.sprite = hurt;
            }
            else
            {
                theRigidBody.velocity = new Vector2(0f, 0f);
                sr.sprite = killed;
            }
        }
        else
        {
        float inputX = Input.GetAxis("Horizontal" + playerNumber);
        float inputY = Input.GetAxis("Vertical" + playerNumber);

            if (groundedL || groundedR) // movement when grounded
            {
                if (inputX == 0) // stop em if they aren't inputting anything
                {
                    theRigidBody.velocity = new Vector2(0f, theRigidBody.velocity.y);
                }
                else if (theRigidBody.velocity.x > 10 || theRigidBody.velocity.x < -10) // limit speed when going too fast aka always
                {
                    theRigidBody.velocity = new Vector2(10f * inputX, theRigidBody.velocity.y);
                }
                else // add force to move in normal circumstances
                {
                    theRigidBody.AddForce(new Vector2(inputX * walkSpeed, 0f));
                }
                if (aimDir == playerAim.Facing.None) // how to handle the sprite if they are standing still and not aiming
                {
                    if (inputY == 0)
                    {
                        sr.sprite = uidle; // no y input, dont look anywhere
                    }
                    else if (inputY > 0)
                    {
                        sr.sprite = ulookup; // look up if positive y input on left stick
                    }
                    else
                    {
                        sr.sprite = ulookdown; // look down if negative y input on left stick
                    }
                }
                lsr.sprite = lidle; // if you're on the ground and not moving then have regular legs
            }
            else // movement in air and on walls
            {
                if (theRigidBody.velocity.x > airSpeed) // limit speed by adding a negative force instead 
                {
                    theRigidBody.AddForce(new Vector2(-25f, 0f));
                }
                else if (theRigidBody.velocity.x < -airSpeed) // cant have a nice 2 in one check because you can be moving in one direction but inputting the other
                {
                    theRigidBody.AddForce(new Vector2(25f, 0f));
                }
                else // add force to actually move the player. Due to no hard limit on speed in the air, walk speed is divided by 1.5
                {
                    theRigidBody.AddForce(new Vector2(inputX * (walkSpeed / 1.5f), 0f));
                }
                if (theRigidBody.velocity.y >= 0 && !(groundedL || groundedR)) // moving up?
                {
                    lsr.sprite = ljump; // give em the right legs
                    if (aimDir == playerAim.Facing.None)
                    {
                        sr.sprite = ujump; // upwards looking when rising
                    }
                }
                else if (theRigidBody.velocity.y < 0 && !(groundedL || groundedR))
                {
                    if (aimDir == playerAim.Facing.None)
                    {
                        sr.sprite = ufall; // downwards looking sprite when falling
                    }
                }
            }

            if (wallRT || wallLT || wallRB || wallLB) // doing a couple of different things here...
            {
                doubleJump = false; // prevent 2 in 1 walljumps
                wallTouched = 10; // allow walljumping briefly after leaving wall            
                sr.sprite = uwallslide; // change sprite to wall sliding one
                lsr.sprite = lwallslide; // same for the legs
                if (wallLT || wallLB)
                {
                    sr.flipX = true; // flip while wall sliding on left walls
                    lastLeft = true;
                    lastRight = false; // notify that the last wall touched was on the left
                }
                else
                {
                    sr.flipX = false; // not needed most of the time but helps if theres no x input
                    lastLeft = false;
                    lastRight = true; // notify that the last wall touched was on the right
                }
            }
            else // if you arent touching a wall...
            {
                if (wallTouched > 0) // using wallTouched like this, the player can walljump briefly after leaving a wall. Makes movement feel better.
                {
                    wallTouched--; // decrease wallTouched while its above 0
                }
            }

            if (groundedR || groundedL) // allows the player to double jump if they run off a ledge instead of jumping off
            {
                doubleJump = true;
            }

            if (jumpInput) // time for some jumps
            {
                if (groundedL || groundedR) // jump from ground
                {
                    theRigidBody.AddForce(new Vector2((inputX), jumpHeight));
                    doubleJump = true;
                    notify("jump");
                }
                else
                {
                    if (doubleJump) // double jump
                    {
                        doubleJump = false;
                        theRigidBody.velocity = new Vector2(theRigidBody.velocity.x, 0f);
                        theRigidBody.AddForce(new Vector2(40f * (inputX), (jumpHeight / 1.1f)));
                        notify("doublejump");
                    }
                    if (wallTouched > 0 && lastLeft) // wall jump with wall on left side 
                    {
                        theRigidBody.velocity = new Vector2(0f, 0f);
                        theRigidBody.AddForce(new Vector2(wallPush, jumpHeight));
                        doubleJump = true;
                        sr.flipX = false;
                        notify("jump");
                    }
                    if (wallTouched > 0 && lastRight) // wall jump with wall on right side
                    {
                        theRigidBody.velocity = new Vector2(0f, 0f);
                        theRigidBody.AddForce(new Vector2(-wallPush, jumpHeight));
                        doubleJump = true;
                        sr.flipX = true;
                        notify("jump");
                    }
                }
            }
            if (wallTouched == 0) // only need to worry about what the last wall touched was while you can walljump
            {
                lastLeft = false;
                lastRight = false;
            }
            // a simple check to make sure the player is facing the way they're moving
            // only happens if there is no aim input
            if (aimDir == playerAim.Facing.None)
            {
                if (inputX < 0)
                {
                    sr.flipX = true; // flip if going left
                }
                if (inputX > 0)
                {
                    sr.flipX = false; // unflip if going right
                }
            }
            if (theRigidBody.velocity.y != 0 && !(wallRT || wallLT || wallRB || wallLB) && !(groundedL || groundedR))
            {
                lsr.sprite = ljump; // keeps the legs from being invisible after sliding off a wall w/o jumping
            }
        }
    }
    override public void notify(string description)
    {
        smScript.onNotify(description);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            if (!dead)
            {
                notify("hurt");
                getShotAndDie();
                dead = true;
            }
        }
    }

    IEnumerator waitRespawn(float waitTime)
    {
        //dead = false;
        yield return new WaitForSeconds(waitTime);
        transform.position = new Vector2(spawnPosX, spawnPosY);
        dead = false;
        //pShoot.ammo = 2;
    }

    void getShotAndDie()
    {
        theRigidBody.AddForce(new Vector2(0f, 400f));
        lsr.sprite = emptySpr;
        StartCoroutine(waitRespawn(2.0f));
    }

    void levelLoop()
    {
        if (transform.position.x > 20)
        {
            transform.position = new Vector2(-19.75f, transform.position.y);
        }
        else if (transform.position.x < -20)
        {
            transform.position = new Vector2(19.75f, transform.position.y);
        }
        if (transform.position.y > 19.5)
        {
            transform.position = new Vector2(transform.position.x, -3.5f);
        }
        else if (transform.position.y < -4.0f)
        {
            transform.position = new Vector2(transform.position.x, 19f);
        }
    }
}
