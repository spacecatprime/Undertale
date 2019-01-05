using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SpriteShatter;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;
using MoreMountains.NiceVibrations;

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
    public GameObject dustSound;
    public GameObject monsterContinueButton;
    public static GameObject staticMonsterContinueButton;
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
    public GameObject canvas;
    public GameObject bulletBoard;
    public static bool monsterTalking;
    public static bool nextDeathSentence;
    public static bool dustNow;
    public static float defence;
    public static float health;
    public static bool canBeBetrayed;
    public static bool nextBetrayalSentence;
    public static bool nextSentence;
    public static bool shakeHasStopped;
    public static bool isHit;

    // Use this for initialization
    void Start () {
        staticMonsterContinueButton = monsterContinueButton;

        monster.AddComponentExt(ProjectileManager.staticEnemy.enemyPhaseDialogueManager.name);

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

        defence = ProjectileManager.staticEnemy.Def;

        player.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {

        health = realValue;
        AttackStrikeBarMove();

        Lerp();
        MonsterSounds();
        MonsterTalkCheck();
        DustMonsterIfDeadCheck();

        if (realValue < 0)
        {
            realValue = 0;
        }

    }

    private void DustMonsterIfDeadCheck()
    {
        if (dustNow)
        {
            dustNow = false;
            StartCoroutine(Shatter());
        }
    }

    private static void MonsterTalkCheck()
    {
        if (!monsterTalking)
            foreach (GameObject bubble in FlavourTextManager.staticSpeechBubbles)
            {
                bubble.SetActive(false);
            }
    }

    private void MonsterSounds()
    {
        if (shouldPlayDeathSound && !hasPlayedDeathSound)
        {
            dustSound.GetComponent<AudioSource>().Play();
            hasPlayedDeathSound = true;
        }
    }

    private void AttackStrikeBarMove()
    {
        if (strikeMove)
        {
            strikeBar.transform.position = new Vector2(strikeBar.transform.position.x + 500 * Time.deltaTime, strikeBar.transform.position.y);
        }

        if (strikeBar.GetComponent<RectTransform>().transform.position.x > 750 && strikeMove)
        {
            AttackPressed();
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
        StartCoroutine(ResumeCoroutine(false));
    }

    public void Pause()
    {
        anim.SetTrigger("DialogueActivate");
        StartCoroutine(LatePlayerSetActive());
        ProjectileManager.fighting = false;
        FlavourTextManager.shouldShowFT = true;
    }

    public IEnumerator ResumeCoroutine(bool isDead)
    {
        foreach (Button x in buttons)
        {
            x.interactable = false;
        }

        if (!ProjectileManager.fighting)
        {
            GameManager.nextPhaseCalculation = true; //Request for next phase
            anim.SetTrigger("DefaultActivate");
        }
        strikeBar.transform.position = new Vector2(-315, strikeBar.transform.position.y);
        player.transform.position = new Vector2(0, 0);

        yield return new WaitForSeconds(0.25f);

        player.SetActive(true);
        if(!monsterTalking)
        {
            if (!isDead)
            {
                ProjectileManager.currentPhase = GameManager.currentPhase;
                Debug.Log("Starting Phase " + GameManager.currentPhase);
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                ProjectileManager.fighting = true;
            }
            else
            {
                ProjectileManager.fighting = false;
            }
        }

        yield return new WaitForSeconds(0.01f);
        anim.ResetTrigger("DefaultActivate");

    }

    public IEnumerator LatePlayerSetActive()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(0.75f);
        foreach (Button x in buttons)
        {
            x.interactable = true;
        }
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
            damageDealt = Mathf.RoundToInt((attackDamage + weapon - defence + UnityEngine.Random.Range(0, 2)) * 2.2f);
        }
        if(rating < 12)
        {
            damageDealt = Mathf.RoundToInt((attackDamage + weapon - defence + UnityEngine.Random.Range(0, 2)) * ((rating/10)+1));
        }


        strikeBar.GetComponent<Animator>().SetBool("HasAttacked", true);
        slash.GetComponent<Animator>().SetBool("Attack", true);

        yield return new WaitForSeconds(0.75f);

        isHit = true;
        StartCoroutine(Shake());
        realValue -= damageDealt;
        strikeBar.GetComponent<AudioSource>().Play();
        damageIndicator.text = damageDealt.ToString(); 
        damage.SetActive(true);
        monsterHealth.SetActive(true);

        if(realValue <= 0)
        {
            MMVibrationManager.Vibrate();
            monster.GetComponent<AudioSource>().Stop();
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
            StartCoroutine(ResumeCoroutine(false));
        }
        else
        {
            yield return new WaitForSeconds(0.7f);
            StartCoroutine(ResumeCoroutine(true));
            dumbTarget.SetActive(false);
            monsterHealth.SetActive(false);
            damage.SetActive(false);
        }
    }

    public IEnumerator Shatter()
    {
        yield return new WaitForSeconds(0f);
        monster.GetComponent<Shatter>().shatter();
        canvas.SetActive(false);
        bulletBoard.SetActive(false);
        MonsterHeartbreak.isEnemyKilled = true;
    }

    public IEnumerator Miss()
    {
        GameManager.spareCounter += 1;
        strikeBar.GetComponent<Animator>().SetBool("HasAttacked", true);
        slash.GetComponent<Animator>().SetBool("Attack", true);
        Debug.Log("Miss");

        yield return new WaitForSeconds(0.75f);

        miss.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        dumbTarget.GetComponent<Animator>().SetTrigger("Fade");
        strikeBar.SetActive(false);
        StartCoroutine(ResumeCoroutine(false));

        yield return new WaitForSeconds(0.7f);
        dumbTarget.SetActive(false);
        miss.SetActive(false);
    }

    IEnumerator Shake()
    {
        shakeHasStopped = false;
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

        shakeHasStopped = true;
        isHit = false;

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
        GameManager.spareCounter += 1;
        StartCoroutine(ResumeCoroutine(false));
    }

    public void MonsterTalkContinue()
    {
        monsterContinueButton.SetActive(false);
        if (ProjectileManager.enemyKilled && !canBeBetrayed) //neutral kill
        {
            nextDeathSentence = true;
        }
        else if(ProjectileManager.enemyKilled && canBeBetrayed) //betrayal kill
        {
            nextBetrayalSentence = true;
        }
        else if (GameManager.spareCounter >= GameManager.customUnskippableTextRange.x && GameManager.spareCounter <= GameManager.customUnskippableTextRange.y)
        {
            nextSentence = true;
        }
        else
        {
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            monsterTalking = false;
            ResumeCoroutine(realValue <= 0);
            monsterContinueButton.SetActive(false);
            GameManager.currentPhase -= 1;
            StartCoroutine(ResumeCoroutine(ProjectileManager.enemyKilled));
        }
    }

}


















