﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;


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
    public bool shaking;
    public bool shouldShake;


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
            SaveObject.monsterLocation = originPosition;
        }


        if (shake_intensity > 0 && isEnemyKilled)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, this.gameObject.GetComponent<SpriteRenderer>().color.a + 0.25f * Time.deltaTime);
            if(!shaking)
            {
                shouldShake = true;
                StartCoroutine(Shake());
            }
        }


        if(this.gameObject.GetComponent<SpriteRenderer>().color.a >= 1 && !shattered)
        {
            shouldShake = false;
            transform.position = originPosition;
            shattered = true;
            StartCoroutine(DeathSounds());
            shake_intensity = 0;
        }

        foreach (GameObject debris in debrisList)
        {
            debris.SetActive(true);
        }
    }

    public IEnumerator Shake()
    {
        shaking = true;
        while (shouldShake)
        {
            transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator DeathSounds() //Death Sounds
    {
        yield return new WaitForSeconds(1f);
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = snapped;
        audioSource.loop = false;
        audioReverb.enabled = false;
        audioSource.clip = snap;
        audioSource.Play();
        yield return new WaitForSeconds(1.5f);
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
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
