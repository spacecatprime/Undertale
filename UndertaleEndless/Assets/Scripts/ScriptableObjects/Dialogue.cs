using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackableDecorator;
using UnityEditor;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public AudioClip defaultTalk;
    public AudioClip injuredTalk;
    public AudioClip disbeliefTalk;

    public string Encounter;

    public string Check;

    public List<string> NeutralFlavourText;

    public List<string> Talk;

    public List<string> spareDialogue;

    public List<string> defeatNeutral;

    public List<string> defeatBetrayal;

    public List<string> defeatGenocide;
}
