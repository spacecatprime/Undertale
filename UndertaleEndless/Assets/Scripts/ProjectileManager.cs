using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {

    public GameObject projectileTemplate;
    public GameObject player;

    public List<Projectile> projectilePropertiesList;
    public static List<Projectile> staticProjectileList;

    public List<bool> spawnList;

    public float spawnWaitTime;

    private float spawnLocationY;
    private float spawnLocationX;

    public int loop = 0;

    public string spawnPos;
    public Vector2 spawnLoc;
    public Quaternion spawnRot;

    private void Awake() //Add all Projectile Scriptable Objects to List
    {
        foreach (Projectile x in Resources.FindObjectsOfTypeAll(typeof(Projectile)) as Projectile[])
        {
            projectilePropertiesList.Add(x);
            spawnList.Add(false);
        }

        staticProjectileList = projectilePropertiesList; //Add list to static list
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

    public Vector2 SpawnLocation(int loop) //Get Spawn Location
    {
        spawnPos = staticProjectileList[loop].spawnLocation.ToString();
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
            spawnRot = Quaternion.Euler(new Vector3(0, 0, 0));
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

        var coords = new Vector2(spawnLocationX, spawnLocationY);

        return coords;
    }

}
