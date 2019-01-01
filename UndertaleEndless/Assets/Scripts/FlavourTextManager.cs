using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlavourTextManager : MonoBehaviour
{
    public Enemy enemy;
    public AudioSource textSound;
    public AudioSource healSound;
    public TextMeshProUGUI flavourText;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    public List<Button> actionButtons;
    public List<Button> itemButtons;
    public GameObject continueButton;
    public GameObject phaseManager;
    public List<GameObject> speechBubbles;
    public static List<GameObject> staticSpeechBubbles;

    public string currentSentence;
    public string encounter;
    public string check;
    public List<string> neutral;
    public List<string> talk;
    public List<string> genocide;
    public List<HealingItem> healItems;
    public static bool shouldShowFT;

    public static bool fight;
    public static bool act;
    public static bool item;
    public static bool mercy;

    public string entireTag;
    //public static List<string> enemyEntireTag;

    // Use this for initialization
    void Start()
    {
        enemy = ProjectileManager.staticEnemy;

        encounter = enemy.enemyDialogue.Encounter;
        check = enemy.enemyDialogue.Check;
        neutral = enemy.enemyDialogue.NeutralFlavourText;
        talk = enemy.enemyDialogue.Talk;

        StartCoroutine(TypeSentence(encounter));

        staticSpeechBubbles = speechBubbles;

        RecalcItems();
    }

    void RecalcItems()
    {
        leftText.text = "";
        rightText.text = "";

        var count = 3;

        if (healItems.Count > 3)
        {
            count = 3;
        }
        else
            count = healItems.Count;

        for (int i = 0; i < count; i++)
        {
            string fullItem = "* " + healItems[i].shortName + "\n";
            //t
            leftText.text += fullItem;
        }

        for (int i = 3; i < healItems.Count; i++)
        {
            string fullItem = "* " + healItems[i].shortName + "\n";

            rightText.text += fullItem;
        }
    }

    public IEnumerator ShowFlavourText()
    {
        shouldShowFT = false;
        yield return new WaitForSeconds(0.5f);
        flavourText.gameObject.SetActive(true);
        string sentence = enemy.enemyDialogue.NeutralFlavourText[Random.Range(0, enemy.enemyDialogue.NeutralFlavourText.Count)];
        StartCoroutine(TypeSentence(sentence));
    }

    private void Update()
    {
        if(!ProjectileManager.fighting && shouldShowFT)
        {
            StartCoroutine(ShowFlavourText());
        }

        if (fight)
        {
            flavourText.gameObject.SetActive(false);
            Fight();
            fight = false;
        }

        if (act)
        {
            flavourText.gameObject.SetActive(false);
            Act();
            act = false;
        }

        if (item)
        {
            flavourText.gameObject.SetActive(false);
            Item();
            item = false;
        }

        if (mercy)
        {
            flavourText.gameObject.SetActive(false);
            Mercy();
            mercy = false;
        }
    }

    void ClearDialogueBox()
    {
        leftText.gameObject.SetActive(false);
        rightText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Fight()
    {
        ClearDialogueBox();
    }

    void Act()
    {
        ClearDialogueBox();
    }

    void Item()
    {
        ClearDialogueBox();
        RecalcItems();
        leftText.gameObject.SetActive(true);
        rightText.gameObject.SetActive(true);
    }

    void Mercy()
    {
        ClearDialogueBox();
    }

    IEnumerator TypeSentence(string sentence)
    {
        flavourText.text = "";
        yield return new WaitForSeconds(0.25f);
        flavourText.text = "* ";
        yield return new WaitForSeconds(0.05f);

        int i = 0;


        foreach (char letter in sentence)
        {
            string letterStr = letter.ToString();
            
            if (letterStr == "<") //Checking for tags
            {

                while(i < 100)
                {
                    string tagStr = sentence[i].ToString();

                    entireTag += tagStr;
                    i += 1;

                    if (tagStr == ">")
                    {
                        break; //Closing tag
                    }

                } //After entire tag is entered

                Debug.Log(entireTag);
                flavourText.text += entireTag;

            }

            if (entireTag.Length > 0)
            {
                entireTag = entireTag.Substring(0, entireTag.Length - 1);
                continue;
            }




            if (letterStr == "%") //New Line
            {
                flavourText.text += "\n";
            }
            else if (letterStr == "!" || letterStr == "," || letterStr == ".")    // if ! or , wait
            {
                flavourText.text += letterStr;
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                if (letterStr != " ")
                    textSound.Play();
                flavourText.text += letterStr;
                yield return new WaitForSeconds(0.05f); //Time between letters
            }

            i += 1;

        }
    }


    public void Item1() //temp healing items
    {
        GameManager.health += healItems[0].healingAmount;
        ItemContinue(healItems[0].healingAmount, 0);
    }

    public void Item2()
    {
        GameManager.health += healItems[1].healingAmount;
        ItemContinue(healItems[1].healingAmount, 1);
    }

    public void Item3()
    {
        GameManager.health += healItems[2].healingAmount;
        ItemContinue(healItems[2].healingAmount, 2);
    }

    public void Item4()
    {
        GameManager.health += healItems[3].healingAmount;
        ItemContinue(healItems[3].healingAmount, 3);
    }

    public void Item5()
    {
        GameManager.health += healItems[4].healingAmount;
        ItemContinue(healItems[4].healingAmount, 4);
    }

    public void Item6()
    {
        GameManager.health += healItems[5].healingAmount;
        ItemContinue(healItems[5].healingAmount, 5);
    }

    void ItemContinue(int healthGained, int itemNumber)
    {
        healSound.Play();

        itemButtons[healItems.Count-1].interactable = false;

        foreach (Button x in actionButtons)
        {
            x.interactable = false;
        }
        continueButton.SetActive(true);

        leftText.gameObject.SetActive(false);
        rightText.gameObject.SetActive(false);
        flavourText.gameObject.SetActive(true);

        string healString = "";
        
        if (Mathf.RoundToInt(GameManager.health) >= Mathf.RoundToInt(GameManager.maxHealth))
        {
            healString = "You ate the " + healItems[itemNumber].fullName + ".\n* Your HP was maxed out!";
        }
        else
        {
            healString = "You ate the " + healItems[itemNumber].fullName + ".\n* You healed " + healthGained + " HP!";
        }

        StartCoroutine(TypeSentence(healString));
        healItems.Remove(healItems[itemNumber]);
    }

    public void Continue()
    {
        flavourText.gameObject.SetActive(false);
        continueButton.SetActive(false);
        PhaseManager.StaticResume(phaseManager.GetComponent<PhaseManager>());
    }
}
