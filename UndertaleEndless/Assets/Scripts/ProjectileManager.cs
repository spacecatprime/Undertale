using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileManager : MonoBehaviour {

    public Enemy enemy;

    public bool fighting;

    public GameObject projectileTemplate;
    public GameObject player;
    public Image enemyImage;
    public Image battleBG;

    public AudioSource music;

    public List<FightPhase> fightPhaseList;
    public List<Projectile> projectilePropertiesList;
    public static List<Projectile> staticProjectileList;

    public int currentPhase = 0;
    public int maxPhases = 0;

    public List<bool> spawnList;

    private float spawnWaitTime;

    private float spawnLocationY;
    private float spawnLocationX;

    private float phaseTimer;

    public int projectileType = 0;

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



        foreach (FightPhase x in enemy.Phases) //Proccess enemy phases
        {
            
            foreach (Projectile y in x.ProjectileCombo)
            {
                projectilePropertiesList.Add(y);
                y.phase = maxPhases;
                spawnList.Add(false); //add spawning regulator bool
            }
            fightPhaseList.Add(x);
            maxPhases += 1;
        }

        maxPhases -= 1;





        staticProjectileList = projectilePropertiesList;

        music.clip = enemy.bossMusic;
        music.Play();
    }

    void Update () {

        if (projectileType >= staticProjectileList.Count) //Make sure spawning projectile type never goes above max projectiles in list
            projectileType = 0;


        while (staticProjectileList[projectileType].phase != currentPhase) //Add 1 until reach desired phase
        {
            projectileType += 1;
        }

        spawnWaitTime = staticProjectileList[projectileType].SpawnFrequency.Evaluate(GameManager.phaseTime) + 0.1f;

        if (staticProjectileList[projectileType].RandomSpawnTime == true)
            spawnWaitTime = spawnWaitTime * UnityEngine.Random.Range(staticProjectileList[projectileType].TimeMin, staticProjectileList[projectileType].TimeMax);

        spawnLoc = new Vector2(spawnLocationX, spawnLocationY);


        if (!spawnList[projectileType] && fighting) //Call spawn if [not spawning is true] and [should fight]
        {
            StartCoroutine(SpawnProjectile(projectileType));
        }

        projectileType += 1;

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

    private void LateUpdate()
    {
        phaseTimer += Time.deltaTime;

        if (phaseTimer >= fightPhaseList[currentPhase].AttackLength)
        {
            phaseTimer = 0;
            currentPhase += 1;
            GameManager.phaseTime = 0;
        }

        if(currentPhase > maxPhases)
        {
            currentPhase = 0;
        }

    }

    public Vector2 SpawnLocation(int loop) //Get Spawn Location
    {
        spawnPos = staticProjectileList[loop].spawnLocation.ToString();
        specificSpawnPos = staticProjectileList[loop].locationSpecific.ToString();

        if (spawnPos == "Random")
        {
            int random = UnityEngine.Random.Range(1, 5);
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
                spawnLocationX = UnityEngine.Random.Range(-0.75f, 0.75f);
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
                spawnLocationX = UnityEngine.Random.Range(-0.75f, 0.75f);
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
                spawnLocationY = UnityEngine.Random.Range(-0.75f, 0.75f);
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
                spawnLocationY = UnityEngine.Random.Range(-0.75f, 0.75f);
            }
            else
            {
                spawnLocationY = staticProjectileList[loop].specificSpawnLocation;
            }

        }

        var coords = new Vector2(spawnLocationX, spawnLocationY);

        return coords;
    }

}
