using UnityEngine;
using System.Collections;

public class playerShoot : MonoBehaviour {

    public int playerNumber;
    public float shotForce;
    private playerAim pAim;
    private Rigidbody2D theRigidBody;
    private playerAim.Facing aimDir;
    private bool isAxisInUse = false;
    private int ammo = 2;

    public Sprite shotSprite;
    public Sprite emptySprite;
    public GameObject ShotRenderer;
    private SpriteRenderer sr;

    // Use this for initialization
    void Start () {
        theRigidBody = GetComponent<Rigidbody2D>();
        pAim = GameObject.FindObjectOfType(typeof(playerAim)) as playerAim;
        sr = ShotRenderer.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 direction = new Vector2(Input.GetAxis("hAim" + playerNumber), Input.GetAxis("vAim" + playerNumber));
        aimDir = pAim.FindFacing(direction);

        // this is the shittiest thing I really need to know the better way
        if (Input.GetAxisRaw("Shoot" + playerNumber) == 1 && !isAxisInUse)
        {
            isAxisInUse = true;
            if (aimDir == playerAim.Facing.None && ammo >= 0) // aim nowhere, shoot right
            {
            } else {
                ammo--;
                if (aimDir == playerAim.Facing.Down && ammo >= 0) // shooting DOWN 
                {
                    theRigidBody.velocity = new Vector2(theRigidBody.velocity.x, 0f);
                    theRigidBody.AddForce(new Vector2(0f, shotForce));

                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x, ShotRenderer.transform.position.y - 0.9f);
                }
                else if (aimDir == playerAim.Facing.Up && ammo >= 0) // shooting UP
                {
                    theRigidBody.velocity = new Vector2(theRigidBody.velocity.x, 0f);
                    theRigidBody.AddForce(new Vector2(0f, -shotForce));
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x, ShotRenderer.transform.position.y + 1.1f);
                }
                else if (aimDir == playerAim.Facing.Right && ammo >= 0) // shooting RIGHT
                {
                    theRigidBody.velocity = new Vector2(0f, theRigidBody.velocity.y);
                    theRigidBody.AddForce(new Vector2(-shotForce * 1.5f, 0f));
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x + 1.1f, ShotRenderer.transform.position.y + 0.2f);
                }
                else if (aimDir == playerAim.Facing.Left && ammo >= 0) // shooting LEFT
                {
                    theRigidBody.velocity = new Vector2(0f, theRigidBody.velocity.y);
                    theRigidBody.AddForce(new Vector2(shotForce * 1.5f, 0f));
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x - 1.1f, ShotRenderer.transform.position.y + 0.2f);
                }
                else if (aimDir == playerAim.Facing.DownLeft && ammo >= 0) // shooting DOWN LEFT
                {
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(shotForce / 1.2f, shotForce / 1.2f));
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x - 0.7f, ShotRenderer.transform.position.y - 0.7f);
                }
                else if (aimDir == playerAim.Facing.DownRight && ammo >= 0) // shooting DOWN RIGHT
                {
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(-shotForce / 1.2f, shotForce / 1.2f));
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x + 0.7f, ShotRenderer.transform.position.y - 0.7f);
                }
                else if (aimDir == playerAim.Facing.UpLeft && ammo >= 0) // shooting UP LEFT
                {
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(shotForce / 1.2f, -shotForce / 1.2f));
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x - 0.75f, ShotRenderer.transform.position.y + 1.05f);
                }
                else if (aimDir == playerAim.Facing.UpRight && ammo >= 0) // shooting UP RIGHT
                {
                    theRigidBody.velocity = new Vector2(0f, 0f);
                    theRigidBody.AddForce(new Vector2(-shotForce / 1.2f, -shotForce / 1.2f));
                    ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x + 0.75f, ShotRenderer.transform.position.y + 1.05f);
                }
                if (ammo < 0 && aimDir != playerAim.Facing.None)
                {
                    sr.sprite = emptySprite;
                }
                else
                {
                    sr.sprite = shotSprite;
                }
            }
        }
        if (Input.GetButtonDown("Reload" + playerNumber))
        {
            ammo = 2;
        }
        if (Input.GetAxisRaw("Shoot" + playerNumber) == 0)
        {
            isAxisInUse = false;
            sr.sprite = emptySprite;
            ShotRenderer.transform.position = new Vector2(theRigidBody.position.x, theRigidBody.position.y);
        }
    }
    public void shotConfirm() // currently 100% unused but was helpful earlier
    {
        if (ammo == 0)
        {
            ShotRenderer.transform.position = new Vector2(ShotRenderer.transform.position.x, ShotRenderer.transform.position.y);
        }
    }
}
