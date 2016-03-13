using UnityEngine;
using System.Collections;

public class DucklingScript : MonoBehaviour {

	private GameObject mamaWayPoint;
	private Vector3 wayPointPos;
	public float speed;
	public float wanderIntensity;
	private Vector3 wanderDir;
	public int numFramesToWander;
	private int numFramesLeft;
	public bool contactWithMama;
	public float containerRadius;
	private Vector3 initialLocation;
	private Rigidbody rb;
	private bool shouldFallAndDie = false;
	private PlayerControl mamaScript;
	private bool[] valid_moves;
	private bool isMoving;
	private Vector3 movingVec;
	private int movingDir;
	private float movedDistance;

	void Start ()
	{
		rb = this.gameObject.GetComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		mamaWayPoint = GameObject.Find ("wayPoint");
		wanderDir = GetWanderDir();
		numFramesLeft = numFramesToWander;
		contactWithMama = false;
		initialLocation = this.gameObject.transform.position;
		mamaScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
	}

	public void setValidMoves(bool[] new_valid_moves){
		this.valid_moves = new_valid_moves;
	}

	void OnTriggerEnter( Collider other){
		if (other.gameObject.CompareTag("Sewer Grate")) {
			rb.constraints = RigidbodyConstraints.FreezeAll;
			this.shouldFallAndDie = true;
		} else if (contactWithMama) {
			return;
		} else if (other.gameObject.CompareTag("Player")) {
			contactWithMama = true;
			mamaScript.ObtainDuckling(this.gameObject);
		}
	}

	void Update() {
		if (this.shouldFallAndDie){
			FallAndDie();
		}
	}
	
	void FixedUpdate () {
		Vector3 coreMovement = transform.position;
		if (contactWithMama) {
			if (this.isMoving) {
				coreMovement = GetCoreMovement();
			} else {
				SetupMoveViaWaypoint();
				this.isMoving = true;
			}
		}
		Vector3 randomMovement = DucklingRandomMovement();
		rb.MovePosition (randomMovement + coreMovement);
	}

	void SetupMoveViaWaypoint() {
		wayPointPos = new Vector3 (mamaWayPoint.transform.position.x, transform.position.y, mamaWayPoint.transform.position.z);
		Vector3 wayPointDir = wayPointPos - transform.position;
		if (Mathf.Abs (wayPointDir[0]) >= Mathf.Abs (wayPointDir[2])) {
			this.movingDir = (int)(2.5 + Mathf.Sign (wayPointDir[0]) * 0.5);
			this.movingVec = new Vector3 (Mathf.Sign (wayPointDir[0]), 0, 0);
		} else {
			this.movingDir = (int)(0.5 - Mathf.Sign (wayPointDir[2]) * 0.5);
			this.movingVec = new Vector3 (0, 0, Mathf.Sign (wayPointDir[2]));
		}
	}

	Vector3 GetCoreMovement (){
		Vector3 coreMovement = transform.position;
		if (this.valid_moves == null) {
			return transform.position;
		}
		if (!isMoving) {
			if (movingDir == -1){
				return transform.position;
			}
			if (this.valid_moves[movingDir]){
				isMoving = true;
			}
		}
		if (isMoving) {
			float toMove = speed * Time.deltaTime;
			this.movedDistance += speed * Time.deltaTime;
			if (this.movedDistance >= 1) {
				toMove = speed * Time.deltaTime - (movedDistance - 1);
				this.movedDistance = 1;
			}
			coreMovement = transform.position + this.movingVec * toMove;
		} else {
			return transform.position;
		}
		if (this.movedDistance == 1) {
			this.isMoving = false;
			this.movingDir = -1;
			this.movingVec = new Vector3(0, 0, 0);
			this.movedDistance = 0;
		}
		return coreMovement;

	}

	Vector3 DucklingRandomMovement(){
		Vector3 plannedMove = wanderDir;
		if (numFramesLeft <= 0) {
			wanderDir = GetWanderDir ();
			numFramesLeft = numFramesToWander;
		} else {
			numFramesLeft -= 1;
		}
		// Very simple check to ensure the duckling doesn't wander too far away until the mama shows up.
		if (!this.contactWithMama) {
			if (Vector3.Distance (this.initialLocation, transform.position) > this.containerRadius) {
				plannedMove = Vector3.zero;
			}
		}
		return plannedMove*Time.deltaTime;
	}

	Vector3 GetWanderDir () {
		return new Vector3 (Random.Range (-1, 1), 0, Random.Range (-1, 1)) / numFramesToWander;
	}

	void FallAndDie() {
		transform.position = new Vector3 (transform.position.x, transform.position.y - Time.deltaTime, transform.position.z);
		if (transform.position.y < -2) {
			if (contactWithMama) {
				mamaScript.MurderDuckling(this.gameObject, "Sewer Grate");
			}
			GameObject.Destroy(this);
		}
	}
}