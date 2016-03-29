using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : BaseTileMover {
	
	public GameObject wayPoint;   // Invisible gameobject tracking Mama's location so duckling can follow
	public Text mainText;         // Primary text object
	private int score = 0;        // Player score
	
	private float wayPointTimer = 0.5f;  // How frequently we update the waypoint
	public int breadCountScore = 100;    // How many points bread crumbs are worth.
	public int ducklingCount = 0;        // Tracker for # ducklings we've collected
	public int totalDucklings;           // Total ducklings that can be found in this level.

	public override void Start ()
    {
		base.Start();
		setMainText();
    }
	
	void Update ()
	{
		// Up, Down, Left, Right = 0, 1, 2, 3 for this.movingDir
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		if (!this.isMoving) {
			// I don't want it moving both directions at once, so just move horizontally if both are pressed.
			if (moveHorizontal != 0) {
				this.movingDir = (int)(2.5 + Mathf.Sign (moveHorizontal) * 0.5);
				this.movingVec = new Vector3 (Mathf.Sign (moveHorizontal), 0, 0);
			} else if (moveVertical != 0) {
				this.movingDir = (int)(0.5 - Mathf.Sign (moveVertical) * 0.5);
				this.movingVec = new Vector3 (0, 0, Mathf.Sign (moveVertical));
			}
		}

		// Every wayPointTimer seconds, update the waypoint position.
		if (this.wayPointTimer > 0) {
			this.wayPointTimer -= Time.deltaTime;
		} else {
			UpdateWaypoint ();
			this.wayPointTimer = 0.5f;
		}

	}

	void FixedUpdate()
	{
		// Uses GetTileMove from the BaseTileMover class.
		this.rb.MovePosition (this.GetTileMove ());
	}

	// Public method called by ducklings when mama reaches them so mama can update the score and text.
	// TODO: Move text, score, and methods like this to the GameController?
	public void ObtainDuckling()
	{
		this.ducklingCount += 1;
		this.setMainText ();
	}
		
	// Just set the waypoint to our current position.
	void UpdateWaypoint ()
	{
		this.wayPoint.transform.position = this.transform.position;
	}

	// We only handle collisions with items - ducklings, buttons, etc handle their special behavior when we hit them.
	// TODO: Move this to GameController as well?
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Breadcrumb")) {
			this.score += breadCountScore;
			other.gameObject.SetActive (false);
			setMainText ();
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
		this.mainText.text = DucklingText() + "\nScore: " + this.score.ToString ();
	}

	// Score/text updater in the case that a duckling gets murdered.
	public void MurderDuckling(GameObject duckling, string cause){
		this.ducklingCount -= 1;
		this.mainText.text = "You just lost a duckling to a " + cause + "! You monster!";
	}

	public int getScore(){
		return this.score;
	}
	

}

