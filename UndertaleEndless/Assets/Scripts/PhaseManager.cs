using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SpriteShatter;

public class PhaseManager : MonoBehaviour {

    public GameObject player;
    public GameObject joystick;
    public GameObject menuOptions;
    public GameObject strikeBar;
    public GameObject dumbTarget;
    public GameObject hitButton;
    public GameObject slash;
    public GameObject damage;
    public GameObject monster;
    public GameObject enemyUI;
    public TextMeshProUGUI damageIndicator;
    public GameObject monsterHealth;
    public Slider monsterHealthSlider;
    public Animator anim;
    public bool strikeMove;
    public GameObject miss;
    private Button[] buttons;
    public float damageDealt;
    private int attackDamage;
    public float newSliderValue;
    public float monsterMaxHP;
    public float lerpSpeed;
    public float realValue;
    public Vector2 enemyOriginalPos;
    public float shakeAmount;
    public static bool shouldPlayDeathSound;
    public bool hasPlayedDeathSound;


    // Use this for initialization
    void Start () {
        monsterMaxHP = ProjectileManager.monsterHealthInit;
        monsterHealthSlider.maxValue = monsterMaxHP;
        monsterHealthSlider.value = monsterMaxHP;
        realValue = monsterMaxHP;
        attackDamage = 10 + ((PlayerPrefs.GetInt("Level") - 1) * 2);
        buttons = menuOptions.GetComponentsInChildren<Button>();
        hitButton.SetActive(false);
        ProjectileManager.fighting = false;
        enemyOriginalPos = monster.transform.position;

        foreach (Button x in buttons)
        {
            x.interactable = true;
        }

        player.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (strikeMove)
        {
            strikeBar.transform.position = new Vector2(strikeBar.transform.position.x + 500*Time.deltaTime, strikeBar.transform.position.y);
        }

        if (strikeBar.GetComponent<RectTransform>().transform.position.x > 750 && strikeMove)
        {
            AttackPressed();
        }

        Lerp();

        if(shouldPlayDeathSound && !hasPlayedDeathSound)
        {
            enemyUI.GetComponent<AudioSource>().Play();
            hasPlayedDeathSound = true;
        }


    }

    void Lerp()
    {
        monsterHealthSlider.value = Mathf.Lerp(monsterHealthSlider.value, realValue, lerpSpeed * Time.deltaTime);
    }

    public static void StaticPause(PhaseManager c)
    {
        c.Pause();
    }

    public static void StaticResume(PhaseManager c)
    {
        c.Resume();
    }

    public void Resume()
    {
        StartCoroutine(ResumeCoroutine());
    }

    public void Pause()
    {
        foreach (Button x in buttons)
        {
            x.interactable = true;
        }

        anim.SetTrigger("DialogueActivate");
        StartCoroutine(LatePlayerSetActive());
        ProjectileManager.fighting = false;
        FlavourTextManager.shouldShowFT = true;
    }

    public IEnumerator ResumeCoroutine()
    {
        strikeBar.transform.position = new Vector2(-315, strikeBar.transform.position.y);
        foreach (Button x in buttons)
        {
            x.interactable = false;
        }
        anim.SetTrigger("DefaultActivate");
        player.transform.position = new Vector2(0, 0);
        yield return new WaitForSeconds(0.75f);
        player.SetActive(true);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        ProjectileManager.fighting = true;
    }

