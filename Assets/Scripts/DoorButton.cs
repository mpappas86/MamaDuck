using UnityEngine;
using System.Collections;

public class DoorButton : MonoBehaviour {
	
	public GameObject wall_to_remove;    // Pointer to the Wall object that this button removes.
	public float removeSpeed;            // Speed at which to move the Wall down once it is being removed.
	public bool makePassable = true;     // Whether or not to update the tile to be passable once the wall is down.

	private Vector3 wall_inital_position;  // Tracker of the wall's starting position to determine when we've moved it enough.
	private bool contactWithMama = false;  // Whether or not Mama has pressed us yet.
	private bool vert_orientation;         // Whether or not we are oriented vertically - determines which directions the tile becomes
	                                       // passable from once the wall is down.

	void Start(){
		wall_inital_position = wall_to_remove.transform.position;
		// True is left-right orientation, False is up-down
		vert_orientation = (this.transform.localRotation.y == 90);
	}

	// When Mama presses the button, register that and then turn red.
	void OnTriggerEnter (Collider other){
		if (this.contactWithMama) {
			return;
		}
		if (other.gameObject.CompareTag("Player")) {
			this.contactWithMama = true;
			this.gameObject.GetComponent<Renderer>().material.color = Color.red;
		}
	}

	void Update() {
		// Each frame after Mama has touched us, move the wall down slightly more.
		// Once the wall has moved down far enough, remove it entirely and, if necessary, update the passability
		// of its tile.
		if (this.contactWithMama) {
			if (this.wall_to_remove != null) {
				Vector3 end = this.wall_inital_position - new Vector3 (0, this.wall_to_remove.transform.localScale.y * 2, 0);
				this.wall_to_remove.transform.position += (end - this.wall_inital_position)*Time.deltaTime*this.removeSpeed;
				if (this.wall_to_remove.transform.position.y <= end.y){
					if (this.makePassable && this.wall_to_remove.transform.parent != null){
						BaseTileScript bts = ((BaseTileScript)this.wall_to_remove.transform.GetComponentInParent(typeof(BaseTileScript)));
						if(vert_orientation){
							bts.amPassable[2] = true;
							bts.amPassable[3] = true;
						} else {
							bts.amPassable[0] = true;
							bts.amPassable[1] = true;
							bts.amPassable[2] = true;
							bts.amPassable[3] = true;
						}
					}
					Destroy(this.wall_to_remove);
				}
			}
		}
	}
}