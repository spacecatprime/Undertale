using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameObject player;

    private float maxHealth;

    public static int healthCondition = 0; //HEAL: -1, NONE: 0, DAMAGE: 1

    public static bool isInvincible;

    public TextMeshProUGUI Score;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Level;
    public Slider healthBar;

    public static float score;
    public static float health;
    public static int level = 1;

    void Start()
    {
        isInvincible = false;

        level = PlayerPrefs.GetInt("Level");

        player = GameObject.Find("Player");

        maxHealth = 20 + (level - 1) * 4;
        healthBar.maxValue = maxHealth;
        score = 0;
        health = maxHealth;

        Level.text = ("LV " + level.ToString());
    }

    void FixedUpdate()
    {
        if (health > maxHealth)
            health = maxHealth;

        if (health < 0) //Dead
        {
            health = 0;
        }

        if (health <= 0)
            Dead();

        Health.text = (Mathf.RoundToInt(health).ToString() + "/" + Mathf.RoundToInt(maxHealth).ToString());
        healthBar.value = health;

    }
    private void LateUpdate()
    {

        score += 1.0f * Time.deltaTime;

        Score.text = ("EXP " + Mathf.RoundToInt(score).ToString());

        
    }

    public static void Dead()
    {
        SaveObject.lastLocation = player.transform.position;
        PlayerPrefs.SetInt("Experience", Mathf.RoundToInt(score));
        SceneManager.LoadScene("DeathScreen");
    }

}
