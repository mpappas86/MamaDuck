using UnityEngine;
using System.Collections;

public class OrientationAdjustment : MonoBehaviour {

	// Really simple script that just enforces LandscapeLeft orientation.
	// Also possible to have it detect the screen orientation, but since portrait looks
	// horrible, better to just enforce.

	void Awaks(){
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
}
