using UnityEngine;
using System.Collections;

public class DoorButton : MonoBehaviour {
	
	public GameObject wall_to_remove;
	public float removeSpeed;
	private Vector3 wall_inital_position;
	private bool contactWithMama = false;
	public bool makePassable = true;

	void Start(){
		wall_inital_position = wall_to_remove.transform.position;
	}

	void OnTriggerEnter (Collider other){
		if (this.contactWithMama) {
			return;
		}
		if (other.gameObject.name == "MamaDuck") {
			this.contactWithMama = true;
			this.gameObject.GetComponent<Renderer>().material.color = Color.red;
		}
	}

	void Update() {
		if (this.contactWithMama) {
			if (this.wall_to_remove != null) {
				Vector3 end = this.wall_inital_position - new Vector3 (0, this.wall_to_remove.transform.localScale.y * 2, 0);
				this.wall_to_remove.transform.position += (end - this.wall_inital_position)*Time.deltaTime*this.removeSpeed;
				if (this.wall_to_remove.transform.position.y <= end.y){
					if (this.makePassable && this.wall_to_remove.transform.parent != null){
						BaseTileScript bts = ((BaseTileScript)this.wall_to_remove.transform.GetComponentInParent(typeof(BaseTileScript)));
						bts.amPassable = true;
					}
					Destroy(this.wall_to_remove);
				}
			}
		}
	}
}