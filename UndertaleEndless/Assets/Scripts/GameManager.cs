using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public bool isDead;
    private float maxHealth;

    public TextMeshProUGUI Score;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Level;
    public Slider healthBar;

    public static float score;
    public static float health;
    public static int level = 1;

    void Start()
    {
        maxHealth = 20 + (level - 1) * 4;
        healthBar.maxValue = maxHealth;
        score = 0;
        health = maxHealth;
        isDead = false;
    }

    void Update()
    {
        score += 1.0f * Time.deltaTime;

        healthBar.value = health;

        Level.text = ("LV " + level.ToString());
        Score.text = ("EXP " + Mathf.RoundToInt(score).ToString());
        Health.text = (Mathf.RoundToInt(health).ToString() + "/" + Mathf.RoundToInt(maxHealth).ToString());

        if (health <= 0)
        {
            isDead = true;
        }

        if (health > maxHealth)
            health = maxHealth;

        if (isDead == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }


}
