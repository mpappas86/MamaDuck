using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

	// If the player hits this button, just tell the GameController that the current level is over.
	void OnTriggerEnter (Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			GameObject gc = GameObject.FindGameObjectWithTag("GameController");
			GameController gcScript = (GameController)gc.GetComponent(typeof(GameController));
			gcScript.endLevel();
		}
	}
}