using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileManager : MonoBehaviour {

    public Enemy enemy;

    public GameObject projectileTemplate;
    public GameObject player;
    public Image enemyImage;
    public Image battleBG;

    public AudioSource music;

    public List<Projectile> projectilePropertiesList;
    public static List<Projectile> staticProjectileList;

    public List<bool> spawnList;

    public float spawnWaitTime;

    private float spawnLocationY;
    private float spawnLocationX;

    public int loop = 0;

    public string spawnPos;
    public string specificSpawnPos;
    public Vector2 spawnLoc;
    public Quaternion spawnRot;

    private void Awake() //Add all Projectile Scriptable Objects to List
    {


        enemyImage.sprite = enemy.EnemySprite;

        battleBG.sprite = enemy.Background;

        if(enemy.EnemySprite != null)
            enemyImage.color = new Color(1.0f, 1.0f, 1.0f);

        if (enemy.Background != null)
            battleBG.color = new Color(1.0f, 1.0f, 1.0f);

        foreach (Projectile x in enemy.ProjectilesUsed) //Add enemy projectiles to list
        {
            projectilePropertiesList.Add(x);
        }

        foreach (Projectile x in projectilePropertiesList) //Add list to static list and add spawning regulator bool
        {
            spawnList.Add(false);
        }

        staticProjectileList = projectilePropertiesList;

        music.clip = enemy.bossMusic;
        music.Play();
    }

    void Update () {

        spawnWaitTime = staticProjectileList[loop].SpawnFrequency.Evaluate(GameManager.score) + 0.1f;

        spawnLoc = new Vector2(spawnLocationX, spawnLocationY);

        if (!spawnList[loop]) //Call spawn if not spawning is true
        {
            StartCoroutine(SpawnProjectile(loop));
        }
        else
        {
            loop += 1;
            if (loop == staticProjectileList.Count)
                loop = 0;
        }

    }

    public Vector2 SpawnLocation(int loop) //Get Spawn Location
    {
        spawnPos = staticProjectileList[loop].spawnLocation.ToString();
        specificSpawnPos = staticProjectileList[loop].locationSpecific.ToString();

        if (spawnPos == "Random")
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
            spawnLocationY = 1.0f;
            spawnRot = Quaternion.Euler(new Vector3(0, 0, -180));


            if (specificSpawnPos == "None")
            {
                spawnLocationX = Random.Range(-0.75f, 0.75f);
            }
            else
            {
                spawnLocationX = staticProjectileList[loop].specificSpawnLocation;
            }

        }

        else if (spawnPos == "Bottom")
        {
            spawnLocationY = -1.0f;
            spawnRot = Quaternion.Euler(new Vector3(0, 0, 0));


            if (specificSpawnPos == "None")
            {
                spawnLocationX = Random.Range(-0.75f, 0.75f);
            }
            else
            {
                spawnLocationX = staticProjectileList[loop].specificSpawnLocation;
            }

        }

        else if (spawnPos == "Left")
        {
            spawnLocationX = -1.0f;
            spawnRot = Quaternion.Euler(new Vector3(0, 0, -90));

            if (specificSpawnPos == "None")
            {
                spawnLocationY = Random.Range(-0.75f, 0.75f);
            }
            else
            {
                spawnLocationY = staticProjectileList[loop].specificSpawnLocation;
            }


        }

        else if (spawnPos == "Right")
        {
            spawnLocationX = 1.0f;
            spawnRot = Quaternion.Euler(new Vector3(0, 0, 90));

            if (specificSpawnPos == "None")
            {
                spawnLocationY = Random.Range(-0.75f, 0.75f);
            }
            else
            {
                spawnLocationY = staticProjectileList[loop].specificSpawnLocation;
            }

        }

        var coords = new Vector2(spawnLocationX, spawnLocationY);

        return coords;
    }


    IEnumerator SpawnProjectile(int Class) //Spawning Sequence
    {
        spawnList[Class] = true;

        yield return new WaitForSeconds(spawnWaitTime * 2.0f);

        GameObject instance = (GameObject)Instantiate<GameObject>(projectileTemplate, SpawnLocation(Class), spawnRot); //Instantiate Projectile
        var instanceSprite = instance.GetComponent<SpriteRenderer>();

        instance.SetActive(true);

        instance.transform.localScale = new Vector3(instance.transform.localScale.x, instance.transform.localScale.y, Class);

        spawnList[Class] = false;
    }

}
