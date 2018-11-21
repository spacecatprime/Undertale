using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollider : MonoBehaviour {

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public static bool isDead;

    void Start()
    {
        isDead = false;
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            isDead = true;
        }
    }
}
