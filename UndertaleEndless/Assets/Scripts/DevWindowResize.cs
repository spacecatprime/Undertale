using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevWindowResize : MonoBehaviour {


    private void Start()
    {

        Screen.SetResolution(Display.main.systemHeight * 9 / 16, Display.main.systemHeight, false);
    }

}
