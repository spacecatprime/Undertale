using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextLimiter : MonoBehaviour {

    public TextMeshProUGUI title;
    public TMP_InputField mainInputField;
    public string name;
    public GameObject text;
    public Animation imageAnimator;
    public GameObject yes;
    public GameObject no;
    public GameObject Continue;
    public AudioSource audio;
    public GameObject nameOverlay;
    public GameObject overlay;
    public bool nameFocus;

    // Use this for initialization
    void Start () {
        yes.SetActive(false);
        no.SetActive(false);
        Continue.SetActive(true);
        title.text = "Name the fallen human.";
        mainInputField.characterLimit = 6;
        mainInputField.readOnly = false;
        nameOverlay.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if(nameFocus == true)
        {

        }
    }

    public IEnumerator focus()
    {
        yield return new WaitForSeconds(3.0f);
        text.transform.position = new Vector3(0, -385, 0);
    }

    public void Enter()
    {
        if(mainInputField.text.Length > 0)
        {
            nameFocus = true;
            TextShake.shouldShake = true;
            imageAnimator.Play("NameFocus");
            yes.SetActive(true);
            no.SetActive(true);
            Continue.SetActive(false);
            mainInputField.readOnly = true;
            nameOverlay.SetActive(true);
            title.text = "Is this name correct?";
        }
    }

    public void Yes()
    {
        nameFocus = true;
        yes.SetActive(false);
        no.SetActive(false);
        title.text = "";
        name = mainInputField.text;
        PlayerPrefs.SetString("Name", name);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.Save();
        audio.Stop();
        overlay.SetActive(true);
        //PLAY AMIMATION
        StartCoroutine(LoadMenu());
    }

    public void No()
    {
        nameFocus = false;
        TextShake.shouldShake = false;
        imageAnimator.Play("NameUnfocus");
        yes.SetActive(false);
        no.SetActive(false);
        Continue.SetActive(true);
        mainInputField.readOnly = false;
        nameOverlay.SetActive(false);
        title.text = "Name the fallen human.";
        name = "";
        PlayerPrefs.SetString("Name", name);
        PlayerPrefs.Save();
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(5.25f);
        SceneManager.LoadScene("MainMenu");
    }
}
