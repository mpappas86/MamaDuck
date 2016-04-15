using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

	// If the player hits this button, just tell the GameController that the current level is over.
	void OnTriggerEnter (Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			GameObject lc = GameObject.FindGameObjectWithTag("LevelController");
			LevelController lcScript = (LevelController)lc.GetComponent(typeof(LevelController));
			GameObject gc = GameObject.FindGameObjectWithTag("GameController");
			GameControllerScript gcScript = (GameControllerScript)gc.GetComponent(typeof(GameControllerScript));
			gcScript.levelWin(Application.loadedLevel);
			lcScript.endLevel();
		}
	}
}