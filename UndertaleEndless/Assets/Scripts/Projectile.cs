using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Location
{
    Top, Bottom, Left, Right, Random
}

public enum LocationSpecific
{
    None, SpecificX, SpecificY
}

public enum MovementType
{
    Straight, DirectPlayer, Magnet, Random, SineWave
}

public enum ProjectileType
{
    Regular, BlueNoMove, OrangeYesMove, Heal
}


[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject {

    public ProjectileType projectileType;

    public Location spawnLocation;

    public LocationSpecific locationSpecific;
    public float specificSpawnLocation;

    public MovementType ProjectileMovementType;
    public bool AffectedByGravity;

    public Color SpriteTint = new Color(1.0f, 1.0f, 1.0f);

    public AnimationCurve SpawnFrequency;

    public Sprite image;

    public AudioClip spawnSound;
    public AudioClip flightSound;

    public float speed;
    public bool FlipX;
    public bool FlipY;

    public float damage;
    public bool destroyOnTouch;

}

