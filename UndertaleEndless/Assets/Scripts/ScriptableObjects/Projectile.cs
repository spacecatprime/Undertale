using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Location
{
    Top, Bottom, Left, Right, Random, Specific
}

public enum LocationSpecific
{
    None, SpecificX, SpecificY, SpecificXY
}

public enum MovementType
{
    Straight, DirectPlayer, Magnet, Random, SineWave, NegSineWave
}

public enum ProjectileType
{
    Regular, BlueNoMove, OrangeYesMove, Heal
}


[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject {

    public Sprite image;

    public AudioClip spawnSound;
    public AudioClip flightSound;

    public float speed;
    public bool FlipX;
    public bool FlipY;

    public ProjectileType projectileType;

    public Location spawnLocation;

    public LocationSpecific locationSpecific;
    public float specificSpawnX;
    public float specificSpawnY;

    public MovementType ProjectileMovementType;
    public bool RandomWaveTime;
    public float WaveMagnitude;
    public float WaveFrequency;

    public bool AffectedByGravity;

    public Color SpriteTint = new Color(1.0f, 1.0f, 1.0f);

    public AnimationCurve SpawnFrequency;
    public bool RandomSpawnTime;
    public float TimeMin;
    public float TimeMax;

    public float damage;
    public bool destroyOnTouch;
}

