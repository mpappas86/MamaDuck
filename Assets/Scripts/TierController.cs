using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TierController : MonoBehaviour {
	
	public GameObject[] tiers;      // List of all available tiers (top-level GameObjects containing the whole
	// tier underneath) for the level.
	public int currentTier = 0;     // Starting tier for the level
	private int liveTier = 0;        // Tier on which ducks are, in case we're just viewing other tiers, not moving.

	private float prevTimeRate;     // For when we pause because we're looking at other tiers.

	private GameObject[] ducklings_ref; // Any ducklings.
	private GameObject player_ref;      // The player. These two are used since sometimes the ducks go inactive.

	private delegate void ExampleTierSwapper(bool up, float ydiff, int up_down);

	private CameraController cc;

	public void Start () {
		player_ref = GameObject.FindGameObjectWithTag ("Player");
		ducklings_ref = GameObject.FindGameObjectsWithTag ("Duckling");
		cc = (CameraController)GameObject.FindGameObjectWithTag ("MainCamera").GetComponent (typeof(CameraController));
	}

	public bool onLiveTier(){
		return liveTier == currentTier;
	}
	
	public void moveTier(bool up){
		changeTier (up, _moveTier);
	}
	
	private void _moveTier(bool up, float ydiff, int up_down){
		// Set the duck to be done moving so it will reevaluate where it can move to.
		BaseTileMover bts = (BaseTileMover)player_ref.GetComponent (typeof(BaseTileMover));
		bts.isMoving = false;
		
		// Move the ducklings if they're currently connected to Mama.
		foreach (GameObject ducky in ducklings_ref) {
			DucklingScript dc = ducky.GetComponent<DucklingScript> ();
			if (dc.contactWithMama) {
				dc.transform.position += new Vector3 (0, ydiff, 0);
			}
		}
		this.liveTier = this.liveTier + up_down;

	}
	
	private void changeTier(bool up, ExampleTierSwapper theTierSwapper){
		int up_down = up ? 1 : -1;
		if (currentTier + up_down >= tiers.Length) {
			up_down = -1 * currentTier;
		}
		GameObject newTier = tiers [currentTier + up_down];
		GameObject oldTier = tiers [currentTier];
		
		float ydiff = newTier.transform.position.y - oldTier.transform.position.y;
		// Move the Player up/down a value equal to the y difference between the two tiers.
		// Do this even if we're not really moving the ducks so the camera will move. Hence the activation.
		player_ref.SetActive (true);
		player_ref.transform.position += new Vector3 (0, ydiff, 0);
		
		theTierSwapper (up, ydiff, up_down);
		
		newTier.SetActive (true);
		oldTier.SetActive (false);
		this.currentTier = this.currentTier + up_down;
	}
	
	public void viewTier(bool up){
		changeTier (up, _viewTier);
	}
	
	private void _viewTier(bool up, float ydiff, int up_down){
		bool active_setting;
		if(currentTier + up_down == this.liveTier){
			active_setting = true;
			cc.UnFreeze();
		} else {
			active_setting = false;
			cc.SetPositionImmediatelyXZ(tiers[currentTier].transform.position);
			cc.Freeze();
		}
		player_ref.SetActive(active_setting);
		foreach(GameObject ducky in ducklings_ref){
			ducky.SetActive(active_setting);
		}
	}
}
