using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BaseTileScript : MonoBehaviour {
	
	public bool[] amPassable = {true, true, true, true};  // Whether or not this tile can be passed through via each of the
	                                                      // four possible directions.
	private List<GameObject> adjacent_tiles = null;       // List of all adjacent tiles to this one.
	private Renderer myRenderer;                          // Accessor to renderer to change colors.
	private Color initialColor;                           // Initial color of the tile.
	private float blinkCadence = 0.5f;                    // Rate at which to blink if something changes.
	private bool amBlinking = false;                      // Whether I'm goddamn blinking.
	private bool activateBlinking = false;                // Start blinking when I get turned active?
	
	void Start(){
		myRenderer = this.transform.FindChild("Tile").GetComponent<Renderer> ();
		initialColor = myRenderer.material.color;
	}

	// This can be effectively ignored - it's used by the TileMapInspector Editor script, not used in the game.
	public void Move (int up_down, int left_right) {
		gameObject.transform.position = gameObject.transform.position + new Vector3 (left_right, 0, up_down);
	}

	// Determine if a move is valid given the tile the move would go into, and the direction along which the movement occurs.
	private bool is_valid_move(GameObject tile, int dir){
		if (tile == null) {
			return false;
		} else {
			return ((BaseTileScript)tile.GetComponent(typeof(BaseTileScript))).canPass(dir);
		}
	}

	// Iterate across the tiles that are adjacent to you and report whether the walker can move into each of them.
	public bool[] getValidMoves(bool force=false) {
		if (this.amBlinking) {
			StopCoroutine ("blink");
			this.myRenderer.material.color = this.initialColor;
			this.amBlinking = false;
		}
		if (adjacent_tiles == null || force) {
			adjacent_tiles = getAdjacentTiles();
		}
		bool[] valid_moves = new bool[4];
		valid_moves [0] = is_valid_move (adjacent_tiles [0], 0);
		valid_moves [1] = is_valid_move (adjacent_tiles [1], 1);
		valid_moves [2] = is_valid_move (adjacent_tiles [2], 2);
		valid_moves [3] = is_valid_move (adjacent_tiles [3], 3);
		return valid_moves;
	}

	// Can a walker pass through me from the given direction?
	public bool canPass(int dir){
		return this.amPassable[dir];
	}

	public void blinkUntilSteppedOn(float cadence=0.5f){
		this.blinkCadence = cadence;
		this.amBlinking = true;
		if (this.gameObject.activeInHierarchy) {
			StartCoroutine ("blink");
		} else {
			this.activateBlinking = true;
		}
	}

	private void OnEnable(){
		if (this.activateBlinking) {
			this.activateBlinking = false;
			StartCoroutine ("blink");
		}
	}

	IEnumerator blink(){
		for (;;) {
			Color myColor = this.myRenderer.material.color;
			if (myColor != Color.yellow) {
				this.myRenderer.material.color = Color.yellow;
			} else {
				this.myRenderer.material.color = Color.white;
			}
			yield return new WaitForSeconds (this.blinkCadence);
		}
	}

	// Much less sophisticate than this looks - all it's really doing is, from our tile, checking for any other
	// Tile objects within a distance of 1 (the size of our tiles). I then use dot products and "fuzzy equality"
	// simply to grab the four adjacent (not diagonal) tiles out of all those gameobjects we just found.
	private List<GameObject> getAdjacentTiles () {
		GameObject up_tile = null;
		GameObject down_tile = null;
		GameObject left_tile = null;
		GameObject right_tile = null;
		
		Collider[] adjacentObjects = Physics.OverlapSphere(transform.position, 1f);
		foreach(Collider col in adjacentObjects){
			if (col.gameObject.transform.parent == null){
				continue;
			}
			GameObject colGO = col.gameObject.transform.parent.gameObject;
			if(colGO.CompareTag("Tile")){
				// This vector <1, 2, 4> basically forces the tiles to express themselves in binary - the results of the dot
				// product should be the results of this vector by <+-1,0,0> and <0,0,+-4>, so the resulting values
				// should be +-1 (for left/right tiles) and +-4 (for up/down tiles).
				float dot = Vector3.Dot (transform.position - colGO.transform.position, new Vector3(1, 2, 4));
				if (Mathf.Abs(dot-1) < 0.1){
					left_tile = colGO;
				} else if (Mathf.Abs (dot+1) < 0.1){
					right_tile = colGO;
				} else if (Mathf.Abs (dot-4) < 0.1) {
					down_tile = colGO;
				} else if (Mathf.Abs (dot +4) < 0.1) {
					up_tile = colGO;
				}
			}	
		}
		return new List<GameObject>(new GameObject[] {up_tile, down_tile, left_tile, right_tile});
	}
}
