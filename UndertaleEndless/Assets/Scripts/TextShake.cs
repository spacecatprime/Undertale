using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextShake : MonoBehaviour
{
    public GameObject text;

    public static Quaternion originalRotation;

    public static bool shouldShake;

    public float shakeRange = 20f; // shake range change be changed from inspector,
                                   //keep it mind that max it can go is half in either direction

    // Use this for initialization
    void Start()
    {
        shouldShake = false;
        originalRotation = text.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldShake)
        {
            StartCoroutine(Shake());
        }
        else
        {
            text.transform.rotation = originalRotation;
        }

    }

    private IEnumerator Shake()
    {
        float z = Random.value * shakeRange - (shakeRange / 2);
        text.transform.eulerAngles = new Vector3(originalRotation.x, originalRotation.y, originalRotation.z + z);
        yield return new WaitForSeconds(0.05f);
    }
}