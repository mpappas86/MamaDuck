using UnityEngine;
using System.Collections;

public class TileTrigger : MonoBehaviour {

	public bool amStartingTile = false;

	void Start(){
		if (amStartingTile) {
			OnTriggerEnter(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>());
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			BaseTileScript bts = (BaseTileScript) this.gameObject.GetComponentInParent (typeof(BaseTileScript));
			bool[] valid_moves = bts.MamaDuckEntered ();
			PlayerControl pc = (PlayerControl) other.GetComponent(typeof(PlayerControl));
			pc.setValidMoves(valid_moves);
		}
	}
}
