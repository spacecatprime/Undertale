using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeterminationText : MonoBehaviour {

    public TMP_Text dialogueText;

    public string sentence1;
    public string sentence2;

    public Queue<string> sentences;

    public AudioSource asgoreVoice;

    // Use this for initialization
    void Start()
    {
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
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            string letterStr = letter.ToString();
            if (letterStr == "!")    // if ! wait
            {
                dialogueText.text += letterStr;
                yield return new WaitForSeconds(1.0f);
            }
            if (letterStr != " ")    // if 'space' dont speak
                asgoreVoice.Play();

            if (letterStr != "!")
                dialogueText.text += letterStr;
            yield return new WaitForSeconds(0.075f); //Time between letters
        }
        yield return new WaitForSeconds(1.25f);
        if(sentence == sentence1)
            StartDialogue(sentence2);
    }
}
