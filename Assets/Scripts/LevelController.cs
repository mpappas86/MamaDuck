using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelController : TimerManager {
	
	private bool levelOver = false; // Whether or not the level has ended.
	private int finalScore = 0;     // Score the player accumulated, all things considered - to be tallied at level end.
	
	private InputHandler ih;          // InputHandler to track non-player-related inputs
	private TierController tc;        // TierController script.

	private bool paused = false;      // Is the level paused?
	private float prevTimeRate;       // If the level is paused, what's the time rate to revert to?

	public override void Start () {
		base.Start();
		// The GameController manages physics-time, and can set it to 0 to pause the game.
		Time.timeScale = 1;
		GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
		ih = (InputHandler)gc.GetComponent(typeof(InputHandler));
		tc = (TierController)this.gameObject.GetComponent (typeof(TierController));
	}
	
	void Update (){
		// Only do a tier view when timeScale is 0 if it got that way from a tier change.
		// This means no tier changes when the game is paused.
		if (ih.isTrigger()){
			if (Time.timeScale != 0 || !tc.onLiveTier()) {
				tc.viewTier(true);
			}
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

	void Pause(){
		this.paused = true;
		this.prevTimeRate = Time.timeScale;
		Time.timeScale = 0;
	}
	
	void Unpause(){
		this.paused = false;
		Time.timeScale = this.prevTimeRate;
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
		if (Application.loadedLevel != 0) {
			if (this.paused) {
				GUI.Window(0, new Rect(.02f*Screen.width, .02f*Screen.height, .96f*Screen.width, .96f*Screen.height), ThePauseMenu, "Paused");
				
			} else {
				if (GUI.Button (new Rect (.9f * Screen.width, .9f * Screen.height, .1f*Screen.width, .05f * Screen.height), "Pause")) {
					// Don't pause if the timescale is already 0 (i.e. we were viewing a different tier.)
					// Allowing a pause would mean we don't know what time scale to return to.
					if (Time.timeScale != 0){
						Pause ();
					}
				}
			}
		}
	}
	
	void ThePauseMenu(int idNum){
		if (GUI.Button (new Rect (.5f * Screen.width - 100, .8f * Screen.height, 200, .12f * Screen.height), "Unpause")) {
			Unpause ();
		}
		if (GUI.Button (new Rect (.5f * Screen.width - 100, .6f * Screen.height, 200, .12f * Screen.height), "Return to Main Menu")) {
			Application.LoadLevel(0);
		}
	}
}
