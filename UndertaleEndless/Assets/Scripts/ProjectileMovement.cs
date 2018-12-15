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

    public string maskInteraction;
    public string movementType;
    public string projectileTypeTint;
    public Vector3 movementDir;
    public Vector3 vectorToTarget;

    public AudioClip damaged;
    public AudioClip healed;
    public AudioSource audioSource;

    private float randomTP;
    private float randomSpeed;
    private float initializationTime;
    private float timeSinceInitialization;

    // Use this for initialization
    void Start() {
        initializationTime = Time.timeSinceLevelLoad;
        gameManager = GameObject.Find("GameManager");
        audioSource = gameManager.GetComponent<AudioSource>();

        Class = Mathf.RoundToInt(this.transform.localScale.z);

        projectileProperties = ProjectileManager.staticProjectileList[Class];

        maskInteraction = projectileProperties.maskInteraction.ToString();

        //Variables
        var instanceSprite = this.gameObject.GetComponent<SpriteRenderer>();
        instanceSprite.sprite = ProjectileManager.staticProjectileList[Class].image;                   //Assign Sprite
        this.gameObject.AddComponent<PolygonCollider2D>();                                             //Add Collider with sprite collision
        var instanceCollider = this.gameObject.GetComponent<PolygonCollider2D>();                      //Get Collider
        instanceCollider.isTrigger = true;

        movementType = projectileProperties.ProjectileMovementType.ToString();

        projectileTypeTint = projectileProperties.projectileType.ToString();

        speed = projectileProperties.speed;

        var instanceRB = this.gameObject.GetComponent<Rigidbody2D>();
        if (ProjectileManager.staticProjectileList[Class].AffectedByGravity)
            instanceRB.gravityScale = 0.05f;
        else
            instanceRB.gravityScale = 0;

        instanceSprite.flipX = ProjectileManager.staticProjectileList[Class].FlipX;
        instanceSprite.flipY = ProjectileManager.staticProjectileList[Class].FlipY;


        if (ProjectileManager.staticProjectileList[Class].RandomWaveTime) //Random Wave Movement
            randomTP = Random.Range(0.5f, 1.5f);
        else
            randomTP = 1;

        if(maskInteraction == "None")
            instanceSprite.maskInteraction = SpriteMaskInteraction.None;
        else if(maskInteraction == "VisibleInsideMask")
            instanceSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        else
            instanceSprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        randomSpeed = Random.Range(0.9f, 1.1f);

        SetTint();
        GetMovement();
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
    void Update() {
        damage = projectileProperties.damage;
        timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        //Continually Rotate Towards Player and move
        if (watchPlayer && movementType == "Magnet")
        {
            movementDir = player.transform.position - transform.position;
            var tempFacing = ProjectileMovement.FaceObject(transform.position, player.transform.position, FacingDirection.UP);
            transform.rotation = tempFacing;
        }

        //Set rotation to player and then move
        else if (watchPlayer && movementType == "DirectPlayer")
        {
            movementDir = player.transform.position - transform.position;
            var tempFacing = ProjectileMovement.FaceObject(transform.position, player.transform.position, FacingDirection.UP);
            transform.rotation = tempFacing;
            watchPlayer = false;

        }


        Move();


        //Destroy if out of bounds
        if (this.gameObject.transform.position.x > 5 || this.gameObject.transform.position.x < -5 || this.gameObject.transform.position.y > 5 || this.gameObject.transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }


    }





    void OnTriggerStay2D(Collider2D other) //Collision
    {
        if (other.tag == "Player" && damage > 0 && !GameManager.isInvincible) //Inflicts damage when vulnerable
        {
            //Play Damage
            if (projectileTypeTint == "Regular" || projectileTypeTint == "Heal")
            {
                GameManager.health -= damage;
                GameManager.isInvincible = true;
                audioSource.clip = damaged;
                audioSource.Play();
                CameraShake.shakeTrue = true;
                if (projectileProperties.destroyOnTouch)
                    Destroy(this.gameObject);
            }
            if (projectileTypeTint == "BlueNoMove" && Movement.moving) //if Blue, then if moving
            {
                GameManager.health -= damage;
                GameManager.isInvincible = true;
                audioSource.clip = damaged;
                audioSource.Play();
                CameraShake.shakeTrue = true;
                if (projectileProperties.destroyOnTouch)
                    Destroy(this.gameObject);
            }
            if (projectileTypeTint == "OrangeYesMove" && !Movement.moving) //if Orange, then if !moving
            {
                GameManager.health -= damage;
                GameManager.isInvincible = true;
                audioSource.clip = damaged;
                audioSource.Play();
                CameraShake.shakeTrue = true;
                if (projectileProperties.destroyOnTouch)
                    Destroy(this.gameObject);
            }

        }

        if (other.tag == "Player" && damage < 0) //Heals player regardless
        {
            //Play Heal
            GameManager.health -= damage;
            audioSource.clip = healed;
            audioSource.Play();

            if (projectileProperties.destroyOnTouch)
                Destroy(this.gameObject);

        }
    }

    void SetTint()
    {
        if (projectileTypeTint == "Regular") //White
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        }
        if (projectileTypeTint == "BlueNoMove") //Blue
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0.8666667f, 1f); 
        }
        if (projectileTypeTint == "OrangeYesMove") //Orange
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.9882354f, 0.6509804f, 0f); 
        }
        if (projectileTypeTint == "Heal") //Green
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.01568628f, 0.8352942f, 0f);
        }
    }





    void GetMovement()
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
        if (movementType == "SineWave")
        {
            movementDir = this.gameObject.transform.up;
        }
        if (movementType == "NegSineWave")
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
        if(movementType == "SineWave")
        {
            transform.position += movementDir * (speed * randomSpeed) * Time.deltaTime; //Move foward
            transform.position += transform.right * Mathf.Sin((timeSinceInitialization + 0.5f) * ProjectileManager.staticProjectileList[Class].WaveFrequency * randomTP) / ProjectileManager.staticProjectileList[Class].WaveMagnitude * Time.deltaTime;
        }
        if (movementType == "NegSineWave")
        {
            transform.position += movementDir * (speed * randomSpeed) * Time.deltaTime; //Move foward
            transform.position -= transform.right * Mathf.Sin((timeSinceInitialization + 0.5f) * ProjectileManager.staticProjectileList[Class].WaveFrequency * randomTP) / ProjectileManager.staticProjectileList[Class].WaveMagnitude * Time.deltaTime;
        }
    }
}
