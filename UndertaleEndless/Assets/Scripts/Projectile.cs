using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Location
{
    Top, Bottom, Left, Right, Random
}

public enum MovementType
{
    Straight, DirectPlayer, Magnet, Random
}

public enum ProjectileType
{
    Regular, BlueNoMove, OrangeYesMove, Heal
}


[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject {


    public ProjectileType projectileType;
    public Location spawnLocation;
    public MovementType ProjectileMovementType;

    public Color SpriteTint = new Color(1.0f, 1.0f, 1.0f);

    public AnimationCurve SpawnFrequency;

    public Sprite image;

    public AudioClip spawnSound;
    public AudioClip flightSound;

    public float speed;
    public float rotationModifier;

    public float damage;
    public bool destroyOnTouch;

}

