using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {

    public List<Projectile> projectilePropertiesList;
    public Projectile projectileProperties;

    public float x;
    public float y;
    public float speed;
    public int Class;


    // Use this for initialization
    void Start () {
        Class = Mathf.RoundToInt(this.transform.localScale.z);

        projectileProperties = ProjectileManager.staticProjectileList[Class];
    

        var instanceSprite = this.gameObject.GetComponent<SpriteRenderer>();
        instanceSprite.sprite = ProjectileManager.staticProjectileList[Class].image;                   //Assign Sprite
        this.gameObject.AddComponent<PolygonCollider2D>();                                             //Add Collider with sprite collision
        var instanceCollider = this.gameObject.GetComponent<PolygonCollider2D>();                      //Get Collider
        instanceCollider.isTrigger = true;


        speed = projectileProperties.speed;
    }
	
	// Update is called once per frame
	void Update () {

        this.gameObject.transform.Translate(0, speed * Time.deltaTime, 0, Space.Self);

        if (this.gameObject.transform.position.x > 10 || this.gameObject.transform.position.x < -10 || this.gameObject.transform.position.y > 10 || this.gameObject.transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }

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
}
