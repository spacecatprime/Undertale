﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public GameObject player;

    private float maxHealth;

    public static int healthCondition = 0; //HEAL: -1, NONE: 0, DAMAGE: 1

    public static bool isInvincible;

    public TextMeshProUGUI Score;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI Name;
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
    }

    void FixedUpdate()
    {
        if (health <= 0) //Dead
        {
            SaveObject.lastLocation = player.transform.position;
            PlayerPrefs.SetInt("Experience", Mathf.RoundToInt(score));
            SceneManager.LoadScene("DeathScreen");
        }


    }
    private void Update()
    {
        if (health > maxHealth)
            health = maxHealth;

        healthBar.value = health;
        score += 1.0f * Time.deltaTime;
        Level.text = ("LV " + level.ToString());
        Score.text = ("EXP " + Mathf.RoundToInt(score).ToString());
        Name.text = PlayerPrefs.GetString("Name");
        Health.text = (Mathf.RoundToInt(health).ToString() + "/" + Mathf.RoundToInt(maxHealth).ToString());
    }

}
