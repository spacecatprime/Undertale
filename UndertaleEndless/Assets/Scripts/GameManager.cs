using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    private float maxHealth;

    public static int healthCondition = 0; //HEAL: -1, NONE: 0, DAMAGE: 1

    public static bool isInvincible;

    public TextMeshProUGUI Name;
    //public TextMeshProUGUI Score;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Level;
    public Slider healthBar;

    public static float phaseTime;
    public static float health;
    public static int level = 1;

    void Start()
    {
        isInvincible = false;

        level = PlayerPrefs.GetInt("Level");

        maxHealth = 20 + (level - 1) * 4;
        healthBar.maxValue = maxHealth;
        phaseTime = 0;
        health = maxHealth;

        Level.text = ("LV " + level.ToString());

        Name.text = (PlayerPrefs.GetString("Name").ToUpper());

    }

    void Update()
    {
        if (health > maxHealth)
            health = maxHealth;

        if (health < 0) //Dead
        {
            health = 0;
        }

        if (health <= 0)
            Dead();

        Health.text = (health.ToString() + "/" + Mathf.RoundToInt(maxHealth).ToString());
        healthBar.value = health;

    }
    private void LateUpdate()
    {

        phaseTime += 1.0f * Time.deltaTime;

        //Score.text = ("EXP " + Mathf.RoundToInt(score).ToString());

        
    }

    public static void Dead()
    {
        SaveObject.lastLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
        //PlayerPrefs.SetInt("Experience", Mathf.RoundToInt(phaseTime));
        SceneManager.LoadScene("DeathScreen");
    }

}
