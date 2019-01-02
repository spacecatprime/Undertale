using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Template MonsterBehaviour
public class MonsterBehaviourTemplate : MonoBehaviour {

    public int startingPhase = 0;
    public Vector2 normalLoopRange = new Vector2(0, 2); //Lowest,Highest
    public Vector2 conditionalHealthPhase = new Vector2(0, 2); //Conditional Phase, if health is Y or below

    // Use this for initialization
    void Start () {
        GameManager.currentPhase = startingPhase;
    }
	
	// Update is called once per frame
	void Update () {
		if(GameManager.nextPhaseCalculation)
        {
            GameManager.nextPhaseCalculation = false;
            if(GameManager.health <= conditionalHealthPhase.y) //Health Condition
            {
                GameManager.currentPhase = Mathf.RoundToInt(conditionalHealthPhase.x);
            }
            else if((GameManager.currentPhase += 1) > normalLoopRange.y) //Loop maxed condition
            {
                GameManager.currentPhase = Mathf.RoundToInt(normalLoopRange.x); //Else
            }
            else
            {
                GameManager.currentPhase += 1;
            }
        }
    }
}
