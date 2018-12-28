using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Item", menuName = "Healing Item")]
public class HealingItem : ScriptableObject
{
    public string fullName;
    public string shortName;
    public string seriousName;
    public int healingAmount;
}
