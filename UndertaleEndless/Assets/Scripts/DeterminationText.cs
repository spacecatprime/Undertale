using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeterminationText : MonoBehaviour {

    public TMP_Text dialogueText;

    private string name;
    public List<string> allSentece1;
    public List<string> allSentece2;
    private string sentence1;
    private string sentence2;

    public Queue<string> sentences;

    public AudioSource asgoreVoice;

    // Use this for initialization
    void Start()
    {
        sentence1 = allSentece1[Random.Range(0, allSentece1.Count)];
        sentence2 = allSentece2[Random.Range(0, allSentece2.Count)];

        name = PlayerPrefs.GetString("Name");
        sentences = new Queue<string>();
        StartDialogue(sentence1);
    }



    public void StartDialogue(string sentence)
    {
        sentences.Clear();

        DisplayNextSentence(sentence);
    }

    //When Enter is Pressed
    public void DisplayNextSentence(string sentence)
    {
        if (sentences.Count != 0)
        {
            sentence = sentences.Dequeue();
        }
        else //Enter is pressed at end of sentence
        {
            StopAllCoroutines();

            StartCoroutine(TypeSentence(sentence));
        }
    }


    IEnumerator TypeSentence(string sentence)
    {
        if(sentence == sentence1)
            yield return new WaitForSeconds(4f);
        if(sentence != sentence2)
            dialogueText.text = "";

        float time = 1.25f;
        foreach (char letter in sentence.ToCharArray())
        {
            
            string letterStr = letter.ToString();
            if (letterStr == "!" || letterStr == ",")    // if ! or , wait
            {
                dialogueText.text += letterStr;
                time -= 1.0f;
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                asgoreVoice.Play();
                dialogueText.text += letterStr;
                yield return new WaitForSeconds(0.075f); //Time between letters
            }

        }
        if(sentence == sentence1)
        {
            yield return new WaitForSeconds(time);
            StartDialogue(name);
        }
        if (sentence == name)
            StartDialogue(sentence2);
    }
}
