using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {

    public List<Projectile> projectilePropertiesList;
    public Projectile projectileProperties;

    public GameObject gameManager;

    public GameObject player;

    public float damage;

    public float x;
    public float y;
    public float speed;
    public int Class;

    public bool watchPlayer = false;

    public string movementType;
    public Vector3 movementDir;
    public Vector3 vectorToTarget;

    public AudioClip damaged;
    public AudioClip healed;
    public AudioSource audioSource;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager");
        audioSource = gameManager.GetComponent<AudioSource>();

        Class = Mathf.RoundToInt(this.transform.localScale.z);

        projectileProperties = ProjectileManager.staticProjectileList[Class];
    
        //Variables
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

    public enum FacingDirection //Facing2D
    {
        UP = 270,
        DOWN = 90,
        LEFT = 180,
        RIGHT = 0
    }

    public static Quaternion FaceObject(Vector2 startingPosition, Vector2 targetPosition, FacingDirection facing) //Facing2D
    {
        Vector2 direction = targetPosition - startingPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= (float)facing;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Update is called once per frame
    void Update () {



        if(GameManager.isInvincible)
        {
            damage = 0;
        }
        else
        {
            damage = projectileProperties.damage;
        }

        //Destroy if out of bounds
        if (this.gameObject.transform.position.x > 10 || this.gameObject.transform.position.x < -10 || this.gameObject.transform.position.y > 10 || this.gameObject.transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }

        //Continually Rotate Towards Player and move
        if(watchPlayer && movementType == "Magnet")
        {
            movementDir = player.transform.position - transform.position;
            var tempFacing = ProjectileMovement.FaceObject(transform.position, player.transform.position, FacingDirection.UP + Mathf.RoundToInt(projectileProperties.rotationModifier));
            transform.rotation = tempFacing;
        }

        //Set rotation to player and then move
        else if(watchPlayer && movementType == "DirectPlayer")
        {
            movementDir = player.transform.position - transform.position;
            var tempFacing = ProjectileMovement.FaceObject(transform.position, player.transform.position, FacingDirection.UP + Mathf.RoundToInt(projectileProperties.rotationModifier));
            transform.rotation = tempFacing;
            watchPlayer = false;

        }

        Move();

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (damage > 0 && !GameManager.isInvincible)
            {
                //Play Damage
                GameManager.isInvincible = true;
                GameManager.health -= damage;
                audioSource.clip = damaged;
                audioSource.Play();
            }
            if (damage < 0)
            {
                //Play Heal
                GameManager.health -= damage;
                audioSource.clip = healed;
                audioSource.Play();
            }
                        //Destroy if on touch and you can hit player                         Destroy if healing item anyway if player is invincible
            if(projectileProperties.destroyOnTouch && !GameManager.isInvincible || projectileProperties.destroyOnTouch && GameManager.isInvincible && damage < 0)
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
        if (movementType == "DirectPlayer")
        {
            watchPlayer = true;
        }
        if (movementType == "Magnet")
        {
            watchPlayer = true;
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
            transform.position += movementDir * speed * Time.deltaTime;
        }
        if(movementType == "DirectPlayer")
        {
            transform.position += movementDir * speed * Time.deltaTime;
        }
        if (movementType == "Magnet")
        {
            transform.position += -movementDir * speed * 1/(movementDir.x + movementDir.y) * Time.deltaTime;
        }
    }
}
