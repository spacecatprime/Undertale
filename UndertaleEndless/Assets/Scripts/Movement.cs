using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float moveSpeed;     //Movement speed for the player.
    public float horizontalDir, verticalDir, lastX, lastY;     //horizontalDir & verticalDir determine the direction of the player inputs.
                                                               //lastX & lastY stores the last input in the appropriate axis for later use.
    private bool isMoving;      //Boolean isMoving determines whether the player is currently moving.
    private Rigidbody2D playerRB;   //Variable playerRB declared as RigidBody2D

    public Vector2 inputDir;

    public static bool moving;
    
    public bool currentlyInvincible;

    public Sprite normal;
    public Sprite invincible;
    public SpriteRenderer sprite;

    private void OnEnable()
    {
        sprite.sprite = normal;
        currentlyInvincible = false;
        GameManager.isInvincible = false;
    }

    // Use this for initialization
    void Start() {

        sprite = this.gameObject.GetComponent<SpriteRenderer>();

        playerRB = GetComponent<Rigidbody2D>(); //Gets Player's RigidBody2D component.
    }

    Vector3 SnapTo(Vector3 v3, float snapAngle)
    {
        float angle = Vector3.Angle(v3, Vector3.up);
        if (angle < snapAngle / 2.0f)          // Cannot do cross product 
            return Vector3.up * v3.magnitude;  //   with angles 0 & 180
        if (angle > 180.0f - snapAngle / 2.0f)
            return Vector3.down * v3.magnitude;

        float t = Mathf.Round(angle / snapAngle);
        float deltaAngle = (t * snapAngle) - angle;

        Vector3 axis = Vector3.Cross(Vector3.up, v3);
        Quaternion q = Quaternion.AngleAxis(deltaAngle, axis);
        return q * v3;
    }

    // Update is called once per frame
    void FixedUpdate() {

        horizontalDir = inputDir.x; //Gets horizontal input from inputDirX.
        verticalDir = inputDir.y;     //Gets vertical input from inputDirY.
        isMoving = false;                               //Sets isMoving to false automatically.

        //These two if statements check whether the player has inputted movement, if so it adds a force to the RigidBody2D to move the Player.
        if (horizontalDir > 0.5f || horizontalDir < -0.5f)
        {
            //transform.Translate(new Vector3(horizontalDir * moveSpeed * Time.deltaTime, 0f, 0f));   //Moves player using a vector3 object.
            //Vector3 takes in x, y, and z directions, so we manipulate the axis through the corresponding input. 

            playerRB.velocity = new Vector2(horizontalDir * moveSpeed, playerRB.velocity.y);    //Moves the player through manipulating the RigidBody.
            isMoving = true;        //Sets isMoving to true.
            lastX = horizontalDir;  //Stores the last direction into lastX for the anim object.
            lastY = 0f;             //Resets lastY to 0 for the anim object.
        }
        if (verticalDir > 0.5f || verticalDir < -0.5f)  //Similar to the first if statement, only for the vertical axis.
        {
            //transform.Translate(new Vector3(0f, verticalDir * moveSpeed * Time.deltaTime, 0f));

            playerRB.velocity = new Vector2(playerRB.velocity.x, verticalDir * moveSpeed);
            isMoving = true;
            lastX = 0f;
            lastY = verticalDir;
        }

        //These if statements check whether there is no longer force input, if there is none then set RB axis force to 0.
        if (horizontalDir == 0)
        {
            playerRB.velocity = new Vector2(0f, playerRB.velocity.y);
        }
        if (verticalDir == 0)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0f);
        }


        if (GameManager.isInvincible && !currentlyInvincible)
        {
            StartCoroutine(mercyFrames());
        }
    }


    public IEnumerator mercyFrames()
    {
        currentlyInvincible = true;
        sprite.sprite = invincible;
        yield return new WaitForSeconds(1.0f);
        sprite.sprite = normal;
        currentlyInvincible = false;
        GameManager.isInvincible = false;
    }

    public void N()
    {
        inputDir = new Vector3(0.0f, 1.0f);
    }

    public void NE()
    {
        inputDir = new Vector3(0.7f, 0.7f);
    }

    public void E()
    {
        inputDir = new Vector3(1.0f, 0f);
    }

    public void SE()
    {
        inputDir = new Vector3(0.7f, -0.7f);
    }

    public void S()
    {
        inputDir = new Vector3(0.0f, -1.0f);
    }

    public void SW()
    {
        inputDir = new Vector3(-1.0f, -0.7f);
    }

    public void W()
    {
        inputDir = new Vector3(-1.0f, 0f);
    }

    public void NW()
    {
        inputDir = new Vector3(-0.7f, 0.7f);
    }

    public void ZeroMovement()
    {
        inputDir = Vector3.zero;
    }
}
