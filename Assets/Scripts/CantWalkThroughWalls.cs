using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CantWalkThroughWalls : MonoBehaviour {
	public void TryMove (Vector3 proposedDir) {
		Vector3 proposedEndpoint = this.gameObject.transform.position + proposedDir;
		Vector3 direction = proposedDir; // This is the direction from start to goal.
		direction = Vector3.Scale(direction, this.gameObject.transform.localScale);
		Vector3 point = this.gameObject.transform.position;

		RaycastHit hit;
		if (Physics.Linecast (point, proposedEndpoint, out hit)) {
			if (hit.transform.gameObject.CompareTag("Wall")){
				return;
			}
		}
		this.gameObject.transform.position += proposedDir;
	}
}

