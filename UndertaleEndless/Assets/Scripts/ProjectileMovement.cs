using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {

    public List<Projectile> projectilePropertiesList;
    public Projectile projectileProperties;

    public GameObject player;

    public float x;
    public float y;
    public float speed;
    public int Class;

    public bool watchPlayer = false;

    public string movementType;
    public Vector3 movementDir;
    


    // Use this for initialization
    void Start () {

        Class = Mathf.RoundToInt(this.transform.localScale.z);

        projectileProperties = ProjectileManager.staticProjectileList[Class];
    

        var instanceSprite = this.gameObject.GetComponent<SpriteRenderer>();
        instanceSprite.sprite = ProjectileManager.staticProjectileList[Class].image;                   //Assign Sprite
        this.gameObject.AddComponent<PolygonCollider2D>();                                             //Add Collider with sprite collision
        var instanceCollider = this.gameObject.GetComponent<PolygonCollider2D>();                      //Get Collider
        instanceCollider.isTrigger = true;

        instanceSprite.color = projectileProperties.SpriteTint;                                         //Set colour

        movementType = projectileProperties.ProjectileMovementType.ToString();

        speed = projectileProperties.speed;

        getMovement();
    }
	
	// Update is called once per frame
	void Update () {
        Move();

        if (this.gameObject.transform.position.x > 10 || this.gameObject.transform.position.x < -10 || this.gameObject.transform.position.y > 10 || this.gameObject.transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }

        if(watchPlayer)
        {
            movementDir = player.transform.position - this.gameObject.transform.position;
            var dir = movementDir - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
        }

    }

    public void rotate(bool singleTime)
    {
        if(singleTime)
            watchPlayer = false;
        else
            watchPlayer = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameManager.health -= projectileProperties.damage;

            if(projectileProperties.destroyOnTouch)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void getMovement()
    {
        if (movementType == "Straight")
        {
            movementDir = this.gameObject.transform.up;
        }
        if (movementType == "FollowPlayer")
        {
            rotate(true);
        }
        if (movementType == "Magnet")
        {
            rotate(false);
        }
        if (movementType == "Random")
        {
            movementDir = this.gameObject.transform.up;
        }
    }

    void Move()
    {
        if (movementType == "Straight")
        {
            this.gameObject.transform.position += movementDir * speed * Time.deltaTime;
        }
        if(movementType == "FollowPlayer")
        {
            this.gameObject.transform.position += movementDir * speed * Time.deltaTime;
        }
    }
}
