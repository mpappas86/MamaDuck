using UnityEngine;
using System.Collections;

public class ElevatorButton : MonoBehaviour {
	
	public bool up = true;  // Whether the button sends Mama up (true) or down (false)
		
	private GameController gc;  // Pointer to the GameController object.
	private bool active = true; // Whether or not the button can be hit. 

	void Start() {
		gc = (GameController) GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
	}

	// When we get hit, turn red and then tell the gameController to update the active tier.
	void OnTriggerEnter (Collider other){
		if (other.gameObject.CompareTag("Player") && active) {
			this.gameObject.GetComponent<Renderer>().material.color = Color.red;
			this.active = false;
			gc.TierChange(this.up);
		}
	}

	// Expose a public method to turn active again
	public void ResetColor(){
		this.gameObject.GetComponent<Renderer>().material.color = Color.white;
		this.active = true;
	}
}