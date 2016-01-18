using UnityEngine;
using System.Collections;

public class DucklingScript : MonoBehaviour {

	private GameObject mamaWayPoint;
	private Vector3 wayPointPos;
	public float speed;
	private Vector3 wanderDir;
	public int numFramesToWander;
	private int numFramesLeft;
	private bool contactWithMama;

	void Start () {
		mamaWayPoint = GameObject.Find ("wayPoint");
		wanderDir = GetWanderDir();
		numFramesLeft = numFramesToWander;
		contactWithMama = false;
	}

	void OnCollisionEnter (Collision other){
		if (other.gameObject.name == "MamaDuck") {
			contactWithMama = true;
		}
	}

	void Update () {
		if (contactWithMama) {
			MoveViaWaypoint();
		}
		DucklingRandomMovement();
	}

	void DucklingRandomMovement(){
		transform.position += wanderDir;
		if (numFramesLeft <= 0) {
			wanderDir = GetWanderDir ();
			numFramesLeft = numFramesToWander;
		} else {
			numFramesLeft -= 1;
		}
	}

	void MoveViaWaypoint() {
		wayPointPos = new Vector3 (mamaWayPoint.transform.position.x, transform.position.y, mamaWayPoint.transform.position.z);
		transform.position = Vector3.MoveTowards (transform.position, wayPointPos, speed * Time.deltaTime);
	}

	Vector3 GetWanderDir () {
		return new Vector3 (Random.Range (0, transform.localScale.x)/numFramesToWander, 0, Random.Range (0, transform.localScale.z)/numFramesToWander);
	}
}