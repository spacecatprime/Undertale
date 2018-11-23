using UnityEngine;

public class Player2DExample : MonoBehaviour 
{
    public float moveSpeed = 8f;
    public Joystick joystick;

    Vector3 targetDir;

    private void Update()
    {
        Vector3 inputDir = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);
        Vector3 v = inputDir.normalized;
        v.x = Mathf.Round(v.x);
        v.z = Mathf.Round(v.z);
        if (v.sqrMagnitude > 0.1f)
            targetDir = v.normalized;

        // your movement code

        //Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);

        if (inputDir != Vector3.zero)
        {
            //    transform.rotation = Quaternion.LookRotation(Vector3.forward, moveVector);
            transform.Translate(targetDir * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
