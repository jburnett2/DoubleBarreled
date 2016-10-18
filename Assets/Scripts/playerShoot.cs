using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerShoot : Observable {

    public int playerNumber;
    public float shotForce;

    private playerAim pAim;
    private Rigidbody2D theRigidBody;
    private playerAim.Facing aimDir;
    private bool isAxisInUse = false;
    public int ammo = 2;
    private int activeTimer = 0;
    private int score = 0;

    public Sprite shotSprite;
    public Sprite emptySprite;
    public GameObject ShotRenderer;
    public GameObject SoundManager;
    public Canvas victoryCanvas;

    private Text scoreText;
    private Animator animationController;
    private PolygonCollider2D hitbox;
    private soundManager smScript;
    private playerMove pMove;

    // Use this for initialization
    void Start () {
        theRigidBody = GetComponent<Rigidbody2D>();
        pAim = GameObject.FindObjectOfType(typeof(playerAim)) as playerAim;
        animationController = ShotRenderer.GetComponent<Animator>();
        hitbox = ShotRenderer.GetComponent<PolygonCollider2D>();
        scoreText = GameObject.Find("scorep" + playerNumber).GetComponent<Text>();
        smScript = SoundManager.GetComponent<soundManager>();
        pMove = this.gameObject.GetComponent<playerMove>();
        victoryCanvas.enabled = true;
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 direction = new Vector2(Input.GetAxis("hAim" + playerNumber), Input.GetAxis("vAim" + playerNumber));
        aimDir = pAim.FindFacing(direction);

        // this is the shittiest thing I really wish there was a better way
        if (Input.GetAxisRaw("Shoot" + playerNumber) != 0 && !isAxisInUse) // if you pull the trigger and its not already pulled
        {
            isAxisInUse = true; // makes it so you wont fire multiple times in a single pull
            activeTimer = 20; // this gets decremented to set isAxisInUse to false, more on that later down
            if (aimDir == playerAim.Facing.None && ammo >= 0) // aim nowhere, do nothing
            {
            } else {
                ammo--;
                if (aimDir == playerAim.Facing.Down && ammo >= 0) // shooting DOWN 
                {
                    theRigidBody.velocity = new Vector2(theRigidBody.velocity.x, 0f); // reset y velocity first
                    theRigidBody.AddForce(new Vector2(0f, shotForce)); // add the force to push player in opposite direction
                    ShotRenderer.transform.Rotate(Vector3.forward * -90); // rotate the shot to make it face the right way
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x, ShotRenderer.transform.position.y - 2.0f); // put the sprite in the right spot so it lines up with the gun
                }
                else if (aimDir == playerAim.Facing.Up && ammo >= 0) // shooting UP
                {
                    theRigidBody.velocity = new Vector2(theRigidBody.velocity.x, 0f);
                    theRigidBody.AddForce(new Vector2(0f, -shotForce));
                    ShotRenderer.transform.Rotate(Vector3.forward * 90);
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x, ShotRenderer.transform.position.y + 2.26f);
                }
                else if (aimDir == playerAim.Facing.Right && ammo >= 0) // shooting RIGHT
                {
                    theRigidBody.velocity = new Vector2(0f, theRigidBody.velocity.y);
                    theRigidBody.AddForce(new Vector2(-shotForce * 1.5f, 0f));
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x + 2f, ShotRenderer.transform.position.y + 0.2f);
                }
                else if (aimDir == playerAim.Facing.Left && ammo >= 0) // shooting LEFT
                {
                    theRigidBody.velocity = new Vector2(0f, theRigidBody.velocity.y);
                    theRigidBody.AddForce(new Vector2(shotForce * 1.5f, 0f));
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x - 2f, ShotRenderer.transform.position.y + 0.2f);
                    ShotRenderer.transform.Rotate(Vector3.forward * 180);
                }
                else if (aimDir == playerAim.Facing.DownLeft && ammo >= 0) // shooting DOWN LEFT
                {
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(shotForce / 1.2f, shotForce / 1.2f));
                    ShotRenderer.transform.Rotate(Vector3.forward * -135);
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x - 1.48f, ShotRenderer.transform.position.y - 1.34f);
                }
                else if (aimDir == playerAim.Facing.DownRight && ammo >= 0) // shooting DOWN RIGHT
                {
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(-shotForce / 1.2f, shotForce / 1.2f));
                    ShotRenderer.transform.Rotate(Vector3.forward * -45);
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x + 1.48f, ShotRenderer.transform.position.y - 1.34f);
                }
                else if (aimDir == playerAim.Facing.UpLeft && ammo >= 0) // shooting UP LEFT
                {
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(shotForce / 1.2f, -shotForce / 1.2f));
                    ShotRenderer.transform.Rotate(Vector3.forward * 135);
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x - 1.56f, ShotRenderer.transform.position.y + 1.8f);
                }
                else if (aimDir == playerAim.Facing.UpRight && ammo >= 0) // shooting UP RIGHT
                {
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(-shotForce / 1.2f, -shotForce / 1.2f));
                    ShotRenderer.transform.Rotate(Vector3.forward * 45);
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x + 1.56f, ShotRenderer.transform.position.y + 1.8f);
                }
                if (ammo < 0 && aimDir != playerAim.Facing.None) // do nothing if you're out of ammo
                {
                    animationController.SetFloat("shoot", 0);
                }
                else
                {
                    animationController.SetFloat("shoot", Input.GetAxisRaw("Shoot" + playerNumber)); // play the shooting animation
                    hitbox.enabled = true; // enable the shot hitbox
                    notify("shoot");
                }
            }
        }
        if (Input.GetButtonDown("Reload" + playerNumber)) // reload, pretty self explanatory
        {
            ammo = 2;
        }
        if (isAxisInUse) // decrement the activeTimer if axis is in use
        {
            if (activeTimer > 0) // I do this as a way to make sure the sprite and hitbox stay in proper positions
            {
                activeTimer--; //  its a pretty bad way to do it
            }
        }
        if (activeTimer == 0) // set axis to free if the "timer" is up
        {
            isAxisInUse = false;
        }
        if (Input.GetAxisRaw("Shoot" + playerNumber) == 0) // if you're not shooting, make sure the animation doesnt play
        {
            animationController.SetFloat("shoot", 0); // makes it so the shot effect goes away when its over
        }
        if (!isAxisInUse) // if the axis isnt being used, then you're not shooting so...
        {
            hitbox.enabled = false; // disable the hitbox
            ShotRenderer.transform.position = new Vector2(theRigidBody.position.x, theRigidBody.position.y); // keep the shot on the player so its ready to render
            ShotRenderer.transform.rotation = Quaternion.identity; // set the rotation of the shot back to 0
        }
        if (pMove.dead)
        {
            isAxisInUse = true;
            ammo = 2;
        }
        if (score >= 1)
        {
            victoryCanvas.enabled = true;
        }
        else
        {
            victoryCanvas.enabled = false;
        }
    }
    override public void notify(string description)
    {
        smScript.onNotify(description);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            score++;
            scoreText.text = "" + score;
        }
    }
}
