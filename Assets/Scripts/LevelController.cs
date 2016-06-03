using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelController : TimerManager {
	
	public GameObject[] tiers;      // List of all available tiers (top-level GameObjects containing the whole
	                                // tier underneath) for the level.
	public int currentTier = 0;     // Starting tier for the level
	public int liveTier = 0;        // Tier on which ducks are, in case we're just viewing other tiers, not moving.
	
	private bool levelOver = false; // Whether or not the level has ended.
	private int finalScore = 0;     // Score the player accumulated, all things considered - to be tallied at level end.

	private float prevTimeRate;     // For when we pause because we're looking at other tiers.

	private InputHandler ih;        // InputHandler to track non-player-related inputs

	private GameObject[] ducklings_ref; // Any ducklings.
	private GameObject player_ref;      // The player. These two are used since sometimes the ducks go inactive.

	public override void Start () {
		base.Start();
		// The GameController manages physics-time, and can set it to 0 to pause the game.
		Time.timeScale = 1;
		GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
		ih = (InputHandler)gc.GetComponent(typeof(InputHandler));

		player_ref = GameObject.FindGameObjectWithTag ("Player");
		ducklings_ref = GameObject.FindGameObjectsWithTag ("Duckling");
	}

	// Change the active tier. This should probably be two methods - one for moving and one for viewing -
	// once we get animations for tier changes. But for now it's cleaner code this way.
	public void TierChange(bool up, bool moveDucks=true){
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
		
		if (moveDucks) {
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
		} else {
			bool active_setting;
			if(currentTier + up_down == this.liveTier){
				active_setting = true;
				Time.timeScale = this.prevTimeRate;
			} else {
				active_setting = false;
				this.prevTimeRate = Time.timeScale;
				Time.timeScale = 0;
			}
			player_ref.SetActive(active_setting);
			foreach(GameObject ducky in ducklings_ref){
				ducky.SetActive(active_setting);
			}

		}

		newTier.SetActive (true);
		oldTier.SetActive (false);
		this.currentTier = this.currentTier + up_down;
		
	}

	void Update (){
		// Only do a tier change when timeScale is 0 if it got that way from a tier change.
		// This means no tier changes when the game is paused.
		if (ih.isTrigger() && (Time.timeScale != 0 || liveTier != currentTier)) {
			TierChange(true, false);
		}
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
