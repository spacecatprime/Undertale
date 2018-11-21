using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Location
{
    Top, Bottom, Left, Right, Random
}


[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject {
   
    public AnimationCurve Curve;

    public Sprite image;

    public Location spawnLocation;
    public Vector2 size;

    public AudioClip spawnSound;
    public AudioClip flightSound;

    public float speed;
    public float rotationSpeed;

    public int damage;

    

}
