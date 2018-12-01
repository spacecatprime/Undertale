
using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    public static bool shakeTrue;
    // How long the object should shake for.
    private float shakeDuration = 0.075f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    private float shakeAmount = 0.025f;
    private float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        shakeTrue = false;

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    private void Update()
    {
        if(shakeTrue)
        {
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {

                shakeDuration = 0f;
                camTransform.localPosition = originalPos;
                shakeTrue = false;
                shakeDuration = 0.075f;
                shakeAmount = 0.025f;
            }
        }
    }


}