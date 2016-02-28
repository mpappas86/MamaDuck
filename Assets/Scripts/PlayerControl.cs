using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    public float speed;
	public GameObject wayPoint;
	public Text mainText;
	private int score = 0;
	
	private float wayPointTimer = 0.5f;
	public int breadCountScore = 100;
	private Vector3 movingVec;
	private int movingDir = -1;
	private float movedDistance = 0;
	private bool isMoving;
	public int ducklingCount = 0;
	public int totalDucklings;
	private Rigidbody rb;
	private bool[] valid_moves;

	// Up, Down, Left, Right = 0, 1, 2, 3

	void Start ()
    {
		setMainText();
		rb = this.gameObject.GetComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
	
	void Update ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		if (!isMoving) {
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

	public void setValidMoves(bool[] new_valid_moves){
		this.valid_moves = new_valid_moves;
	}

	void FixedUpdate (){
		if (this.valid_moves == null) {
			return;
		}
		if (!isMoving) {
			if (movingDir == -1){
				return;
			}
			if (this.valid_moves[movingDir]){
				isMoving = true;
			}
		}
		if (isMoving) {
			float toMove = speed*Time.deltaTime;
			this.movedDistance += speed*Time.deltaTime;
			if (this.movedDistance >= 1){
				toMove = speed*Time.deltaTime - (movedDistance - 1);
				this.movedDistance = 1;
			}
			this.rb.MovePosition (transform.position + this.movingVec * toMove);
		}
		if (this.movedDistance == 1) {
			this.isMoving = false;
			this.movingDir = -1;
			this.movingVec = new Vector3(0, 0, 0);
			this.movedDistance = 0;
		}
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

