using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public GameObject particles;
    public ParticleSystem ps;
    public float particleAmount = 0;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI LastScore;
    public string score;

    void Start()
    {
        ps = particles.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var emission = ps.emission;
        emission.rateOverTime = particleAmount;
        particleAmount += 0.1f * Time.deltaTime;


        if(ParticleCollider.isDead == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        score = Mathf.RoundToInt(particleAmount * 20).ToString();

        Score.text = ("Score: " + score);
    }


}
