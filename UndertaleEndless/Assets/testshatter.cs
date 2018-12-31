using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteShatter;

public class testshatter : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        //If the user clicks the left mouse button, explode the monster!
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shattering!");
            GetComponent<Shatter>().shatter();
            MonsterHeartbreak.isEnemyKilled = true;
        }

        //If the user clicks the right mouse button, reset the monster!
        else if (Input.GetMouseButtonDown(1))
            GetComponent<Shatter>().reset();
    }
}
