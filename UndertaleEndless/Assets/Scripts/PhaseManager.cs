using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseManager : MonoBehaviour {

    public GameObject player;
    public GameObject joystick;
    public GameObject menuOptions;
    public GameObject strikeBar;
    public GameObject dumbTarget;
    public GameObject hitButton;
    public GameObject slash;
    public Animator anim;
    public bool strikeMove;
    public Button[] buttons;

    // Use this for initialization
    void Start () {
        buttons = menuOptions.GetComponentsInChildren<Button>();
        hitButton.SetActive(false);
        ProjectileManager.fighting = true;
    }
	
	// Update is called once per frame
	void Update () {

        if(strikeMove)
        {
            strikeBar.transform.position = new Vector2(strikeBar.transform.position.x + 5, strikeBar.transform.position.y);
        }

        if (strikeBar.GetComponent<RectTransform>().transform.position.x > 700 && strikeMove)
        {
            StartCoroutine(Miss());
        }

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
        foreach (Button x in buttons)
        {
            x.interactable = true;
        }

        anim.SetBool("Dialogue", true);
        anim.SetBool("Default", false);
        player.SetActive(false);
        player.transform.position = new Vector2(0, 0);
        ProjectileManager.fighting = false;
    }

    public void Resume()
    {
        foreach (Button x in buttons)
        {
            x.interactable = false;
        }
        anim.SetBool("Dialogue", false);
        anim.SetBool("Default", true);
        player.SetActive(true);
        ProjectileManager.fighting = true;
        Debug.Log("resume");
    }

    public void Fight()
    {
        foreach (Button x in buttons)
        {
            x.interactable = false;
        }
        hitButton.SetActive(true);
        dumbTarget.SetActive(true);
        strikeBar.SetActive(true);
        strikeBar.GetComponent<RectTransform>().transform.position = new Vector2(-315, 666.5f);
        strikeMove = true;
    }

    public void Item()
    {

    }

    public void Act()
    {

    }

    public void Mercy()
    {

    }

    public void AttackPressed()
    {
        hitButton.SetActive(false);
        strikeMove = false;

        if (strikeBar.GetComponent<RectTransform>().transform.position.x < 700 && strikeBar.GetComponent<RectTransform>().transform.position.x > 0)
            StartCoroutine(Hit(strikeBar.GetComponent<RectTransform>().transform.position.x));
        else
            StartCoroutine(Miss());
    }

    public IEnumerator Hit(float pos)
    {
        strikeBar.GetComponent<Animator>().SetBool("HasAttacked", true);
        slash.GetComponent<Animator>().SetBool("Attack", true);
        Debug.Log("hit");
        yield return new WaitForSeconds(1f);
        dumbTarget.SetActive(false);
        strikeBar.SetActive(false);
        Resume();
    }
    public IEnumerator Miss()
    {
        Debug.Log("miss");
        yield return new WaitForSeconds(1f);
        dumbTarget.SetActive(false);
        strikeBar.SetActive(false);
        Resume();
    }
}
