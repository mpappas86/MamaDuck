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
	private bool contactWithMama;
	public float containerRadius;
	private Vector3 initialLocation;
	private Rigidbody rb;
	private bool shouldFallAndDie = false;
	private PlayerControl mamaScript;
	
	void Start ()
	{
		rb = this.gameObject.GetComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		mamaWayPoint = GameObject.Find ("wayPoint");
		wanderDir = GetWanderDir();
		numFramesLeft = numFramesToWander;
		contactWithMama = false;
		initialLocation = this.gameObject.transform.position;
		mamaScript = GameObject.Find ("MamaDuck").GetComponent<PlayerControl> ();
	}

	void OnCollisionEnter (Collision other){
		if (contactWithMama) {
			return;
		}
		if (other.gameObject.name == "MamaDuck") {
			contactWithMama = true;
			mamaScript.ducklingCount += 1;
			mamaScript.setMainText();
		}
	}

	void OnTriggerEnter( Collider other){
		if (other.gameObject.CompareTag("Sewer Grate")) {
			rb.constraints = RigidbodyConstraints.FreezeAll;
			this.shouldFallAndDie = true;
		}
	}

	void Update() {
		if (this.shouldFallAndDie){
			FallAndDie();
		}
	}
	
	void FixedUpdate () {
		Vector3 moveDir = Vector3.zero;
		if (contactWithMama) {
			moveDir = MoveViaWaypoint();
		}
		moveDir += DucklingRandomMovement();
		rb.MovePosition (transform.position + moveDir * Time.deltaTime);
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
		return plannedMove;
	}

	Vector3 MoveViaWaypoint() {
		wayPointPos = new Vector3 (mamaWayPoint.transform.position.x, transform.position.y, mamaWayPoint.transform.position.z);
		Vector3 wayPointDir = wayPointPos - transform.position;
		return wayPointDir*speed;
	}

	Vector3 GetWanderDir () {
		return new Vector3 (Random.Range (-1*transform.localScale.x, transform.localScale.x), 0, Random.Range (-1*transform.localScale.z, transform.localScale.z)) * wanderIntensity / numFramesToWander;
	}

	void FallAndDie() {
		transform.position = new Vector3 (transform.position.x, transform.position.y - Time.deltaTime, transform.position.z);
		if (transform.position.y < -2) {
			if (contactWithMama) {
				mamaScript.ducklingCount -= 1;
				mamaScript.MurderDuckling("Sewer Grate");
			}
			GameObject.Destroy(this);
		}
	}
}