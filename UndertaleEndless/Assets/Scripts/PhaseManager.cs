using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhaseManager : MonoBehaviour {

    public GameObject player;
    public GameObject joystick;
    public GameObject menuOptions;
    public GameObject strikeBar;
    public GameObject dumbTarget;
    public GameObject hitButton;
    public GameObject slash;
    public GameObject damage;
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
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        yield return new WaitForSeconds(0.75f);
        player.SetActive(true);
        ProjectileManager.fighting = true;
    }

    public IEnumerator LatePlayerSetActive()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(0.75f);
        player.SetActive(false);
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
        damageDealt = attackDamage / ((100 + (Mathf.Abs(100 - (pos / 400 * 100)))) / 100)*2.5f;
        damageDealt = Mathf.RoundToInt(damageDealt);
        strikeBar.GetComponent<Animator>().SetBool("HasAttacked", true);
        slash.GetComponent<Animator>().SetBool("Attack", true);

        yield return new WaitForSeconds(0.75f);

        realValue -= damageDealt;
        strikeBar.GetComponent<AudioSource>().Play();
        damageIndicator.text = damageDealt.ToString(); 
        damage.SetActive(true);
        monsterHealth.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        dumbTarget.GetComponent<Animator>().SetTrigger("Fade");
        strikeBar.SetActive(false);
        StartCoroutine(ResumeCoroutine());

        yield return new WaitForSeconds(0.3f);

        player.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        dumbTarget.SetActive(false);
        monsterHealth.SetActive(false);
        damage.SetActive(false);
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

        yield return new WaitForSeconds(0.3f);

        player.SetActive(true);


        yield return new WaitForSeconds(0.4f);

        dumbTarget.SetActive(false);
        miss.SetActive(false);
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
