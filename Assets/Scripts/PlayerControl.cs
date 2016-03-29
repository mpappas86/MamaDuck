using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : BaseTileMover {
	
	public GameObject wayPoint;
	public Text mainText;
	private int score = 0;
	
	private float wayPointTimer = 0.5f;
	public int breadCountScore = 100;
	public int ducklingCount = 0;
	public int totalDucklings;

	// Up, Down, Left, Right = 0, 1, 2, 3

	public override void Start ()
    {
		base.Start();
		setMainText();
    }
	
	void Update ()
	{
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

		if (this.wayPointTimer > 0) {
			this.wayPointTimer -= Time.deltaTime;
		} else {
			UpdateWaypoint ();
			this.wayPointTimer = 0.5f;
		}

	}

	void FixedUpdate()
	{
		this.rb.MovePosition (this.GetTileMove ());
	}
	
	public void ObtainDuckling(GameObject duckling){	
		this.ducklingCount += 1;
		this.setMainText ();
	}
		
		void UpdateWaypoint ()
	{
		this.wayPoint.transform.position = this.transform.position;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Breadcrumb")) {
			this.score += breadCountScore;
			other.gameObject.SetActive (false);
			setMainText ();
		}
	}

	string DucklingText(){
		if (this.ducklingCount == 0) {
			return "No ducklings found!";
		} else if (this.ducklingCount == this.totalDucklings) {
			return "You've collected all the ducklings in this level!";
		} else {
			return "You've collected " + this.ducklingCount.ToString() + " ducklings so far!";
		}
	}

	public void setMainText(){
		this.mainText.text = DucklingText() + "\nScore: " + this.score.ToString ();
	}

	public void MurderDuckling(GameObject duckling, string cause){
		this.ducklingCount -= 1;
		this.mainText.text = "You just lost a duckling to a " + cause + "! You monster!";
	}

	public int getScore(){
		return this.score;
	}
	

}

