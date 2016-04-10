﻿using UnityEngine;
using System.Collections;

public class DoorButton : MonoBehaviour {
	
	public GameObject[] walls_to_move;    // Pointer to the Wall object that this button removes.
	public float[] move_weight;           // How much weight to give the motion of each wall. -1 moves down, +1 moves up.
	public bool makePassable = true;     // Whether or not to update the tile to be passable once the wall is down.

	private bool contactWithMama = false;  // Whether or not Mama has pressed us yet.
	
	// When Mama presses the button, register that and then turn red.
	void OnTriggerEnter (Collider other){
		if (this.contactWithMama) {
			return;
		}
		if (other.gameObject.CompareTag("Player")) {
			this.contactWithMama = true;
			this.gameObject.GetComponent<Renderer>().material.color = Color.red;
			int wall_index = 0;
			foreach(GameObject wall in this.walls_to_move){
				if (wall != null) {
					wall.SetActive(true);
					WallScript ws = (WallScript)wall.GetComponent(typeof(WallScript));
					ws.getButtoned(this.move_weight[wall_index], makePassable);
				}
				wall_index += 1;
			}
		}
	}
}