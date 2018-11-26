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
    public GameObject overlay;
    public Animation overlayAnimator;

    // Use this for initialization
    void Start () {
        yes.SetActive(false);
        no.SetActive(false);
        Continue.SetActive(true);
        title.text = "Name the fallen human.";
        mainInputField.characterLimit = 6;
        mainInputField.characterValidation = TMP_InputField.CharacterValidation.Name;
        mainInputField.ActivateInputField();
        mainInputField.readOnly = false;
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void Enter()
    {
        imageAnimator.Play("NameFocus");
        yes.SetActive(true);
        no.SetActive(true);
        Continue.SetActive(false);
        mainInputField.readOnly = true;
        title.text = "Is this name correct?";
    }

    public void Yes()
    {
        yes.SetActive(false);
        no.SetActive(false);
        title.text = "";
        name = mainInputField.text;
        PlayerPrefs.SetString("Name", name);
        PlayerPrefs.Save();
        audio.Stop();
        overlay.SetActive(true);
        //PLAY AMIMATION
        StartCoroutine(LoadMenu());
    }

    public void No()
    {
        imageAnimator.Play("NameUnfocus");
        yes.SetActive(false);
        no.SetActive(false);
        Continue.SetActive(true);
        mainInputField.readOnly = false;
        title.text = "Name the fallen human.";
        name = "";
        PlayerPrefs.SetString("Name", name);
        PlayerPrefs.Save();
        overlayAnimator.Play("OverlayFocus");
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(5.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
