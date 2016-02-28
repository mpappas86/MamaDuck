using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {
	void OnTriggerEnter (Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			GameObject gc = GameObject.FindGameObjectWithTag("GameController");
			GameController gcScript = (GameController)gc.GetComponent(typeof(GameController));
			gcScript.endLevel();
		}
	}
}