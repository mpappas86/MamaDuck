﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Base class for anything that moves along the tiles while checking whether or not it's allowed on the next one.
public class BaseTileMover : MonoBehaviour {
	
	public float speed;
	public Vector3 movingVec;
	public int movingDir = -1;
	public Rigidbody rb;

	private float movedDistance = 0;
	public bool isMoving;
	private int wasMovingDir = -1;
	public bool[] valid_moves;

	public bool momentumMoving = false;
	private int momentumCountdown = 20;
	private InputHandler ih;
	
	// Up, Down, Left, Right = 0, 1, 2, 3
	
	public virtual void Start ()
	{
		rb = this.gameObject.GetComponent<Rigidbody> ();
		// TileMoving objects should never have physics push them in the y direction, nor should it rotate them.
		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
		ih = (InputHandler)gc.GetComponent(typeof(InputHandler));
	}

	public virtual void Update ()
	{
		if (!this.isMoving && !momentumMoving) {
			int dir = ih.getInputDir ();
			if (dir != -1){
				this.movingDir = dir;
				this.movingVec = new Vector3 (ih.reduceXDir (this.movingDir), 0, ih.reduceYDir (this.movingDir));
			}
		}
	}
	
	void FixedUpdate()
	{
		if (this.movingDir != -1) {
			wasMovingDir = this.movingDir;
		}
		// Uses GetTileMove from the BaseTileMover class.
		this.rb.MovePosition (this.GetTileMove ());
		if (momentumMoving) {
			if (this.movingDir == -1 && this.wasMovingDir != -1) {
				this.momentumCountdown -= 1;
				if (this.momentumCountdown == 0){
					this.movingDir = wasMovingDir;
					this.movingVec = new Vector3 (ih.reduceXDir (this.movingDir), 0, ih.reduceYDir (this.movingDir));
					this.momentumCountdown = 20;
				}
			}
		}
	}

	// Public setter for TileTrigger - TileTrigger calls this to inform us where we can go each time we step onto a new tile. 
	public void setValidMoves(bool[] new_valid_moves){
		this.valid_moves = new_valid_moves;
	}

	public void setTileQuality(string tileQuality){
		if (tileQuality == "slide") {
			momentumMoving = true;
		} else if (tileQuality == "stop") {
			momentumMoving = false;
		}
	}

	// Compute where we should move to next. Gets called within FixedUpdate, aka once per physics frame.
	public Vector3 GetTileMove (){
		Vector3 destination = transform.position;
		// If there are no valid moves to take, or we don't know what the valid moves are, don't move.
		if (this.valid_moves == null) {
			return destination;
		}
		// If we're not currently moving, check if there's a direction we'd like to move (movingDir).
		// If not, just don't move, but if so (and the desired direction is a valid move), then let's move there!
		if (!isMoving) {
			if (movingDir == -1){
				return destination;
			}
			if (this.valid_moves[movingDir]){
				isMoving = true;
			} else {
				momentumMoving = false;
			}
		}
		// If we have a valid direction and a desire to move, let's do it!
		if (isMoving) {
			float toMove = speed*Time.deltaTime;
			// We track movedDistance so that if we ever overshoot the distance of 1 (the size of our tiles), we can correct.
			// This prevents us from ever actually overshooting a single tile (which might let us, say, think that it's ok to
			// walk to the right, and then keep going through the next tile, which is a wall).
			this.movedDistance += toMove;
			if (this.movedDistance >= 1){
				toMove = speed*Time.deltaTime - (movedDistance - 1);
				this.movedDistance = 1;
			}
			destination += this.movingVec * toMove;
		}
		// If we've finished traversing this tile, reset all the relevant vars - especially isMoving, which shows that we're
		// no longer actively walking on any tile.
		if (this.movedDistance == 1) {
			this.isMoving = false;
			this.movingDir = -1;
			this.movingVec = new Vector3(0, 0, 0);
			this.movedDistance = 0;
		}
		return destination;
	}
}

