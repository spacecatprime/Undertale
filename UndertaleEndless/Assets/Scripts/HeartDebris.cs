using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDebris : MonoBehaviour {

    public bool currentlySwaping;
    public Sprite[] DebrisSpriteList;
    public Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        Vector2 location = new Vector2(PersistentData.LastDeathLocation.x, PersistentData.LastDeathLocation.y - 0.025f); //Slight bias lets bullets shoot up more
        Rigidbody2DExtension.AddExplosionForce(rb, 10.0f, location, 20.0f);
    }

    // Update is called once per frame
    void Update () {
        if (!currentlySwaping)
            StartCoroutine(swapSprites());


    }

    public IEnumerator swapSprites()
    {
        currentlySwaping = true;
        var spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        var i = 0;
        spriteRenderer.sprite = DebrisSpriteList[i];
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.sprite = DebrisSpriteList[i + 1];
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.sprite = DebrisSpriteList[i + 2];
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.sprite = DebrisSpriteList[i + 3];
        yield return new WaitForSeconds(0.25f);
        currentlySwaping = false;
    }
}

public static class Rigidbody2DExtension
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * (wearoff <= 0f ? 0f : explosionForce) * wearoff);
    }

    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        Vector3 baseForce = dir.normalized * (wearoff <= 0f ? 0f : explosionForce) * wearoff;
        body.AddForce(baseForce);

        float upliftWearoff = 1 - upliftModifier / explosionRadius;
        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
        body.AddForce(upliftForce);
    }
}