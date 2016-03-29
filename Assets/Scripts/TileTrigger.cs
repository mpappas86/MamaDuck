using UnityEngine;
using System.Collections;

public class TileTrigger : MonoBehaviour {

	public bool amStartingTile = false;  // Designates whether or not this tile is the tile Mama starts on.
	private BaseTileScript bts;          // Pointer to this Tile's BaseTileScript for access to public methods.

	void Start(){
		bts = (BaseTileScript) this.gameObject.GetComponentInParent (typeof(BaseTileScript));
		// If this is the starting tile, then on startup of the game, we need to tell Mama what her valid moves are.
		// We do this by effectivelly "triggering" as if Mama had just walked onto us.
		if (amStartingTile) {
			OnTriggerEnter(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>());
		}
	}

	// If a BaseTileMover steps onto us, we set their valid moves.
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			setValidMoves(other, bts.getValidMoves());
		} else if (other.CompareTag("Duckling")) {
			setValidMoves(other, bts.getValidMoves());
		}
	}

	void setValidMoves(Collider obj, bool[] valid_moves){
		BaseTileMover btm = (BaseTileMover) obj.GetComponent(typeof(BaseTileMover));
		btm.setValidMoves(valid_moves);
	}

}
