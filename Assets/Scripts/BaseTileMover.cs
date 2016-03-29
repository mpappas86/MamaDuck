using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseTileMover : MonoBehaviour {
	
	public float speed;
	public Vector3 movingVec;
	public int movingDir = -1;
	public Rigidbody rb;

	private float movedDistance = 0;
	public bool isMoving;
	private bool[] valid_moves;
	
	// Up, Down, Left, Right = 0, 1, 2, 3
	
	public virtual void Start ()
	{
		rb = this.gameObject.GetComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
	}
	
	public void setValidMoves(bool[] new_valid_moves){
		this.valid_moves = new_valid_moves;
	}
	
	public Vector3 GetTileMove (){
		Vector3 moveTo = transform.position;
		if (this.valid_moves == null) {
			return moveTo;
		}
		if (!isMoving) {
			if (movingDir == -1){
				return moveTo;
			}
			if (this.valid_moves[movingDir]){
				isMoving = true;
			}
		}
		if (isMoving) {
			float toMove = speed*Time.deltaTime;
			this.movedDistance += speed*Time.deltaTime;
			if (this.movedDistance >= 1){
				toMove = speed*Time.deltaTime - (movedDistance - 1);
				this.movedDistance = 1;
			}
			moveTo += this.movingVec * toMove;
		}
		if (this.movedDistance == 1) {
			this.isMoving = false;
			this.movingDir = -1;
			this.movingVec = new Vector3(0, 0, 0);
			this.movedDistance = 0;
		}
		return moveTo;
	}
}

