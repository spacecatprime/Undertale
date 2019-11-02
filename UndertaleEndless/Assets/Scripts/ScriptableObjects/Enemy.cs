using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackableDecorator;
using UnityEditor;
using Mono;
using UnityScript;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject {

    [Box(4, 4, 4, 4, order = 1)]
    [Group("Enemy", 10)]
    [Heading(title = "Enemy Settings", order = 1)]
    [StackableField]
    public string enemyName;
    [InGroup("Enemy")]
    [StackableField]
    public Object enemyPhaseDialogueManager;
    [InGroup("Enemy")]
    [StackableField]
    public Dialogue enemyDialogue;
    [InGroup("Enemy")]
    [StackableField]
    [Preview]
    [Expandable]
    public RuntimeAnimatorController EnemyAnimation;
    [InGroup("Enemy")]
    [StackableField]
    [Preview]
    [Expandable]
    public Sprite Background;
    [InGroup("Enemy")]
    [StackableField]
    public AudioClip bossMusic;
    [InGroup("Enemy")]
    [StackableField]
    public float HP;
    [InGroup("Enemy")]
    [StackableField]
    public float Atk;
    [InGroup("Enemy")]
    [StackableField]
    public float Def;
    [InGroup("Enemy")]
    [StackableField]
    public float Exp;
    [InGroup("Enemy")]
    [StackableField]
    public float Gold;

    //public string EnemyName;

    [StackableField]
    public List<FightPhase> Phases;

}