    public IEnumerator LatePlayerSetActive()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(0.75f);
        player.SetActive(false);
        player.transform.position = new Vector2(0, 0);
    }


    public void StartAttack()
    {
        foreach (Button x in buttons)
        {
            x.interactable = false;
        }
        hitButton.SetActive(true);
        dumbTarget.SetActive(true);
        strikeBar.SetActive(true);
        strikeBar.GetComponent<RectTransform>().transform.position = new Vector2(-200, 666.5f);
        strikeMove = true;
    }

    public void AttackPressed()
    {
        slash.GetComponent<AudioSource>().Play();
        hitButton.SetActive(false);
        strikeMove = false;

        if (strikeBar.GetComponent<RectTransform>().transform.position.x < 700 && strikeBar.GetComponent<RectTransform>().transform.position.x > 60)
            StartCoroutine(Hit(strikeBar.GetComponent<RectTransform>().transform.position.x));
        else
            StartCoroutine(Miss());
    }

    public IEnumerator Hit(float pos)
    {
        //TempStats

        float weapon = 1;
        float rating = 0;

        if (pos > 375)
            rating = (((-pos) + 750) / 25) - 2;
        else
            rating = (pos / 25) - 2;
            
        if (rating >= 12)
        {
            damageDealt = Mathf.RoundToInt((attackDamage + weapon - ProjectileManager.staticEnemy.Def + Random.Range(0, 2)) * 2.2f);
        }
        if(rating < 12)
        {
            damageDealt = Mathf.RoundToInt((attackDamage + weapon - ProjectileManager.staticEnemy.Def + Random.Range(0, 2)) * ((rating/10)+1));
        }


        strikeBar.GetComponent<Animator>().SetBool("HasAttacked", true);
        slash.GetComponent<Animator>().SetBool("Attack", true);

        yield return new WaitForSeconds(0.75f);

        StartCoroutine(Shake());
        realValue -= damageDealt;
        strikeBar.GetComponent<AudioSource>().Play();
        damageIndicator.text = damageDealt.ToString(); 
        damage.SetActive(true);
        monsterHealth.SetActive(true);

        if(realValue <= 0)
        {
            monster.GetComponent<AudioSource>().Stop();
            monster.GetComponent<SpriteRenderer>().sprite = ProjectileManager.staticEnemy.ReallyHurt;
            ProjectileManager.enemyKilled = true;
        }

        yield return new WaitForSeconds(0.5f);

        dumbTarget.GetComponent<Animator>().SetTrigger("Fade");
        strikeBar.SetActive(false);
        if(!ProjectileManager.enemyKilled)
        {
            yield return new WaitForSeconds(0.7f);
            dumbTarget.SetActive(false);
            monsterHealth.SetActive(false);
            damage.SetActive(false);
            StartCoroutine(ResumeCoroutine());
        }
        else
        {
            yield return new WaitForSeconds(0.7f);
            dumbTarget.SetActive(false);
            monsterHealth.SetActive(false);
            damage.SetActive(false);
            yield return new WaitForSeconds(2f);
            monster.GetComponent<Shatter>().shatter();
        }

    }

    public IEnumerator Miss()
    {
        strikeBar.GetComponent<Animator>().SetBool("HasAttacked", true);
        slash.GetComponent<Animator>().SetBool("Attack", true);
        Debug.Log("Miss");

        yield return new WaitForSeconds(0.75f);

        miss.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        dumbTarget.GetComponent<Animator>().SetTrigger("Fade");
        strikeBar.SetActive(false);
        StartCoroutine(ResumeCoroutine());

        yield return new WaitForSeconds(0.7f);
        dumbTarget.SetActive(false);
        miss.SetActive(false);
    }

    IEnumerator Shake()
    {
        shakeAmount = 0.75f;
        float shakeRecovery = 0.5f;

        if ((monsterHealthSlider.value - damageDealt) <= 0)
        {
            shakeAmount = 1.0f;
            shakeRecovery = 0.25f;
        }


        while (shakeAmount > 0)
        {
            if (shakeAmount < 0.025f)
                shakeAmount = 0;
            shakeAmount = Mathf.Lerp(shakeAmount, 0, shakeRecovery);
            yield return new WaitForSeconds(0.1f);
            monster.transform.position = new Vector2(enemyOriginalPos.x + Mathf.Abs(shakeAmount), enemyOriginalPos.y);
            yield return new WaitForSeconds(0.1f);
            monster.transform.position = new Vector2(enemyOriginalPos.x - Mathf.Abs(shakeAmount), enemyOriginalPos.y);
        }

        if(ProjectileManager.enemyKilled)
        {
            monster.GetComponent<SpriteRenderer>().sprite = ProjectileManager.staticEnemy.DeathSprite;
        }
    }

    public void Fight()
    {
        FlavourTextManager.fight = true;
        StartAttack();
    }

    public void Item()
    {
        FlavourTextManager.item = true;
    }

    public void Act()
    {
        FlavourTextManager.act = true;
    }

    public void Mercy()
    {
        FlavourTextManager.mercy = true;
    }
}
