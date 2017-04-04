using UnityEngine;
using System.Collections;

public class ElevatorButton : MonoBehaviour {
	
	public bool up = true;  // Whether the button sends Mama up (true) or down (false)
		
	private TierController tc;  // Pointer to the TierController object.
	private bool active = true; // Whether or not the button can be hit. 

	void Start() {
		tc = (TierController) GameObject.FindGameObjectWithTag ("LevelController").GetComponent<TierController>();
	}

	// When we get hit, turn red and then tell the gameController to update the active tier.
	void OnTriggerEnter (Collider other){
		if (other.gameObject.CompareTag("Player") && active) {
			this.gameObject.GetComponent<Renderer>().material.color = Color.red;
			this.active = false;
			tc.moveTier(this.up);
		}
	}

	// Expose a public method to turn active again
	public void ResetColor(){
		this.gameObject.GetComponent<Renderer>().material.color = Color.white;
		this.active = true;
	}
}