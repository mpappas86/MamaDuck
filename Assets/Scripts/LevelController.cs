using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelController : TimerManager {
	
	private bool levelOver = false; // Whether or not the level has ended.
	private int finalScore = 0;     // Score the player accumulated, all things considered - to be tallied at level end.
	
	private InputHandler ih;          // InputHandler to track non-player-related inputs
	private TierController tc;        // TierController script.
	private GameControllerScript gcs; // GameControllerScript reference

	private bool paused = false;      // Is the level paused?

	private string nextLevelString = "NextLevel?"; // The text on the next level button. Changes if can't access next level for some reason.

	public override void Start () {
		base.Start();
		GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
		ih = (InputHandler)gc.GetComponent(typeof(InputHandler));
		ih.UnFreeze ();
		gcs = (GameControllerScript)gc.GetComponent (typeof(GameControllerScript)); 
		tc = (TierController)this.gameObject.GetComponent (typeof(TierController));

        //mark code
        // single audio source
        // load sound effects with resources.load before playing
        gc.AddComponent<AudioSource>();
    }
	
	public override void Update (){
		base.Update ();
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
		ih.Freeze ();
	}

	// Retrieve the total points the player accumulated, separate from any time or completion bonus.
	private int getPlayerScore(){
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		PlayerControl playerScript = (PlayerControl)player.GetComponent(typeof(PlayerControl));
		return playerScript.getScore();
	}

	void Pause(){
		this.activeTimer (false);
		this.paused = true;
		ih.Freeze ();

		// markcode
		SfxHandler sfxScript = (SfxHandler)GameObject.FindGameObjectWithTag("GameController").GetComponent(typeof(SfxHandler));
        sfxScript.playAudio(0);
	}
	
	void Unpause(){
		this.activeTimer (true);
		this.paused = false;
		ih.UnFreeze ();
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
			
			
			if (GUI.Button (new Rect (.5f * Screen.width - 200, .35f * Screen.height, 400, .08f * Screen.height), nextLevelString)) {
				if(gcs.getCurrentUnlockedLevel() > Application.loadedLevel){
					Application.LoadLevel (Application.loadedLevel+1);
				} else {
					nextLevelString = "Not Unlocked Yet";
				}
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 200, .55f * Screen.height, 400, .08f * Screen.height), "Return to Menu?")) {
				Application.LoadLevel (0);
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 200, .75f * Screen.height, 400, .08f * Screen.height), "Replay?")) {
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
