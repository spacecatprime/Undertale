using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveObject : MonoBehaviour {

    public static bool shouldLoad;
    public float time;
    public int level;
    public int exp;
    public static Vector3 lastLocation;
    public string name;

    public void Awake()
    {
        shouldLoad = true;
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            time += Time.unscaledDeltaTime;
            PlayerPrefs.SetFloat("Time", time);
            PlayerPrefs.Save();
        }


        if (shouldLoad)
            Load();

        if (!PlayerPrefs.HasKey("Name") || PlayerPrefs.GetString("Name") == "") //Check for playername
        {
            name = "PLAYER";
        }
        if (PlayerPrefs.GetInt("Level") == 0) //set level to 1
        {
            level = 1;
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationPause(bool pause)
    {
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetString("Name", name);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Experience", exp);
        PlayerPrefs.SetFloat("Time", time);
        PlayerPrefs.Save();
    }


    public void Load()
    {
        shouldLoad = false;
        time = PlayerPrefs.GetFloat("Time");
        level = PlayerPrefs.GetInt("Level");
        exp = PlayerPrefs.GetInt("Experience");
        name = PlayerPrefs.GetString("Name");
    }

}
