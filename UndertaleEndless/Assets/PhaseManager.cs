using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour {

    public GameObject player;
    public Animator anim;

	// Use this for initialization
	void Start () {
        ProjectileManager.fighting = true;
    }
	
	// Update is called once per frame
	void Update () {
	}

    public static void StaticPause(PhaseManager c)
    {
        c.Pause();
    }

    public static void StaticResume(PhaseManager c)
    {
        c.Resume();
    }

    public void Pause()
    {
        anim.SetBool("Dialogue", true);
        anim.SetBool("Default", false);
        player.SetActive(false);
        player.transform.position = new Vector2(0, 0);
        ProjectileManager.fighting = false;
    }

    public void Resume()
    {
        anim.SetBool("Dialogue", false);
        anim.SetBool("Default", true);
        player.SetActive(true);
        ProjectileManager.fighting = true;
        Debug.Log("resume");
    }
}
