using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour {

    public AudioSource music;

    // Use this for initialization
    void Start () {
        this.gameObject.SetActive(true);
        StartCoroutine(disable());

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator disable()
    {
        yield return new WaitForSeconds(1f);
        music.Play();
        this.gameObject.SetActive(false);
    }
}
