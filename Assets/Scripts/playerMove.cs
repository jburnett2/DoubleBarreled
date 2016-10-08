using UnityEngine;
using System.Collections;

public class playerMove : MonoBehaviour {

    public int playerNumber;
    public float walkSpeed;
    public float jumpHeight;
    public float wallPush;
    private float airSpeed = 15f;
    private int wallTouched = 0;
    private bool lastLeft;
    private bool lastRight;
    bool doubleJump = true;

    private Rigidbody2D theRigidBody;
    public GameObject groundCheckL;
    public GameObject groundCheckR;
    public GameObject wallCheckLT;
    public GameObject wallCheckRT;
    public GameObject wallCheckLB;
    public GameObject wallCheckRB;

    private SpriteRenderer sr;
    public Sprite pidle;
    public Sprite pjump;
    public Sprite pfall;
    public Sprite plookup;
    public Sprite plookdown;
    public Sprite pwallslide;
    public LayerMask groundMask;
    private playerAim pAim;

    // Use this for initialization
    void Start () {
        theRigidBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pAim = GameObject.FindObjectOfType(typeof(playerAim)) as playerAim;
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

        // Trying out some completely different shit
        // Move the player when grounded and keep them within speed limit
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
            if (aimDir == playerAim.Facing.None)
            {
                if (inputY == 0)
                {
                    sr.sprite = pidle; // return to idle sprite, put this in inputX == 0 later
                }
                else if (inputY > 0)
                {
                    sr.sprite = plookup;
                }
                else
                {
                    sr.sprite = plookdown;
                }
            }
        }
        else // movement in air and on walls
        {
            if (theRigidBody.velocity.x > airSpeed) // limit speed by adding a negative force instead 
            {
                theRigidBody.AddForce(new Vector2(-25f, 0f));
            }
            else if (theRigidBody.velocity.x < -airSpeed)
            {
                theRigidBody.AddForce(new Vector2(25f, 0f));
            }/*
            else if (theRigidBody.velocity.x < 0 && inputX > 0)
            {
                //theRigidBody.AddForce(new Vector2(50f, 0f));
                theRigidBody.velocity = new Vector2(0f, theRigidBody.velocity.y);
                //theRigidBody.AddForce(new Vector2(inputX * (walkSpeed/1f), 0f));

            }
            else if (theRigidBody.velocity.x > 0 && inputX < 0)
            {
                //theRigidBody.AddForce(new Vector2(-50f, 0f));
                theRigidBody.velocity = new Vector2(0f, theRigidBody.velocity.y);
                //theRigidBody.AddForce(new Vector2(inputX * (walkSpeed / 1f), 0f));
            }*/
            else
            {
                theRigidBody.AddForce(new Vector2(inputX * (walkSpeed/1.5f), 0f));
            }
            if (theRigidBody.velocity.y >= 0)
            {
                sr.sprite = pjump; // jump sprite when going up
            }
            else
            {
                sr.sprite = pfall; // fall sprite when coming down
            }
        }

        if (wallRT || wallLT || wallRB || wallLB) // doing a couple of different things here...
        {
            doubleJump = false; // prevent 2 in 1 walljumps
            wallTouched = 10; // allow walljumping briefly after leaving wall            
            sr.sprite = pwallslide; // change sprite to wall sliding one
            if (wallLT || wallLB)
            {
                sr.flipX = true; // flip while wall sliding on left walls
                lastLeft = true;
                lastRight = false;
            }
            else
            {
                sr.flipX = false; // not needed most of the time but helps if theres no x input
                lastLeft = false;
                lastRight = true;
            }
        }
        else
        {
            if (wallTouched > 0)
            {
                //doubleJump = false;
                wallTouched--;
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
                Debug.Log("Regular Jump");
            }
            else
            {
                if (doubleJump) // double jump
                {
                    doubleJump = false;
                    theRigidBody.velocity = new Vector2(theRigidBody.velocity.x, 0f);
                    theRigidBody.AddForce(new Vector2(40f * (inputX), (jumpHeight / 1.1f)));
                    Debug.Log("Double Jump");
                }
                if (wallTouched > 0 && lastLeft) // wall jump with wall on left side 
                {
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(wallPush, jumpHeight));
                    doubleJump = true;
                    sr.flipX = false;
                    Debug.Log("Wall Jump");
                }
                if (wallTouched > 0 && lastRight) // wall jump with wall on right side
                {
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(-wallPush, jumpHeight));
                    doubleJump = true;
                    sr.flipX = true;
                    Debug.Log("Wall Jump");
                }
            }
        }
        //Debug.Log(lastLeft);
        //Debug.Log(wallTouched);
        if (wallTouched == 0)
        {
            //doubleJump = true;
            lastLeft = false;
            lastRight = false;
        }
        // a simple check to make sure the player is facing the way they're moving
        if (inputX < 0)
        {
            sr.flipX = true; // flip if going left
        }
        if (inputX > 0)
        {
            sr.flipX = false; // unflip if going right
        }
    }
}
