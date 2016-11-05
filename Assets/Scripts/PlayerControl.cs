using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : BaseTileMover {
	
	public GameObject wayPoint;   // Invisible gameobject tracking Mama's location so duckling can follow
	public Text mainText;         // Primary text object
	private int score = 0;        // Player score
	private GameObject myDuckling; // Duckling we've obtained if any
	
	private float wayPointTimer = 0.01f;  // How frequently we update the waypoint
	public int breadCountScore = 100;    // How many points bread crumbs are worth.
	public int ducklingCount = 0;        // Tracker for # ducklings we've collected
	public int totalDucklings;           // Total ducklings that can be found in this level.

	private InputHandler ih;
	private GameControllerScript gcs;

	private int wasMovingDir = -1;
	private int momentumCountdown = 20;

	public override void Start ()
    {
		base.Start();
		setMainText();
		GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
		ih = (InputHandler)gc.GetComponent(typeof(InputHandler));
		gcs = (GameControllerScript)gc.GetComponent (typeof(GameControllerScript));
    }
	
	void Update ()
	{
		if (!this.isMoving) {
			int dir = ih.getInputDir ();
			if (dir != -1){
				this.movingDir = dir;
				this.movingVec = new Vector3 (ih.reduceXDir (this.movingDir), 0, ih.reduceYDir (this.movingDir));
			}
		}

		// Every wayPointTimer seconds, update the waypoint position.
		if (this.wayPointTimer > 0) {
			this.wayPointTimer -= Time.deltaTime;
		} else {
			UpdateWaypoint ();
			this.wayPointTimer = 0.01f;
		}

	}

	void FixedUpdate()
	{
		if (this.movingDir != -1) {
			wasMovingDir = this.movingDir;
		}
		// Uses GetTileMove from the BaseTileMover class.
		this.rb.MovePosition (this.GetTileMove ());
		if (this.gcs.GetMovementHasMomentum ()) {
			if (this.movingDir == -1 && this.wasMovingDir != -1) {
				this.momentumCountdown -= 1;
				if (this.momentumCountdown == 0){
					this.movingDir = wasMovingDir;
					this.movingVec = new Vector3 (ih.reduceXDir (this.movingDir), 0, ih.reduceYDir (this.movingDir));
					this.momentumCountdown = 20;
				}
			}
		}
	}

	// Public method called by ducklings when mama reaches them so mama can update the score and text.
	// TODO: Move text, score, and methods like this to the GameController?
	public void ObtainDuckling(GameObject duckling)
	{
		this.ducklingCount += 1;
		this.myDuckling = duckling;
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
			MurderDuckling("sewer grate");
		}
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
		this.myDuckling.SetActive (false);
		this.mainText.text = "You just lost a duckling to a " + cause + "! You monster!";
	}

	public int getScore(){
		return this.score + 500 * this.ducklingCount;
	}
	

}

