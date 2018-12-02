using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject {

    public string EnemyName;

    public List<FightPhase> Phases;
    //public List<Projectile> ProjectilesUsed; Old method

    public Sprite EnemySprite;
    public Sprite Background;
    public AudioClip bossMusic;
    
}
