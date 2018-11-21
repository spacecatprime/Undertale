using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {

    public GameObject projectileTemplate;
    public GameObject player;
    public Projectile projectileProperties;

    public bool spawning = false;

    public float spawnWaitTime;

    private float spawnLocationY;
    private float spawnLocationX;


    public string spawnPos;
    public Vector2 spawnLoc;
    public Quaternion spawnRot;


    // Use this for initialization
    void Start () 
    {
    }

    // Update is called once per frame
    void Update () {

        spawnWaitTime = projectileProperties.Curve.Evaluate(Time.time) + 0.25f;

        spawnPos = projectileProperties.spawnLocation.ToString();
        if(spawnPos == "Random")
        {
            int random = Random.Range(1, 5);
            if (random == 1)
                spawnPos = "Top";
            else if (random == 2)
                spawnPos = "Bottom";
            else if (random == 3)
                spawnPos = "Left";
            else
                spawnPos = "Right";

        }

        if (spawnPos == "Top")
        {
            spawnLocationY = 3.0f;
            spawnLocationX = Random.Range(-0.75f, 0.75f);
            spawnRot = Quaternion.Euler(new Vector3(0, 0, -180));
        }

        else if (spawnPos == "Bottom")
        {
            spawnLocationY = -3.0f;
            spawnLocationX = Random.Range(-0.75f, 0.75f);
            spawnRot = Quaternion.Euler(new Vector3(0, 0, 180));
        }

        else if (spawnPos == "Left")
        {
            spawnLocationX = -3.0f;
            spawnLocationY = Random.Range(-0.75f, 0.75f);
            spawnRot = Quaternion.Euler(new Vector3(0, 0, -90));
        }

        else// if (spawnPos == "Right")
        {
            spawnLocationX = 3.0f;
            spawnLocationY = Random.Range(-0.75f, 0.75f);
            spawnRot = Quaternion.Euler(new Vector3(0, 0, 90));
        }


        spawnLoc = new Vector2(spawnLocationX, spawnLocationY);

        if (!spawning)
        {
            StartCoroutine(SpawnProjectile());
        }
    }

    IEnumerator SpawnProjectile()
    {
        spawning = true;
        yield return new WaitForSeconds(spawnWaitTime);

        GameObject instance = (GameObject)Instantiate<GameObject>(projectileTemplate, spawnLoc, spawnRot);
        var instanceCollider = instance.GetComponent<PolygonCollider2D>();
        var instanceSprite = instance.GetComponent<SpriteRenderer>();
        
        Destroy(instanceCollider);                              //Remove Collider
        instanceSprite.sprite = projectileProperties.image;     //Assign Sprite
        instance.AddComponent<PolygonCollider2D>();             //Add Collider with sprite collision

        spawning = false;
    }


}
