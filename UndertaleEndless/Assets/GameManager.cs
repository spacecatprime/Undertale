using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public bool isDead;
    public float bulletAmount = 0;
    public TextMeshProUGUI Score;
    public float score;

    void Start()
    {

    }

    void Update()
    {
        score += 1.0f * Time.deltaTime;


        if(isDead == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Score.text = ("Score: " + Mathf.RoundToInt(score).ToString());
    }


}
