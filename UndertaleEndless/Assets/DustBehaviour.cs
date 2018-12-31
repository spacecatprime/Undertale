using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteShatter;

public class DustBehaviour : MonoBehaviour {

    public bool shouldFadeOut;
    public bool shouldMove;
    public float distanceFromTop;
    public SpriteRenderer sprite;

    // Use this for initialization
    void Start () {
        distanceFromTop = new Vector2(this.transform.localPosition.y, this.transform.position.y).magnitude;
        distanceFromTop = Mathf.Abs(3 - distanceFromTop)/1.5f;
        distanceFromTop = Random.Range(distanceFromTop - 0.25f, distanceFromTop + 0.25f);

        GetComponent<Rigidbody2D>().gravityScale = 0;
        sprite = this.GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
		if(shouldFadeOut)
        {
            sprite.color = new Color(1, 1, 1, sprite.color.a - 2f * Time.deltaTime);
        }

        if(sprite.color.a <= 0.05f)
        {
            this.gameObject.SetActive(false);
        }

        WaitParticle();
    }

    public void WaitParticle()
    {
        if (distanceFromTop < 0 && !shouldFadeOut)
        {
            PhaseManager.shouldPlayDeathSound = true;
            shouldFadeOut = true;
            var x = UnityEngine.Random.Range(-0.25f, 0.25f);
            var y = 1f;
            var direction = new Vector2(x, y);
            //if you need the vector to have a specific length:
            direction = direction.normalized * 5f;

            this.GetComponent<Rigidbody2D>().AddForce(direction * 20f);
        }
        else
        {
            distanceFromTop -= Time.deltaTime;
        }
    }
}
