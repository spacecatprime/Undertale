using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heartbreak : MonoBehaviour {

    public AudioClip snap;
    public AudioClip shatter;
    public AudioSource audioSource;
    public AudioReverbFilter audioReverb;

    public GameObject Camera;

    // Use this for initialization
    void Start ()
    {

        this.gameObject.transform.position = PersistentData.LastDeathLocation;
        Camera = GameObject.Find("Main Camera");
        audioSource = Camera.GetComponent<AudioSource>();
        audioReverb = Camera.GetComponent<AudioReverbFilter>();

        StartCoroutine(deathSounds());

    }

    public IEnumerator deathSounds() //Death Sounds
    {
        audioReverb.enabled = false;
        audioSource.clip = snap;
        audioSource.Play();

        yield return new WaitForSeconds(1f);

        audioReverb.enabled = true;
        audioSource.clip = shatter;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
