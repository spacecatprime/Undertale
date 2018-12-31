using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHeartbreak : MonoBehaviour {

    [Header("Shake")]
    public static bool isEnemyKilled;
    public bool alreadyAtMonster;
    public GameObject monster;
    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_intensity;
    public bool shattered;
    public Sprite snapped;


    [Header("Audio")]
    public AudioClip snap;
    public AudioClip shatter;
    public AudioSource audioSource;
    public AudioReverbFilter audioReverb;

    public List<GameObject> debrisList;

    public GameObject debrisTemplate;

    public int numberOfDebris;
    public bool currentlySwaping;

    private void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    void Update()
    {
        if(isEnemyKilled && !alreadyAtMonster)
        {
            alreadyAtMonster = true;
            this.gameObject.transform.position = monster.transform.position;
            originPosition = this.gameObject.transform.position;
        }

        if (shake_intensity > 0 && isEnemyKilled)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, this.gameObject.GetComponent<SpriteRenderer>().color.a + 0.25f* Time.deltaTime);
            transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
        }

        if(this.gameObject.GetComponent<SpriteRenderer>().color.a >= 1 && !shattered)
        {
            shattered = true;
            StartCoroutine(DeathSounds());
            shake_intensity = 0;
        }

        foreach (GameObject debris in debrisList)
        {
            debris.SetActive(true);
        }
    }

    public IEnumerator DeathSounds() //Death Sounds
    {
        SaveObject.lastLocation = this.gameObject.transform.position;
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = snapped;
        audioSource.loop = false;
        audioReverb.enabled = false;
        audioSource.clip = snap;
        audioSource.Play();
        yield return new WaitForSeconds(1.5f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        spawnDebris();
        audioReverb.enabled = true;
        audioSource.clip = shatter;
        audioSource.Play();
    }

    void spawnDebris()
    {
        for (int i = 0; i < numberOfDebris; i++)
        {
            var rotation = new Quaternion(0, 0, 0, 0);
            var position = new Vector2(this.transform.position.x + Random.Range(-0.15f, 0.15f), this.transform.position.y + Random.Range(-0.15f, 0.15f));
            GameObject instance = (GameObject)Instantiate<GameObject>(debrisTemplate, position, rotation); //Instantiate Debris
            debrisList.Add(instance);
            instance.transform.parent = gameObject.transform;
        }
    }
}
