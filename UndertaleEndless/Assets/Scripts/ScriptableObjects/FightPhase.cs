﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackableDecorator;
using UnityEditor;

[CreateAssetMenu(fileName = "Phase0", menuName = "New Phase")]
public class FightPhase : ScriptableObject {

    [Box(4, 4, 4, 4, order = 1)]
    [Group("Enemy", 0)]
    [Heading(title = "Phase Settings", order = 1)]
    [StackableField]
    public float AttackLength;

    [StackableField]
    public List<Projectile> ProjectileCombo;
}
