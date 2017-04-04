using UnityEngine;
using System.Collections;

public class Breadcrumbs : MonoBehaviour {

	// Just rotate breadcrumbs around so they look pretty.
	void Update () 
	{
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}
}