using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//Template MonsterBehaviour
public class TorielBehaviour : MonoBehaviour {

    public int startingPhase = 0;
    public int latestSpare = -1;
    public Vector2 normalLoopRange = new Vector2(0, 1); //Lowest,Highest
    public Vector2 conditionalHealthPhase = new Vector2(2, 2); //If health is X, goto phase Y
    public List<GameObject> speechBubbles;
    public AudioClip monsterTextSound;
    public AudioSource monsterSound;
    public int deathSentence;
    public int betrayalSentence;
    public string entireTag = "";

    private void Start()
    {
        monsterTextSound = ProjectileManager.staticEnemy.enemyDialogue.defaultTalk;

        monsterSound = GameObject.Find("MonsterText").GetComponent<AudioSource>();

        speechBubbles = FlavourTextManager.staticSpeechBubbles;

        foreach (GameObject bubble in speechBubbles)
        {
            bubble.SetActive(false);
        }

        monsterSound.clip = monsterTextSound;
    }

    public bool CheckForMoreMercy()
    {
        if (latestSpare < GameManager.spareCounter)
        {
            latestSpare = GameManager.spareCounter;
            return true;
        }
        else
        {
            latestSpare = GameManager.spareCounter;
            return false;
        }
    }

    // Update is called once per frame
    void Update() {
        if (PhaseManager.nextDeathSentence)
        {
            deathSentence += 1;
            NeutralDeathBubble(1);

            PhaseManager.nextDeathSentence = false;
            if (deathSentence == ProjectileManager.staticEnemy.enemyDialogue.defeatNeutral.Count - 1)
                StartCoroutine(Dust());
            
        }
        else if (PhaseManager.nextBetrayalSentence)
        {
            betrayalSentence += 1;
            BetrayalDeathBubble(1);

            PhaseManager.nextBetrayalSentence = false;
            if (betrayalSentence == ProjectileManager.staticEnemy.enemyDialogue.defeatBetrayal.Count - 1)
                StartCoroutine(Dust());

        }
        else if (GameManager.nextPhaseCalculation)
        {
            GameManager.nextPhaseCalculation = false;
            if (ProjectileManager.enemyKilled && !PhaseManager.canBeBetrayed) //Check for death
            {
                monsterSound.clip = ProjectileManager.staticEnemy.enemyDialogue.injuredTalk;
                NeutralDeathBubble(1);
                PhaseManager.monsterTalking = true;
            }

            if (ProjectileManager.enemyKilled && PhaseManager.canBeBetrayed) //Check for death
            {
                monsterSound.clip = ProjectileManager.staticEnemy.enemyDialogue.disbeliefTalk;
                BetrayalDeathBubble(1);
                PhaseManager.monsterTalking = true;
            }
            if (GameManager.totalPhases == 0)
            {
                GameManager.currentPhase = startingPhase;
            }
            else if(GameManager.spareCounter > 12) //SparePhase
            {
                GameManager.currentPhase = 3;
            }
            else if (GameManager.health <= conditionalHealthPhase.x) //Health Condition
            {
                Debug.Log("Mercy Phase Activated");
                GameManager.currentPhase = Mathf.RoundToInt(conditionalHealthPhase.y);
            }
            else if ((GameManager.currentPhase + 1) > normalLoopRange.y) //Loop maxed condition
            {
                GameManager.currentPhase = Mathf.RoundToInt(normalLoopRange.x); //Back to start of loop
            }
            else
            {
                GameManager.currentPhase += 1; //Next phase
            }
            Debug.Log("The calculated next phase is: " + GameManager.currentPhase);

            if (CheckForMoreMercy())
            {
                int defaultBubble = 2;
                if(GameManager.spareCounter > 12)
                {
                    this.gameObject.GetComponent<AudioSource>().Stop();
                    defaultBubble = 1;
                }
                MercySpeechBubble(defaultBubble);
                PhaseManager.monsterTalking = true;
            }

        }

        if (GameManager.spareCounter > 12)
        {
            PhaseManager.canBeBetrayed = true;
            PhaseManager.defence = -9999;
        }

        if (PhaseManager.health < 200)
        {
            PhaseManager.defence = -9999;
        }
    }


    void NeutralDeathBubble(int bubbleID)
    {
        StartCoroutine(TypeBubble(bubbleID, ProjectileManager.staticEnemy.enemyDialogue.defeatNeutral[deathSentence], speechBubbles[bubbleID].GetComponentInChildren<TextMeshPro>(), true));
    }

    void BetrayalDeathBubble(int bubbleID)
    {
        StartCoroutine(TypeBubble(bubbleID, ProjectileManager.staticEnemy.enemyDialogue.defeatBetrayal[betrayalSentence], speechBubbles[bubbleID].GetComponentInChildren<TextMeshPro>(), true));
    }

    void MercySpeechBubble(int bubbleID)
    {
        StartCoroutine(TypeBubble(bubbleID, ProjectileManager.staticEnemy.enemyDialogue.spareDialogue[GameManager.spareCounter], speechBubbles[bubbleID].GetComponentInChildren<TextMeshPro>(), false));
    }

    IEnumerator TypeBubble(int bubbleID, string sentence, TextMeshPro bubble, bool isDead)
    {
        PhaseManager.staticMonsterContinueButton.SetActive(false);

        if(isDead)
            if(deathSentence == 0 && betrayalSentence == 0)
            {
                yield return new WaitForSeconds(3f);
            }

        bubble.text = "";
        speechBubbles[bubbleID].SetActive(true);

        yield return new WaitForSeconds(0.25f);

        int i = 0;


        foreach (char letter in sentence)
        {
            string letterStr = letter.ToString();

            if (letterStr == "<") //Checking for tags
            {

                while (i < 100)
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
                bubble.text += entireTag;

            }

            if (entireTag.Length > 0)
            {
                entireTag = entireTag.Substring(0, entireTag.Length - 1);
                continue;
            }
            

            if (letterStr == "[") //Special Time adders
            {
                yield return new WaitForSeconds(0.1f);
            }
            else if (letterStr == "]")
            {
                yield return new WaitForSeconds(0.25f);
            }
            else if (letterStr == "%") //New Line
            {
                bubble.text += "\n";
            }
            else
            {
                if (letterStr != " ")
                    monsterSound.Play();
                bubble.text += letterStr;
                yield return new WaitForSeconds(0.05f); //Time between letters
            }

            i += 1;
        }

        if(!(deathSentence + 1 > ProjectileManager.staticEnemy.enemyDialogue.defeatNeutral.Count - 1) || !(betrayalSentence + 1 > ProjectileManager.staticEnemy.enemyDialogue.defeatBetrayal.Count - 1))
            PhaseManager.staticMonsterContinueButton.SetActive(true);
    }

    IEnumerator Dust()
    {
        yield return new WaitForSeconds(6f);
        PhaseManager.dustNow = true;
    }

}
