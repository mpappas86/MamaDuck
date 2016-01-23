using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {
	void OnTriggerEnter (Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			Application.LoadLevel (Application.loadedLevel);
		}
	}
}