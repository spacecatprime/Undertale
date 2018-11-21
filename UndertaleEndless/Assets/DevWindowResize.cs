using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevWindowResize : MonoBehaviour {

    int lastWidth = Screen.width;
    int lastHeight = Screen.height;
    bool isReseting = false;

    void LateUpdate()
    {
        if (Camera.main.aspect != 0.375f && !isReseting)
        {
            if (Screen.width != lastWidth || Screen.height != lastHeight)
            {
                // user is resizing width
                StartCoroutine(SetResolution());
                lastWidth = Screen.width;
                lastHeight = Screen.height;
            }
        }
    }
    IEnumerator SetResolution()
    {
        isReseting = true;
        Screen.fullScreen = !Screen.fullScreen;
        Screen.SetResolution(Screen.height * 9/16, Screen.height, false);
        yield return new WaitForSeconds(0.5F);
        isReseting = false;
    }
}
