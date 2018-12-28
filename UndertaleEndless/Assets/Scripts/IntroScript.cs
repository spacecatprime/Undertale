using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class IntroScript : MonoBehaviour
{
    private AudioSource[] allAudioSources;
    private int i = 0;

    public float currentSeconds;
    public AudioSource mus;
    public GameObject UTLogo;
    public TMP_Text dialogueText;
    public Image m_Image;
    public GameObject Image;
    public Sprite m_Sprite;

    private bool check;

    private string name;

    public List<string> allSenteces;
    public Sprite[] sprites;

    private string sentence;

    public Queue<string> sentences;

    public AudioSource introVoice;

    public Animation imageAnimator;

    // Use this for initialization
    void Start()
    {
        currentSeconds = 0;
        sentence = allSenteces[i];
        m_Image = m_Image.GetComponent<Image>();
        name = PlayerPrefs.GetString("Name");
        sentences = new Queue<string>();
        StartCoroutine(Initialize());

    }

    private void FixedUpdate()
    {
        currentSeconds += Time.fixedDeltaTime;
    }

    public IEnumerator Initialize()
    {
        m_Image.sprite = sprites[10];
        yield return new WaitForSeconds(0.5f);
        imageAnimator.Play("IntroImageFadeInOut");
        yield return new WaitForSeconds(0.2f);
        m_Image.sprite = sprites[0];
        mus.Play();
        StartDialogue(sentence);
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
        dialogueText.text = "";
        float time = 1.25f;
        foreach (char letter in sentence.ToCharArray())
        {

            string letterStr = letter.ToString();

            if(letterStr == "%")
            {
                dialogueText.text += "\n";
            }
            else if (letterStr == "!" || letterStr == "," || letterStr == ".")    // if ! or , wait
            {
                dialogueText.text += letterStr;
                time -= 1.0f;
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                if(letterStr != " ")
                    introVoice.Play();
                dialogueText.text += letterStr;
                yield return new WaitForSeconds(0.05f); //Time between letters
            }

        }

        if (sentence == allSenteces[0])
            yield return new WaitForSeconds(7.5f - currentSeconds); //HUMANS and MONSTERS

        else if (sentence == allSenteces[1])
            yield return new WaitForSeconds(7.5f - currentSeconds); //War broke out

        else if (sentence == allSenteces[2])
            yield return new WaitForSeconds(5f - currentSeconds); //Humans Win

        else if (sentence == allSenteces[3])
            yield return new WaitForSeconds(4.9f - currentSeconds); //Spell

        else if (sentence == allSenteces[4])
            yield return new WaitForSeconds(1.1f - currentSeconds); //...

        else if (sentence == allSenteces[5])
            yield return new WaitForSeconds(5f - currentSeconds); //MT EBOTT

        else if (sentence == allSenteces[6])
            yield return new WaitForSeconds(7.5f - currentSeconds); //Legends say

        else if (sentence == allSenteces[7])
            yield return new WaitForSeconds(5f - currentSeconds); //Looking down the hole

        else if (sentence == allSenteces[8])
            yield return new WaitForSeconds(5f - currentSeconds); //Tripping

        else if (sentence == allSenteces[9])
            yield return new WaitForSeconds(5f - currentSeconds); //Falling

        currentSeconds = 0;


        if (i+2 > allSenteces.Count)
        {
            yield return new WaitForSeconds(1.8f);
            Image.transform.position = new Vector3(Image.transform.position.x, -265, Image.transform.position.y);

            m_Image.color = new Color(1.0f, 1.0f, 1.0f);
            m_Image.sprite = sprites[11];

            imageAnimator.Play("IntroPanDown");

            yield return new WaitForSeconds(12f);
            imageAnimator.Play("FadeAway");
            yield return new WaitForSeconds(15f);
            end();

        }

        else
        {
            i += 1;
            sentence = allSenteces[i];
            imageAnimator.Play("IntroImageFadeInOut");
            yield return new WaitForSeconds(0.2f);
            m_Image.sprite = sprites[i];
            DisplayNextSentence(sentence);
        }

    }


    public void end()
    {
        StopAllCoroutines();
        StopAllAudio();
        UTLogo.SetActive(true);

        StartCoroutine(LoadLevel());
    }

    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(2.0f);
        if (PlayerPrefs.GetString("Name") == "")
        {
            SceneManager.LoadScene("Naming");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
