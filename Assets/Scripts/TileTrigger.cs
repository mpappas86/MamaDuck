using UnityEngine;
using System.Collections;

public class TileTrigger : MonoBehaviour {

	public bool amStartingTile = false;
	private BaseTileScript bts;

	void Start(){
		bts = (BaseTileScript) this.gameObject.GetComponentInParent (typeof(BaseTileScript));
		if (amStartingTile) {
			OnTriggerEnter(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>());
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			setValidMoves(other, bts.MamaDuckEntered());
		} else if (other.CompareTag("Duckling")) {
			setValidMoves(other, bts.MamaDuckEntered());
		}
	}

	void setValidMoves(Collider obj, bool[] valid_moves){
		BaseTileMover btm = (BaseTileMover) obj.GetComponent(typeof(BaseTileMover));
		btm.setValidMoves(valid_moves);
	}

}
