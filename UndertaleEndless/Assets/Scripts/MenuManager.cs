using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public TextMeshProUGUI name;
    public TextMeshProUGUI time;
    public TextMeshProUGUI level;
    public TextMeshProUGUI detail;
    public TextMeshProUGUI contirnueText;

    // Use this for initialization
    void Start () {
                name.text = PlayerPrefs.GetString("Name");

        int minutes = Mathf.FloorToInt(PlayerPrefs.GetFloat("Time") / 60F);
        int seconds = Mathf.FloorToInt(PlayerPrefs.GetFloat("Time") - minutes * 60);
        time.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        level.text = "LV " + PlayerPrefs.GetInt("Level").ToString();

        if(PlayerPrefs.GetString("HasProfile") == "False")
        {
            contirnueText.text = "New Game";
        }
        else
            contirnueText.text = "Continue";
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void Continue()
    {
        SceneManager.LoadScene("Fight");
    }

    public void Restart()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("Name", "");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Naming");
    }
}
