using UnityEngine;
using System.Collections;

public class playerAim : MonoBehaviour {


    public enum Facing { Right, UpRight, Up, UpLeft, Left, DownLeft, Down, DownRight, None };
    private Facing aimDir;
    public int playerNumber;
    private SpriteRenderer sr;
    public Sprite aup;
    public Sprite aupr;
    public Sprite aupl;
    public Sprite ar;
    public Sprite al;
    public Sprite adw;
    public Sprite adwr;
    public Sprite adwl;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        Vector2 direction = new Vector2(Input.GetAxis("hAim" + playerNumber), Input.GetAxis("vAim" + playerNumber));
        //Debug.Log(FindFacing(direction));
        aimDir = FindFacing(direction);

         //this is so gross I'm sorry
        if (aimDir == Facing.Up)
        {
            sr.sprite = aup;
        }
        else if (aimDir == Facing.UpRight)
        {
            sr.sprite = aupr;
            sr.flipX = false;
        }
        else if (aimDir == Facing.Right)
        {
            sr.sprite = ar;
            sr.flipX = false;
        }
        else if (aimDir == Facing.DownRight)
        {
            sr.sprite = adwr;
            sr.flipX = false;
        }
        else if (aimDir == Facing.Down)
        {
            sr.sprite = adw;
        }
        else if (aimDir == Facing.DownLeft)
        {
            sr.sprite = adwl;
            sr.flipX = false;
        }
        else if (aimDir == Facing.Left)
        {
            sr.sprite = al;
            sr.flipX = false;
        }
        else if (aimDir == Facing.UpLeft)
        {
            sr.sprite = aupl;
            sr.flipX = false;
        }
    }

    public Facing FindFacing(Vector2 direction)
    {
        if (direction == Vector2.zero) { return Facing.None; }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Debug.Log("Angle: " + angle);
        if (angle < 0.0f) { angle = 360.0f + angle; }

        angle += 22.5f;

        int i = (int)(angle / 45.0f);
        i = i % 8;

        if (i > 7) { return Facing.None; }

        return (Facing)i;
    }

    
}
