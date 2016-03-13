using UnityEngine;
using System.Collections;

public class ElevatorButton : MonoBehaviour {
	
	public bool up = true;
		
	private GameController gc;

	void Start() {
		gc = (GameController) GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
	}

	void OnTriggerEnter (Collider other){
		if (other.gameObject.CompareTag("Player")) {
			this.gameObject.GetComponent<Renderer>().material.color = Color.red;
			gc.TierChange(this.up);
		}
	}

	public void ResetColor(){
		this.gameObject.GetComponent<Renderer>().material.color = Color.white;
	}
}