using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BaseTileScript : MonoBehaviour {
	
	public bool[] amPassable = {true, true, true, true};
	private List<GameObject> adjacent_tiles = null;

	public void Move (int up_down, int left_right) {
		gameObject.transform.position = gameObject.transform.position + new Vector3 (left_right, 0, up_down);
	}

	private bool is_valid_move(GameObject tile, int dir){
		if (tile == null) {
			return false;
		} else {
			return ((BaseTileScript)tile.GetComponent(typeof(BaseTileScript))).canPass(dir);
		}
	}

	public bool[] MamaDuckEntered(bool force=false) {
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

	public bool canPass(int dir){
		return this.amPassable[dir];
	}

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
