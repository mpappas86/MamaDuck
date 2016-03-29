using UnityEngine;
using System.Collections;

public class DucklingScript : BaseTileMover {

	private GameObject mamaWayPoint;    // Pointer to Mama's Waypoint object to track.
	private Vector3 wayPointPos;        // Tracker of most recent Waypoint position.
	public float wanderIntensity;       // Scaling factor from 0-1 defining intensity of wandering.
	private Vector3 wanderDir;          // Current wander direction
	public int numFramesToWander;       // Total number of frames to wander a given direction before changing.
	private int numFramesLeft;          // Frame counter for wandering
	public bool contactWithMama;        // Whether or not we have been collected by Mama yet.
	public float containerRadius;       // Float describing a circle around the duckling's starting position. Ducklings won't
	                                    // wander outside this until Mama finds them.
	private Vector3 initialLocation;    // Duckling's initial location, to be used for the container radius logic.
	private bool shouldFallAndDie = false;  // Indicator of whether we have run into a sewer grate and are currently dying.
	private PlayerControl mamaScript;   // Pointer to the Mama's PlayerControl script so we can access public methods.


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
			// Freeze the rigidbody so the duckling can't be moved by physics at this point.
			// Instead, we handle the falling manually. We might not want this, but for now prevents risks
			// like the mama knocking the duckling out of the way at the last second, causing weirdness.
			this.rb.constraints = RigidbodyConstraints.FreezeAll;
			this.shouldFallAndDie = true;
		} else if (contactWithMama) {
			return;
		// If we make contanct with Mama, make sure we note that it happened and alert Mama that she found us.
		} else if (other.gameObject.CompareTag("Player")) {
			contactWithMama = true;
			mamaScript.ObtainDuckling();
		}
	}
	
	void Update() {
		// Gets called each frame in Update to allow the duckling to progressively "fall".
		if (this.shouldFallAndDie){
			FallAndDie();
		}
	}

	// Handles walking-around motion.
	void FixedUpdate () {
		Vector3 moveTo = this.transform.position;
		// If we've touched Mama, compute the best movement we should take along the tiles.
		// Always try to move towards mama, but if we couldn't move, recompute the direction to move
		// based on the waypoint.
		if (contactWithMama) {
			moveTo = this.GetTileMove ();
			if (!this.isMoving) {
				SetupMoveViaWaypoint();
			}
		}
		// Add on the random movement on top of the tile movement.
		Vector3 randomMovement = DucklingRandomMovement();
		this.rb.MovePosition (moveTo + randomMovement);
	}

	void SetupMoveViaWaypoint() {
		// Note that I use the duckling's y position, to avoid risks of the duckling suddenly flying/falling if Mama
		// ends up at a different height.
		wayPointPos = new Vector3 (mamaWayPoint.transform.position.x, transform.position.y, mamaWayPoint.transform.position.z);

		// There's some annoying math here, but it just boils down to "only move one of {left, right, up, down} at once".
		// Recall that Up, Down, Left, Right = 0, 1, 2, 3 for this.movingDir.
		Vector3 wayPointDir = wayPointPos - transform.position;
		if (Mathf.Abs (wayPointDir [0]) >= Mathf.Abs (wayPointDir [2])) {
			this.movingDir = (int)(2.5 + Mathf.Sign (wayPointDir [0]) * 0.5);
			this.movingVec = new Vector3 (Mathf.Sign (wayPointDir [0]), 0, 0);
		} else {
			this.movingDir = (int)(0.5 - Mathf.Sign (wayPointDir [2]) * 0.5);
			this.movingVec = new Vector3 (0, 0, Mathf.Sign (wayPointDir [2]));
		}
	}

	// Generate random vector to move along, updating it if we're reaching the limit of our container or if we've wandered
	// for the full set of frames.
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

	// Randomly generate a new wander direction. NEVER move along the y axis (aka fly between tiers).
	Vector3 GetWanderDir () {
		return (new Vector3 (Random.Range (-1, 1), 0, Random.Range (-1, 1)) / numFramesToWander) * wanderIntensity;
	}

	// Each time this gets called, it will move the duckling down slightly. Once the duckling has fallen far enough,
	// we actually remove it from the level.
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