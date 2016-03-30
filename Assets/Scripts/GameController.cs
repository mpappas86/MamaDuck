using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : TimeManager {
	
	public GameObject[] tiers;      // List of all available tiers (top-level GameObjects containing the whole
	                                // tier underneath) for the level.
	public int currentTier = 0;     // Starting tier for the level
	
	private bool levelOver = false; // Whether or not the level has ended.
	private int finalScore = 0;     // Score the player accumulated, all things considered - to be tallied at level end.

	void Start () {
		// The GameController manages physics-time, and can set it to 0 to pause the game.
		Time.timeScale = 1;
	}

	// Change the active tier.
	public void TierChange(bool up){
		int up_down = up ? 1 : -1;
		GameObject newTier = tiers [currentTier + up_down];
		GameObject oldTier = tiers [currentTier];

		// Move the Player up/down a value equal to the y difference between the two tiers.
		// Do the same for the ducklings if they're currently connected to Mama.
		float ydiff = newTier.transform.position.y - oldTier.transform.position.y;
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		player.transform.position += new Vector3 (0, ydiff, 0);
		GameObject[] ducklings = GameObject.FindGameObjectsWithTag("Duckling");
		foreach (GameObject ducky in ducklings) {
			DucklingScript dc = ducky.GetComponent<DucklingScript>();
			if (dc.contactWithMama){
				dc.transform.position += new Vector3 (0, ydiff, 0);
			}
		}

		newTier.SetActive (true);
		oldTier.SetActive (false);
		this.currentTier = currentTier + up_down;
		
	}

	// Stop the game and compute the score, the level is over!
	public void endLevel(){
		this.activeTimer(false);
		this.levelOver = true;
		this.finalScore = this.getTimeBonus () + this.getPlayerScore ();
		Time.timeScale = 0;
	}

	// Retrieve the total points the player accumulated, separate from any time or completion bonus.
	private int getPlayerScore(){
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		PlayerControl playerScript = (PlayerControl)player.GetComponent(typeof(PlayerControl));
		return playerScript.getScore();
	}

	// Only used after the level is over - we put some text up telling you that you won, giving you your final score,
	// and providing buttons with options of what to do next.
	void OnGUI ()
	{
		if (levelOver) {
			GUIStyle winStyle = new GUIStyle ();
			winStyle.fontStyle = FontStyle.BoldAndItalic;
			winStyle.fontSize = 20;
			winStyle.alignment = TextAnchor.UpperCenter;
			string winText = "You Won!\nFinal score: " + this.finalScore.ToString();
			GUI.Box (new Rect (.5f * Screen.width - 200, .1f * Screen.height, 400, .18f * Screen.height), winText, winStyle);
			
			
			if (GUI.Button (new Rect (.5f * Screen.width - 200, .75f * Screen.height, 400, .08f * Screen.height), "Return to Menu?")) {
				Application.LoadLevel ("MainMenu");
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 200, .55f * Screen.height, 400, .08f * Screen.height), "Replay?")) {
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}
}
