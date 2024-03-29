﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using MoreMountains.NiceVibrations;

public class Heartbreak : MonoBehaviour {

    [Header("Audio")]
    public AudioClip snap;
    public AudioClip shatter;
    public AudioClip gameOverMus;
    public AudioSource audioSource;
    public AudioReverbFilter audioReverb;

    public List<GameObject> debrisList;

    public GameObject Camera;
    public GameObject debrisTemplate;

    public int numberOfDebris;
    public bool currentlySwaping;

    // Use this for initialization
    void Start ()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Camera = GameObject.Find("Main Camera");
        audioSource = Camera.GetComponent<AudioSource>();
        audioReverb = Camera.GetComponent<AudioReverbFilter>();

        numberOfDebris = 6;

        StartCoroutine(deathSounds());
        
        if (ProjectileManager.enemyKilled)
        {
            this.gameObject.transform.position = SaveObject.monsterLocation;
        }
        else
        {
            this.gameObject.transform.position = SaveObject.playerLocation;
        }
    }

    public void Update()
    {

        foreach(GameObject debris in debrisList)
        {
            debris.SetActive(true);
        }
    }

    public IEnumerator deathSounds() //Death Sounds
    {
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
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
        yield return new WaitForSeconds(1.5f);
        audioSource.loop = true;
        audioSource.clip = gameOverMus;
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

    public void ContinuePressed()
    {
        StartCoroutine(Continue());
    }

    public IEnumerator Continue()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Fight");
    }

    public void MenuPressed()
    {
        StartCoroutine(Menu());
    }

    public IEnumerator Menu()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
