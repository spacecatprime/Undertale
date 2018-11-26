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

    public GameObject UTLogo;
    public TMP_Text dialogueText;
    public Image m_Image;
    public GameObject Image;
    public Sprite m_Sprite;

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
        sentence = allSenteces[i];
        m_Image = m_Image.GetComponent<Image>();
        name = PlayerPrefs.GetString("Name");
        sentences = new Queue<string>();
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
            if (letterStr == "!" || letterStr == "," || letterStr == "." || letterStr == ":")    // if ! or , wait
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
                yield return new WaitForSeconds(0.075f); //Time between letters
            }

        }
        if (i+2 > allSenteces.Count)
        {
            Image.transform.position = new Vector3(Image.transform.position.x, -265, Image.transform.position.y);

            m_Image.color = new Color(1.0f, 1.0f, 1.0f);
            m_Image.sprite = sprites[11];

            yield return new WaitForSeconds(1.6f); //When the last pane is shown
            imageAnimator.Play("IntroPanDown");


            yield return new WaitForSeconds(10f);
            imageAnimator.Play("IntroImageFadeInOut");

            yield return new WaitForSeconds(0.2f);
            m_Image.sprite = sprites[10];
            yield return new WaitForSeconds(5f);
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
