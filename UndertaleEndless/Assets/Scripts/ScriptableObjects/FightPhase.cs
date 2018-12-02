using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Phase0", menuName = "New Phase")]
public class FightPhase : ScriptableObject {

    public List<Projectile> ProjectileCombo;
    public float AttackLength;

}
