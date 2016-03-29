using UnityEngine;
using System.Collections;

public class DucklingScript : BaseTileMover {

	private GameObject mamaWayPoint;
	private Vector3 wayPointPos;
	public float wanderIntensity;
	private Vector3 wanderDir;
	public int numFramesToWander;
	private int numFramesLeft;
	public bool contactWithMama;
	public float containerRadius;
	private Vector3 initialLocation;
	private bool shouldFallAndDie = false;
	private PlayerControl mamaScript;


	public override void Start ()
	{
		base.Start ();
		this.mamaWayPoint = GameObject.Find ("wayPoint");
		this.wanderDir = GetWanderDir();
		this.numFramesLeft = numFramesToWander;
		this.contactWithMama = false;
		this.initialLocation = this.gameObject.transform.position;
		this.mamaScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
	}

	void OnTriggerEnter( Collider other){
		if (other.gameObject.CompareTag("Sewer Grate")) {
			this.rb.constraints = RigidbodyConstraints.FreezeAll;
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
		Vector3 moveTo = this.transform.position;
		if (contactWithMama) {
			moveTo = this.GetTileMove ();
			if (!this.isMoving) {
				SetupMoveViaWaypoint();
			}
		}
		Vector3 randomMovement = DucklingRandomMovement();
		this.rb.MovePosition (moveTo + randomMovement);
	}

	void SetupMoveViaWaypoint() {
		wayPointPos = new Vector3 (mamaWayPoint.transform.position.x, transform.position.y, mamaWayPoint.transform.position.z);
		Vector3 wayPointDir = wayPointPos - transform.position;
		if (Mathf.Abs (wayPointDir [0]) >= Mathf.Abs (wayPointDir [2])) {
			this.movingDir = (int)(2.5 + Mathf.Sign (wayPointDir [0]) * 0.5);
			this.movingVec = new Vector3 (Mathf.Sign (wayPointDir [0]), 0, 0);
		} else {
			this.movingDir = (int)(0.5 - Mathf.Sign (wayPointDir [2]) * 0.5);
			this.movingVec = new Vector3 (0, 0, Mathf.Sign (wayPointDir [2]));
		}
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
		return (new Vector3 (Random.Range (-1, 1), 0, Random.Range (-1, 1)) / numFramesToWander) * wanderIntensity;
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