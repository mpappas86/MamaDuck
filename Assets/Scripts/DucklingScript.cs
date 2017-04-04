using UnityEngine;
using System.Collections;

public class DucklingScript : MonoBehaviour {

	private GameObject mamaWayPoint;    // Pointer to Mama's Waypoint object to track.
	private Vector3 wayPointPos;        // Tracker of most recent Waypoint position.
	public bool contactWithMama = false;// Whether or not we have been collected by Mama yet.
	private PlayerControl mamaScript;   // Pointer to the Mama's PlayerControl script so we can access public methods.
    private SfxHandler sfxScript;       // Sound effects manager

	public void Start ()
	{
		this.mamaWayPoint = GameObject.Find ("wayPoint");
		this.mamaScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();

        //markcode: set the sfxScript varialbe to the SfxHandler script attached to the game controller
        this.sfxScript = (SfxHandler)GameObject.FindGameObjectWithTag("GameController").GetComponent(typeof(SfxHandler));
	}

	void OnTriggerEnter( Collider other){
		if (contactWithMama) {
			return;
		// If we make contanct with Mama, make sure we note that it happened and alert Mama that she found us.
		} else if (other.gameObject.CompareTag("Player")) {
			contactWithMama = true;
			mamaScript.ObtainDuckling(this.gameObject);
            // markcode: play the duckling_pickup audio which happens to be number 3
            sfxScript.playAudio(3);
        }
	}

	// Handles walking-around motion.
	void FixedUpdate () {
		// If we've touched Mama, compute the best movement we should take along the tiles.
		// Always try to move towards mama, but if we couldn't move, recompute the direction to move
		// based on the waypoint.
		if (contactWithMama) {
			MoveViaWaypoint();
		}
	}

	void MoveViaWaypoint() {
		// Note that I use the duckling's y position, to avoid risks of the duckling suddenly flying/falling if Mama
		// ends up at a different height.
		wayPointPos = new Vector3 (mamaWayPoint.transform.position.x, transform.position.y, mamaWayPoint.transform.position.z);
		this.transform.position = wayPointPos;
	}
}