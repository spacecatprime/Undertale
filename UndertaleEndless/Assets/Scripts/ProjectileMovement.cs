using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {

    public List<Projectile> projectilePropertiesList;
    public Projectile projectileProperties;
    public Projectile childToSpawn;

    public GameObject gameManager;
    public GameObject projectileTemplate;
    public GameObject player;
    public GameObject box;

    public float waitForMove;
    public float spawnWaitTime;
    public float damage;
    public bool canSpawn = true;
    public bool canMove = false;
    public float x;
    public float y;
    public float speed;
    public int Class;

    public string maskInteraction;
    public string movementType;
    public string projectileTypeTint;
    public string movementDirection;
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

        movementDirection = projectileProperties.movementDirection.ToString();

        //Variables
        var instanceSprite = this.gameObject.GetComponent<SpriteRenderer>();
        instanceSprite.sprite = ProjectileManager.staticProjectileList[Class].image;                   //Assign Sprite
        Destroy(this.gameObject.GetComponent<PolygonCollider2D>());
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
        waitForMove = ProjectileManager.staticProjectileList[Class].waitTimer;

        //Set rotation to player and then move
        if (movementType == "DirectPlayer")
        {
            movementDir = player.transform.position - transform.position;
            var tempFacing = ProjectileMovement.FaceObject(transform.position, player.transform.position, FacingDirection.UP);
            transform.rotation = tempFacing;
        }

        if(ProjectileManager.staticProjectileList[Class].relationships.ToString() == "Parent")
        {
            childToSpawn = ProjectileManager.staticProjectileList[Class].child;
        }

        SetTint();
        GetMovement();
        canMove = false;
        StartCoroutine(startMove());
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
        var instanceCollider = this.gameObject.GetComponent<PolygonCollider2D>();                      //Get Collider
        instanceCollider.isTrigger = true;
        damage = projectileProperties.damage;
        timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        //Continually Rotate Towards Player and move
        if (movementType == "Magnet")
        {
            movementDir = player.transform.position - transform.position;
            var tempFacing = ProjectileMovement.FaceObject(transform.position, player.transform.position, FacingDirection.UP);
            transform.rotation = tempFacing;
        }


        if (canMove)
            Move();

        if (ProjectileManager.staticProjectileList[Class].relationships.ToString() == "Parent") //Wait time for spawning
            if(childToSpawn.RandomSpawnFrequency == true)
                spawnWaitTime = UnityEngine.Random.Range(childToSpawn.timeSpawnRange.x, childToSpawn.timeSpawnRange.y);
            else
                spawnWaitTime = childToSpawn.SpawnFrequency.Evaluate(GameManager.phaseTime);
        

        //Destroy if out of bounds
        if (this.gameObject.transform.position.x > 5 || this.gameObject.transform.position.x < -5 || this.gameObject.transform.position.y > 5 || this.gameObject.transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }

        if (ProjectileManager.staticProjectileList[Class].relationships.ToString() == "Parent" && canSpawn)
        {
            StartCoroutine(SpawnProjectile(childToSpawn));
        }

        if (timeSinceInitialization >= ProjectileManager.staticProjectileList[Class].deathTimer) //Destroy Timer
        {
            Destroy(this.gameObject);
        }

        if (!ProjectileManager.fighting && ProjectileManager.staticProjectileList[Class].destroyOnPhaseEnd) //Destroy at end of phase
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator startMove()
    {
        yield return new WaitForSeconds(waitForMove);
        canMove = true;
        if (movementType == "DirectPlayer")
        {
            movementDir = player.transform.position - transform.position;
            var tempFacing = ProjectileMovement.FaceObject(transform.position, player.transform.position, FacingDirection.UP);
            transform.rotation = tempFacing;
        }
    }

    IEnumerator SpawnProjectile(Projectile child) //Spawning Sequence
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnWaitTime);
        canSpawn = true;
        GameObject instance = (GameObject)Instantiate<GameObject>(projectileTemplate, this.transform.position, this.transform.rotation); //Instantiate Projectile
        var instanceSprite = instance.GetComponent<SpriteRenderer>();

        instance.transform.localScale = new Vector3(instance.transform.localScale.x, instance.transform.localScale.y, Class+1);

        instance.SetActive(true);
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

            if(ProjectileManager.endOnDamage) //Check if ends on damage
            {
                ProjectileManager.phaseTimer = -10;
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
        if (movementDirection == "N")
            movementDir = new Vector3(0.0f, 1.0f);
        if (movementDirection == "NE")
            movementDir = new Vector3(0.7f, 0.7f);
        if (movementDirection == "E")
            movementDir = new Vector3(1.0f, 0f);
        if (movementDirection == "SE")
            movementDir = new Vector3(0.7f, -0.7f);
        if (movementDirection == "S")
            movementDir = new Vector3(0.0f, -1.0f);
        if (movementDirection == "SW")
            movementDir = new Vector3(-1.0f, -0.7f);
        if (movementDirection == "W")
            movementDir = new Vector3(-1.0f, 0f);
        if (movementDirection == "NW")
            movementDir = new Vector3(-0.7f, 0.7f);
    }

    void Move()
    {
        if (movementType == "Straight")
        {
            transform.position += movementDir * speed * Time.deltaTime;
        }
        if(movementType == "DirectPlayer")
        {
            transform.position += -this.gameObject.transform.up * speed * Time.deltaTime;
        }
        if (movementType == "Magnet")
        {
            transform.position += -this.gameObject.transform.up * speed * 1/(movementDir.x + movementDir.y) * Time.deltaTime;
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
