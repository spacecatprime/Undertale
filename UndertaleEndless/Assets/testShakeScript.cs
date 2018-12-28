using UnityEngine;
using System.Collections;

public class testShakeScript : MonoBehaviour
{
    public Vector2 original;
    public float shakeAmount;

    private void Start()
    {
        original = this.transform.position;

    }

    private void Update()
    {
        if(Input.anyKey)
        {
            shakeAmount = 100;
            StartCoroutine(Shake());
        }

    }

    IEnumerator Shake()
    {
        while (shakeAmount > 0)
        {
            if (shakeAmount < 0.005f)
                shakeAmount = 0;
            shakeAmount = Mathf.Lerp(shakeAmount, 0, 0.1f);
            yield return new WaitForSeconds(0.1f);
            this.transform.position = new Vector2(original.x + Mathf.Abs(shakeAmount), original.y);
            yield return new WaitForSeconds(0.1f);
            this.transform.position = new Vector2(original.x - Mathf.Abs(shakeAmount), original.y);
        }
    }

}