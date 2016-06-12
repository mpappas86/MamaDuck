using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	private GameObject player;  // Pointer to the player object.
	private Vector3 offset;

	private bool update_this_frame=true; // Whether to update position this frame. False during pauses
	                                     // where we reset the camera position.

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		offset = transform.position - player.transform.position;
	}

	// After everything else is done for a frame, move the camera to follow Mama.
	void LateUpdate ()
	{
		if (update_this_frame) {
			transform.position = player.transform.position + offset;
		}
	}

	public void SetPositionImmediatelyXZ(Vector3 pos){
		transform.position = new Vector3(pos.x, (player.transform.position + offset).y, pos.z);
	}

	public void Freeze(){
		update_this_frame = false;
	}

	public void UnFreeze(){
		update_this_frame = true;
	}
}