using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackableDecorator;
using UnityEditor;

public enum Location
{
    Top, Bottom, Left, Right, Random, Specific
}

public enum LocationSpecific
{
    SpecificX, SpecificY, SpecificXY
}

public enum MovementType
{
    Straight, DirectPlayer, Magnet, Random, SineWave, NegSineWave
}

public enum MovementDirection
{
    NoMod, N, NE, E, SE, S, SW, W, NW
}

public enum ProjectileType
{
    Regular, BlueNoMove, OrangeYesMove, Heal
}

public enum MaskInteraction
{
    None, VisibleInsideMask, VisibleOutsideMask
}

public enum Relationships
{
    None, Child, Parent
}

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject {

//Main Settings

    [Box(4, 4, 4, 4, order = 1)]
    [Group("Main", 2)]
    [Heading(title = "Main Settings", order = 1)]

    [StackableField]
    [Preview]
    [Expandable]
    public Sprite image;

    [InGroup("Main")]
    [EnumButton]
    public Relationships relationships;

    [InGroup("Main")]
    [EnumButton]
    public ProjectileType projectileType;

//Relatiionship Settings

    [EnableIf("#RelationsVisible")]
    [Box(4, 4, 4, 4, order = 1)]
    [Group("Relation", 0)]
    [Heading(title = "Relationship Settings", order = 1)]

    [StackableField]
    public Projectile child;

//Movement Settings

    [Box(4, 4, 4, 4, order = 1)]
    [Group("Movement", 3)]
    [Heading(title = "Movement Settings", order = 1)]

    [EnumButton]
    public MovementType ProjectileMovementType;

    [InGroup("Movement")]
    [EnumButton]
    public MovementDirection movementDirection;

    [InGroup("Movement")]
    [StackableField]
    public float speed;

    [InGroup("Movement")]
    [StackableField]
    public bool AffectedByGravity;

//Wave Settings

    [EnableIf("#WavesVisible")]
    [Box(4, 4, 4, 4, order = 1)]
    [Group("Wave", 2)]
    [Heading(title = "Wave Settings", order = 1)]

    [StackableField]
    public float WaveMagnitude;

    [InGroup("Wave")]
    [StackableField]
    public float WaveFrequency;

    [InGroup("Wave")]
    [StackableField]
    public bool RandomWaveTime;

//Spawn Settings

    [Box(4, 4, 4, 4, order = 1)]
    [Group("Spawn", 7)]
    [Heading(title = "Spawn Settings", order = 1)]
    [EnumButton]
    public Location spawnLocation;

    //Specific

    [ShowIf("#EnableSpecific")]
    [InGroup("Spawn")]
    [StackableField]
    public LocationSpecific locationSpecific;

    [ShowIf("#EnableSpecific")]
    [EnableIf("#EnableSpecificX")]
    [InGroup("Spawn")]
    [StackableField]
    public float specificSpawnX;

    [ShowIf("#EnableSpecific")]
    [EnableIf("#EnableSpecificY")]
    [InGroup("Spawn")]
    [StackableField]
    public float specificSpawnY;

    [InGroup("Spawn")]
    [StackableField]
    [EnableIf("$RandomSpawnFrequency", inverted = true)]
    public AnimationCurve SpawnFrequency;
    [InGroup("Spawn")]
    [StackableField]
    public bool RandomSpawnFrequency;
    [InGroup("Spawn")]
    [StackableField]
    [EnableIf("$RandomSpawnFrequency", enable = true)]
    [RangeSlider(0f, 10f, showInLabel = true)]
    public Vector2 timeSpawnRange = new Vector2(2.5f, 7.5f);
    [InGroup("Spawn")]
    [StackableField]
    public float spawnDeadTime;

    [Box(4, 4, 4, 4, order = 1)]
    [Group("Misc", 10)]
    [Heading(title = "Miscellaneous Settings", order = 1)]
    [StackableField]
    public bool FlipX;
    [InGroup("Misc")]
    [StackableField]
    public bool FlipY;
    [InGroup("Misc")]
    [ValidateValue("Damage has to be NEGATIVE if Projectile type is: Heal", "#CheckDamage")]
    [StackableField]
    public float damage;
    [InGroup("Misc")]
    [StackableField]
    public bool destroyOnTouch;
    [InGroup("Misc")]
    [StackableField]
    public MaskInteraction maskInteraction;
    [InGroup("Misc")]
    [StackableField]
    public float deathTimer = 60f;
    [InGroup("Misc")]
    [StackableField]
    public bool destroyOnPhaseEnd = true;
    [InGroup("Misc")]
    [StackableField]
    public float waitTimer = 0f;

    public bool WavesVisible()
    {
        if(ProjectileMovementType.ToString() == "SineWave" || ProjectileMovementType.ToString() == "NegSineWave")
        {
            return true;
        }
        else
            return false;
    }

    public bool RelationsVisible()
    {

        if (relationships.ToString() == "Parent")
        {
            return true;
        }
        else
            return false;
    }

    public bool EnableSpecific()
    {
        if (spawnLocation.ToString() == "Specific")
        {
            return true;
        }
        else
            return false;
    }

    public bool EnableSpecificX()
    {
        if (locationSpecific.ToString() == "SpecificX" || locationSpecific.ToString() == "SpecificXY")
        {
            return true;
        }
        else
            return false;
    }

    public bool EnableSpecificY()
    {
        if (locationSpecific.ToString() == "SpecificY" || locationSpecific.ToString() == "SpecificXY")
        {
            return true;
        }
        else
            return false;
    }

    public bool CheckDamage(float value)
    {
        if (projectileType.ToString() == "Heal")
            if (value > 0)
                return false;
            else
                return true;
        else
            return true;
    }

    //public ProjectileType projectileType;
    //public Location spawnLocation;
    //public MovementType ProjectileMovementType;
}

