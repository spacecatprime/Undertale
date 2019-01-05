using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileManager : MonoBehaviour {

    public Enemy enemy;
    public GameObject phaseManager;
    public static Enemy staticEnemy;
    public static bool enemyKilled;
    public static bool fighting;
    public GameObject projectileTemplate;
    public GameObject player;
    public SpriteRenderer enemyImage;
    public SpriteRenderer battleBG;
    public AudioSource music;
    public List<FightPhase> fightPhaseList;
    public List<Projectile> projectilePropertiesList;
    public static List<Projectile> staticProjectileList;
    public static int currentPhase;
    public List<bool> spawnList;
    public GameObject box;
    public static bool endOnDamage;
    public static float phaseTimer;
    public static float monsterHealthInit;

    private int maxPhases = 0;
    private float spawnWaitTime;
    private float spawnLocationY;
    private float spawnLocationX;
    private int projectileType = 0;
    private string spawnPos;
    private string specificSpawnPos;
    private Vector2 spawnLoc;
    private Quaternion spawnRot;

    private void Awake() //Add all Projectile Scriptable Objects to List
    {
        battleBG.sprite = enemy.Background;


        if (enemy.Background != null)
            battleBG.color = new Color(1.0f, 1.0f, 1.0f);



        foreach (FightPhase x in enemy.Phases) //Proccess enemy phases
        {
            
            foreach (Projectile y in x.ProjectileCombo)
            {
                projectilePropertiesList.Add(y);
                spawnList.Add(false); //add spawning regulator bool
            }
            fightPhaseList.Add(x);
            maxPhases += 1;
        }

        maxPhases -= 1;



        staticEnemy = enemy;

        staticProjectileList = projectilePropertiesList;

        monsterHealthInit = enemy.HP;

        music.clip = enemy.bossMusic;
        music.Play();
    }

    void Update ()
    {

        StopPhaseProjectileOverflow();


        //Make sure only spawn in projectile in the specified phase
        CheckForCorrectProjectile();

        //Wait time for spawning
        WaitTimeForSpawning();

        spawnLoc = new Vector2(spawnLocationX, spawnLocationY);

        //Call spawn if [not already spawning] and "[should fight]"
        if (!spawnList[projectileType] && fighting)
        {
            StartCoroutine(SpawnProjectile(projectileType));
        }

        projectileType += 1;

        endOnDamage = fightPhaseList[currentPhase].PhaseEndsOnDamage;

    }

    private void WaitTimeForSpawning()
    {
        if (staticProjectileList[projectileType].RandomSpawnFrequency == true)
            spawnWaitTime = UnityEngine.Random.Range(staticProjectileList[projectileType].timeSpawnRange.x, staticProjectileList[projectileType].timeSpawnRange.y);
        else
            spawnWaitTime = staticProjectileList[projectileType].SpawnFrequency.Evaluate(GameManager.phaseTime);
    }

    private void CheckForCorrectProjectile()
    {
        while (!fightPhaseList[currentPhase].ProjectileCombo.Contains(staticProjectileList[projectileType]) && fighting)
        {
            projectileType += 1;
            if (projectileType == (staticProjectileList.Count - 1))
            {
                projectileType = 0;
            }
        }
    }

    private void StopPhaseProjectileOverflow()
    {
        if (projectileType >= staticProjectileList.Count) //Make sure spawning projectile type never goes above max projectiles in list
            projectileType = 0;

        if (currentPhase >= fightPhaseList.Count) //Make sure phase never goes above phaseMax
            currentPhase = 0;
    }

    IEnumerator SpawnProjectile(int Class) //Spawning Sequence
    {
        spawnList[Class] = true;
        yield return new WaitForSeconds(spawnWaitTime);

        if (staticProjectileList[Class].spawnDeadTime > 0)
        {
            StartCoroutine(DeadTimer(Class, staticProjectileList[Class].spawnDeadTime));
        }
        else
        {
            spawnList[Class] = false;
        }

        GameObject instance = (GameObject)Instantiate<GameObject>(projectileTemplate, SpawnLocation(Class), spawnRot); //Instantiate Projectile
        var instanceSprite = instance.GetComponent<SpriteRenderer>();

        instance.SetActive(true);

        instance.transform.localScale = new Vector3(instance.transform.localScale.x, instance.transform.localScale.y, Class);
    }

    IEnumerator DeadTimer(int Class, float deadTime)
    {
        yield return new WaitForSeconds(deadTime);
        spawnList[Class] = false;
    }

    private void LateUpdate()
    {
        if(fighting)
            phaseTimer += Time.deltaTime;

        if (phaseTimer >= fightPhaseList[currentPhase].AttackLength || phaseTimer < 0)
        {
            PhaseManager.StaticPause(phaseManager.GetComponent<PhaseManager>()); //Stop fight

            phaseTimer = 0;
            GameManager.phaseTime = 0;
            GameManager.totalPhases += 1;
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

        if(spawnPos == "Specific")
        {
            spawnRot = Quaternion.Euler(new Vector3(0, 0, 0));

            if (specificSpawnPos == "SpecificXY")
            {
                spawnLocationX = staticProjectileList[loop].specificSpawnX;
                spawnLocationY = staticProjectileList[loop].specificSpawnY;
            }
            else
            {
                if (specificSpawnPos == "SpecificX")
                {
                    spawnLocationX = staticProjectileList[loop].specificSpawnX;
                    spawnLocationY = UnityEngine.Random.Range(-0.75f, 0.75f);
                }
                if (specificSpawnPos == "SpecificY")
                {
                    spawnLocationY = staticProjectileList[loop].specificSpawnY;
                    spawnLocationX = UnityEngine.Random.Range(-0.75f, 0.75f);
                }

            }
        }

        else if (spawnPos == "Top")
        {
            spawnLocationY = 1.0f;
            spawnRot = Quaternion.Euler(new Vector3(0, 0, -180));


            if (specificSpawnPos == "None")
            {
                spawnLocationX = UnityEngine.Random.Range(-0.75f, 0.75f);
            }
            else
            {
                spawnLocationX = staticProjectileList[loop].specificSpawnX;
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
                spawnLocationX = staticProjectileList[loop].specificSpawnX;
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
                spawnLocationY = staticProjectileList[loop].specificSpawnY;
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
                spawnLocationY = staticProjectileList[loop].specificSpawnY;
            }
        }

        var coords = new Vector2(spawnLocationX, spawnLocationY);

        return coords;
    }

}
