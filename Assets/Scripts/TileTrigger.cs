using UnityEngine;
using System.Collections;

public class TileTrigger : MonoBehaviour {

	public GameObject amStartingTileFor = null;  // Designates whether or not this tile is the tile Mama starts on.
	private BaseTileScript bts;          // Pointer to this Tile's BaseTileScript for access to public methods.
    private SfxHandler sfxScript;

	void Start(){
		bts = (BaseTileScript) this.gameObject.GetComponentInParent (typeof(BaseTileScript));
		// If this is the starting tile, then on startup of the game, we need to tell Mama what her valid moves are.
		// We do this by effectivelly "triggering" as if Mama had just walked onto us.
		if (amStartingTileFor != null) {
			OnTriggerEnter(amStartingTileFor.GetComponent<Collider>());
		}

        this.sfxScript = (SfxHandler)GameObject.FindGameObjectWithTag("GameController").GetComponent(typeof(SfxHandler));
    }

	// If a BaseTileMover steps onto us, we set their valid moves.
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			setCurrentStatus(other, bts.getValidMoves(), bts.currentTileQuality());
		}
	}

	void setCurrentStatus(Collider obj, string[] valid_moves, string tileQuality){
		BaseTileMover btm = (BaseTileMover) obj.GetComponent(typeof(BaseTileMover));
		btm.setValidMoves(valid_moves);
        if (tileQuality == "current") {
            int duckling_threshold = bts.getDucklingThreshold();
            if (duckling_threshold > 0) {
                btm.setTileQuality(tileQuality, bts.getCurrentDirection(), duckling_threshold);
            } else {
                btm.setTileQuality(tileQuality, bts.getCurrentDirection());
            }
        } else if (tileQuality == "geyser") {
            btm.setTileQuality(tileQuality, bts.geyserMultiplier);  
        } else {
			btm.setTileQuality (tileQuality);
		}

        if (tileQuality == "slide") { sfxScript.playAudio(7); }
	}

}
