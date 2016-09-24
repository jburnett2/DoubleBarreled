using UnityEngine;
using System.Collections;

public class playerMove : MonoBehaviour {

    public float walkSpeed = 6f;
    public float jumpHeight = 500f;
    public float wallPush = 400f;
    public int wJumpWait = 0;
    public int dashDuration = 0;
    bool doubleJump = true;
    bool wallJumping = false;
    bool facingRight = true;
    bool dashing = false;
    bool canDash = false;

    private Rigidbody2D theRigidBody;
    public GameObject groundCheckL;
    public GameObject groundCheckR;
    public GameObject wallCheckLT;
    public GameObject wallCheckRT;
    public GameObject wallCheckLB;
    public GameObject wallCheckRB;

    public LayerMask groundMask;

    // Use this for initialization
    void Start () {
        theRigidBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        // declaring all booleans first
        bool jumpInput = Input.GetButtonDown("Jump");
        bool groundedR = Physics2D.OverlapCircle(groundCheckR.transform.position, 0.1f, groundMask);
        bool groundedL = Physics2D.OverlapCircle(groundCheckL.transform.position, 0.1f, groundMask);
        bool wallRT = Physics2D.OverlapCircle(wallCheckRT.transform.position, 0.1f, groundMask);
        bool wallLT = Physics2D.OverlapCircle(wallCheckLT.transform.position, 0.1f, groundMask);
        bool wallRB = Physics2D.OverlapCircle(wallCheckRB.transform.position, 0.1f, groundMask);
        bool wallLB = Physics2D.OverlapCircle(wallCheckLB.transform.position, 0.1f, groundMask);

        // move the player in all circumstances other than walljumping
        float inputX = Input.GetAxis("Horizontal");
        if (!wallJumping)
        {
            theRigidBody.velocity = new Vector2(inputX * walkSpeed, theRigidBody.velocity.y);
        }

        // my janky way of doing dashes
        if ((Input.GetAxisRaw("Dash") == 1 || Input.GetButtonDown("Dash")) && canDash)
        {
            dashing = true;
            dashDuration = 8;
            canDash = false;
        }
        dashDuration--;
        if (dashDuration == 0) { dashing = false; }
        if (dashing && (inputX != 0))
        {
            theRigidBody.AddForce(new Vector2(800f * inputX, 0f));
        }

        // check to see if touching wall or ground to prevent 2 in 1 jumps
        if ((groundedR || groundedL) ||(wallRT || wallLT || wallRB || wallLB))
        {
            doubleJump = false;
            wallJumping = false;
        }

        // restore control to the player after walljumping
        if (wallJumping)
        {
            wJumpWait++;
        }
        if (wJumpWait >= 25)
        {
            wallJumping = false;
            wJumpWait = 0;
        }

        if (jumpInput) // time for some jumps
        {
            if (groundedL || groundedR) // jump from ground
            {
                theRigidBody.AddForce(new Vector2(400f * (inputX), jumpHeight));
                doubleJump = true;
                canDash = true;
                Debug.Log("Regular Jump");
            }
            else
            {
                if (doubleJump) // double jump
                {
                    doubleJump = false;
                    theRigidBody.velocity = new Vector2(theRigidBody.velocity.x, 0f);
                    theRigidBody.AddForce(new Vector2(40f * (inputX), (jumpHeight/1.2f)));
                    Debug.Log("Double Jump");
                }
                if (wallLT || wallLB) // wall jump with wall on left side
                {
                    wallJumping = true;
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(wallPush, jumpHeight));
                    doubleJump = true;
                    canDash = true;
                    Debug.Log("Wall Jump");
                }
                if (wallRT || wallRB) // wall jump with wall on right side
                {
                    wallJumping = true;
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(wallPush * -1, jumpHeight));
                    doubleJump = true;
                    canDash = true;
                    Debug.Log("Wall Jump");
                }
            }
        }

        if (theRigidBody.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (theRigidBody.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
    }
}
