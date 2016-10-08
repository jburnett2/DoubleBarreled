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

    // Use this for initialization
    void Start () {
        theRigidBody = GetComponent<Rigidbody2D>();
        pAim = GameObject.FindObjectOfType(typeof(playerAim)) as playerAim;
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 direction = new Vector2(Input.GetAxis("hAim" + playerNumber), Input.GetAxis("vAim" + playerNumber));
        //bool shoot = Input.GetAxisRaw("Shoot");
        aimDir = pAim.FindFacing(direction);

        // this is the shittiest thing I really need to know the better way
        //float shoot = Input.GetAxisRaw("Shoot");
        if (Input.GetAxisRaw("Shoot" + playerNumber) == 1 && !isAxisInUse)
        {
            if (aimDir == playerAim.Facing.Down && ammo > 0)
            {
                theRigidBody.velocity = new Vector2(theRigidBody.velocity.x, 0f);
                theRigidBody.AddForce(new Vector2(0f, shotForce));
                isAxisInUse = true;
                ammo--;
            }
            else if (aimDir == playerAim.Facing.Up && ammo > 0)
            {
                theRigidBody.velocity = new Vector2(theRigidBody.velocity.x, 0f);
                theRigidBody.AddForce(new Vector2(0f, -shotForce));
                isAxisInUse = true;
                ammo--;
            }
            else if (aimDir == playerAim.Facing.Right && ammo > 0)
            {
                theRigidBody.velocity = new Vector2(0f, theRigidBody.velocity.y);
                theRigidBody.AddForce(new Vector2(-shotForce * 1.5f, 0f));
                isAxisInUse = true;
                ammo--;
            }
            else if (aimDir == playerAim.Facing.Left && ammo > 0)
            {
                theRigidBody.velocity = new Vector2(0f, theRigidBody.velocity.y);
                theRigidBody.AddForce(new Vector2(shotForce * 1.5f, 0f));
                isAxisInUse = true;
                ammo--;
            }
            else if (aimDir == playerAim.Facing.DownLeft && ammo > 0)
            {
                theRigidBody.velocity = new Vector2(0f, 0f);
                theRigidBody.AddForce(new Vector2(shotForce/1.2f, shotForce/1.2f));
                isAxisInUse = true;
                ammo--;
            }
            else if (aimDir == playerAim.Facing.DownRight && ammo > 0)
            {
                theRigidBody.velocity = new Vector2(0f, 0f);
                theRigidBody.AddForce(new Vector2(-shotForce/1.2f, shotForce/1.2f));
                isAxisInUse = true;
                ammo--;
            }
            else if (aimDir == playerAim.Facing.UpLeft && ammo > 0)
            {
                theRigidBody.velocity = new Vector2(0f, 0f);
                theRigidBody.AddForce(new Vector2(shotForce / 1.2f, -shotForce / 1.2f));
                isAxisInUse = true;
                ammo--;
            }
            else if (aimDir == playerAim.Facing.UpRight && ammo > 0)
            {
                theRigidBody.velocity = new Vector2(0f, 0f);
                theRigidBody.AddForce(new Vector2(-shotForce / 1.2f, -shotForce / 1.2f));
                isAxisInUse = true;
                ammo--;
            }
        }
        if (Input.GetButtonDown("Reload" + playerNumber))
        {
            ammo = 2;
        }
        if (Input.GetAxisRaw("Shoot" + playerNumber) == 0)
        {
            isAxisInUse = false;
        }
    }
}
