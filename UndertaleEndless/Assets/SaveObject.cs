using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveObject : MonoBehaviour {

    public static Vector3 lastLocation;

    public float time;

    public void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        time = PlayerPrefs.GetFloat("Time");
    }
	
	// Update is called once per frame
	void Update () {
        if(SceneManager.GetActiveScene().name != "MainMenu" || SceneManager.GetActiveScene().name != "Naming" || SceneManager.GetActiveScene().name != "Start")
        {
            time += Time.unscaledDeltaTime;
            PlayerPrefs.SetFloat("Time", time);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.GetInt("Level") == 0) //set level to 1
        {
            PlayerPrefs.SetInt("Level", 1);
        }
    }

}
