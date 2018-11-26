using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        this.gameObject.transform.position = SaveObject.lastLocation;
        Camera = GameObject.Find("Main Camera");
        audioSource = Camera.GetComponent<AudioSource>();
        audioReverb = Camera.GetComponent<AudioReverbFilter>();

        numberOfDebris = Random.Range(3, 8);

        StartCoroutine(deathSounds());

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
            var position = new Vector2(this.transform.position.x + Random.Range(-0.1f, 0.1f), this.transform.position.y + Random.Range(-0.1f, 0.1f));
            GameObject instance = (GameObject)Instantiate<GameObject>(debrisTemplate, position, rotation); //Instantiate Debris
            debrisList.Add(instance);
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
