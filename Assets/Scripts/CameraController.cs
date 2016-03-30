using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	private GameObject player;  // Pointer to the player object.
	private Vector3 offset;
	
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		offset = transform.position - player.transform.position;
	}

	// After everything else is done for a frame, move the camera to follow Mama.
	void LateUpdate ()
	{
		transform.position = player.transform.position + offset;
	}
}