public static class ExtensionMethod
{
    public static Component AddComponentExt(this GameObject obj, string scriptName)
    {
        Component cmpnt = null;


        for (int i = 0; i < 10; i++)
        {
            //If call is null, make another call
            cmpnt = _AddComponentExt(obj, scriptName, i);

            //Exit if we are successful
            if (cmpnt != null)
            {
                break;
            }
        }


        //If still null then let user know an exception
        if (cmpnt == null)
        {
            Debug.LogError("Failed to Add Component");
            return null;
        }
        return cmpnt;
    }

    private static Component _AddComponentExt(GameObject obj, string className, int trials)
    {
        //Any script created by user(you)
        const string userMadeScript = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        //Any script/component that comes with Unity such as "Rigidbody"
        const string builtInScript = "UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        //Any script/component that comes with Unity such as "Image"
        const string builtInScriptUI = "UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        //Any script/component that comes with Unity such as "Networking"
        const string builtInScriptNetwork = "UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        //Any script/component that comes with Unity such as "AnalyticsTracker"
        const string builtInScriptAnalytics = "UnityEngine.Analytics, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        //Any script/component that comes with Unity such as "AnalyticsTracker"
        const string builtInScriptHoloLens = "UnityEngine.HoloLens, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        Assembly asm = null;

        try
        {
            //Decide if to get user script or built-in component
            switch (trials)
            {
                case 0:

                    asm = Assembly.Load(userMadeScript);
                    break;

                case 1:
                    //Get UnityEngine.Component Typical component format
                    className = "UnityEngine." + className;
                    asm = Assembly.Load(builtInScript);
                    break;
                case 2:
                    //Get UnityEngine.Component UI format
                    className = "UnityEngine.UI." + className;
                    asm = Assembly.Load(builtInScriptUI);
                    break;

                case 3:
                    //Get UnityEngine.Component Video format
                    className = "UnityEngine.Video." + className;
                    asm = Assembly.Load(builtInScript);
                    break;

                case 4:
                    //Get UnityEngine.Component Networking format
                    className = "UnityEngine.Networking." + className;
                    asm = Assembly.Load(builtInScriptNetwork);
                    break;
                case 5:
                    //Get UnityEngine.Component Analytics format
                    className = "UnityEngine.Analytics." + className;
                    asm = Assembly.Load(builtInScriptAnalytics);
                    break;

                case 6:
                    //Get UnityEngine.Component EventSystems format
                    className = "UnityEngine.EventSystems." + className;
                    asm = Assembly.Load(builtInScriptUI);
                    break;

                case 7:
                    //Get UnityEngine.Component Audio format
                    className = "UnityEngine.Audio." + className;
                    asm = Assembly.Load(builtInScriptHoloLens);
                    break;

                case 8:
                    //Get UnityEngine.Component SpatialMapping format
                    className = "UnityEngine.VR.WSA." + className;
                    asm = Assembly.Load(builtInScriptHoloLens);
                    break;

                case 9:
                    //Get UnityEngine.Component AI format
                    className = "UnityEngine.AI." + className;
                    asm = Assembly.Load(builtInScript);
                    break;
            }
        }
        catch (Exception e)
        {
            //Debug.Log("Failed to Load Assembly" + e.Message);
        }

        //Return if Assembly is null
        if (asm == null)
        {
            return null;
        }

        //Get type then return if it is null
        Type type = asm.GetType(className);
        if (type == null)
            return null;

        //Finally Add component since nothing is null
        Component cmpnt = obj.AddComponent(type);
        return cmpnt;
    }
}
