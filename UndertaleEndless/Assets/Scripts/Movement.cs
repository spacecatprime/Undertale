using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public GameObject player;
    public float x = 0;
    public float y = 0;

    public bool currentlyInvincible;

    public float speed;
    public Rigidbody2D rb;

    public Sprite normal;
    public Sprite invincible;
    public SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();

        rb = player.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
            y = speed;
        else if (Input.GetKey(KeyCode.DownArrow))
            y = -speed;
        else
            y = 0;

        if (Input.GetKey(KeyCode.RightArrow))
            x = speed;
        else if (Input.GetKey(KeyCode.LeftArrow))
            x = -speed;
        else
            x = 0;

        if(GameManager.isInvincible && !currentlyInvincible)
        {
            StartCoroutine(mercyFrames());
        }
        rb.velocity = new Vector2(x, y);
    }

    public IEnumerator mercyFrames()
    {
        currentlyInvincible = true;
        sprite.sprite = invincible;
        yield return new WaitForSeconds(1.0f);
        GameManager.isInvincible = false;
        sprite.sprite = normal;
        currentlyInvincible = false;
    }
}
