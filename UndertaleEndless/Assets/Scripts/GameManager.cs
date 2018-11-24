using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public GameObject player;

    public bool isDead;
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

        level = PersistentData.Level;

        player = GameObject.Find("Player");

        maxHealth = 20 + (level - 1) * 4;
        healthBar.maxValue = maxHealth;
        score = 0;
        health = maxHealth;
        isDead = false;
    }

    void Update()
    {
        //DO ALL THE PERSISTENT DATA HERE
        PersistentData.LastDeathLocation = player.transform.position;
        PersistentData.Experience = Mathf.RoundToInt(score);

        if (health <= 0)
        {
            isDead = true;
        }

        if (isDead)
        {
            SceneManager.LoadScene("DeathScreen");
        }

        score += 1.0f * Time.deltaTime;

        healthBar.value = health;

        Level.text = ("LV " + level.ToString());
        Score.text = ("EXP " + Mathf.RoundToInt(score).ToString());
        Health.text = (Mathf.RoundToInt(health).ToString() + "/" + Mathf.RoundToInt(maxHealth).ToString());


        if (health > maxHealth)
            health = maxHealth;



    }

}
