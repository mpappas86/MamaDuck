using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerControl : BaseTileMover {
	
	public GameObject wayPoint;   // Invisible gameobject tracking Mama's location so duckling can follow
	public Text mainText;         // Primary text object
	private int score = 0;        // Player score
	private List<GameObject> myDucklings = new List<GameObject>(); // Ducklings we've obtained if any
	
	private float wayPointTimer = 0.01f;  // How frequently we update the waypoint
	public int breadCountScore = 100;    // How many points bread crumbs are worth.
	public int ducklingCount = 0;        // Tracker for # ducklings we've collected
	public int totalDucklings;           // Total ducklings that can be found in this level.
	
	private GameControllerScript gcs;

    private Animation anims;

	public override void Start ()
    {
		base.Start();
		setMainText();
		GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
		gcs = (GameControllerScript)gc.GetComponent (typeof(GameControllerScript));

        //anims = (Animation)gameObject.GetComponent(typeof(Animation));
        //anims.wrapMode = WrapMode.Loop;
        //anims.Play(anims.GetClip("Idle1"));
    }

	public override void Update ()
	{
		base.Update ();
		// Every wayPointTimer seconds, update the waypoint position.
		if (this.wayPointTimer > 0) {
			this.wayPointTimer -= Time.deltaTime;
		} else {
			UpdateWaypoint ();
			this.wayPointTimer = 0.01f;
		}
	}

	// Public method called by ducklings when mama reaches them so mama can update the score and text.
	// TODO: Move text, score, and methods like this to the GameController?
	public void ObtainDuckling(GameObject duckling)
	{
		this.ducklingCount += 1;
		this.myDucklings.Add(duckling);
		this.setMainText ();
	}
		
	// Just set the waypoint to our current position.
	void UpdateWaypoint ()
	{
		this.wayPoint.transform.position = this.transform.position;
	}

	// We only handle collisions with items - ducklings, buttons, etc handle their special behavior when we hit them.
	// TODO: Move this to GameController as well?
    // Adding EndLevel logic !
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Breadcrumb")) {
			this.score += breadCountScore;
			other.gameObject.SetActive (false);
			setMainText ();
			gcs.vibrate ();
			//  gcScript.levelWin(Application.loadedLevel);
			//  lcScript.endLevel();
		} else if (other.gameObject.CompareTag ("Sewer Grate")) {
			if (ducklingCount > 0){
				MurderDuckling ("sewer grate");
			}
		}
	}

	public override int getGeyserFlightKeyValue(){
		return this.myDucklings.Count;
	}

	public override int getCurrentCrossingKeyValue(){
		return this.myDucklings.Count;
	}
	
	// Text setter based on how many ducklings we've collected.
	string DucklingText(){
		if (this.ducklingCount == 0) {
			return "No ducklings found!";
		} else if (this.ducklingCount == this.totalDucklings) {
			return "You've collected all the ducklings in this level!";
		} else {
			return "You've collected " + this.ducklingCount.ToString() + " ducklings so far!";
		}
	}

	// Set the text that explains our duckling count and score.
	public void setMainText(){
		this.mainText.text = DucklingText() + "\nScore: " + this.getScore().ToString ();
	}

	// Score/text updater in the case that a duckling gets murdered.
	public void MurderDuckling(string cause){
		this.ducklingCount -= 1;
		foreach (GameObject duckling in this.myDucklings) {
			duckling.SetActive(false);
		}
		this.mainText.text = "You just lost a duckling to a " + cause + "! You monster!";
	}

	public int getScore(){
		return this.score + 500 * this.ducklingCount;
	}
	

}

