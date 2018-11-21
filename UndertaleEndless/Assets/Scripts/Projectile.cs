using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Location
{
    Top, Bottom, Left, Right, Random
}

public enum MovementType
{
    Straight, FollowPlayer, Random
}


[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject {
   
    public AnimationCurve SpawnFrequency;

    public Sprite image;

    public Location spawnLocation;
    public MovementType ProjectileMovementType;

    public AudioClip spawnSound;
    public AudioClip flightSound;

    public float speed;
    public float rotationSpeed;

    public float damage;
    public bool destroyOnTouch;

}

