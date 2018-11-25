using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public Joystick joystick;

    public Vector3 targetDir;
    public Vector3 angleDir;
    public Vector3 angleDirNormal;


    public bool moving;

    public GameObject player;
    public float x = 0;
    public float y = 0;

    public bool playerMovementTypeIsMobile;

    public bool currentlyInvincible;

    public float speed;
    private float originalSpeed;
    public Rigidbody2D rb;

    public Sprite normal;
    public Sprite invincible;
    public SpriteRenderer sprite;

    // Use this for initialization
    void Start() {
        originalSpeed = speed;

        sprite = this.gameObject.GetComponent<SpriteRenderer>();

        rb = player.GetComponent<Rigidbody2D>();
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

        if (!playerMovementTypeIsMobile) //Arrow key Movement
        {
            speed = speed * 40;
            if (Input.GetKey(KeyCode.UpArrow))
                y = 1.5f;
            else if (Input.GetKey(KeyCode.DownArrow))
                y = -1.5f;
            else
                y = 0;

            if (Input.GetKey(KeyCode.RightArrow))
                x = 1.5f;
            else if (Input.GetKey(KeyCode.LeftArrow))
                x = -1.5f;
            else
                x = 0;
            rb.velocity = new Vector2(x, y);
        }


        else //Player movement IS mobile (joystick)
        {
            speed = originalSpeed;
            Vector3 inputDir = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);

            if (inputDir != Vector3.zero)
                moving = true;
            else
                moving = false;

            targetDir = new Vector3(inputDir.x, inputDir.y);

            //angleDir = new Vector3(Mathf.RoundToInt(SnapTo(targetDir, 45.0f).x), Mathf.RoundToInt(SnapTo(targetDir, 45.0f).y));
            angleDir = new Vector3(SnapTo(targetDir, 45.0f).x, SnapTo(targetDir, 45.0f).y);

            if (moving)
            {
                rb.velocity = angleDir;
                //rb.velocity = rb.velocity * speed * Time.deltaTime;
                rb.velocity = speed * (rb.velocity.normalized);
            }
            else
                rb.velocity = Vector3.zero;
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

}
