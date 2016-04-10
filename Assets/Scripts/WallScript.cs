using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

	private float amt;        // Current "amount" by which the wall has been "moved".
	private BaseTileScript myBTS;  // Accessor to the BaseTileScript of the parent.

	// Use this for initialization
	void Start () {
		this.myBTS = (BaseTileScript)this.transform.GetComponentInParent (typeof(BaseTileScript));
		this.amt = 0;
	}

	// When the amount gets past +-1, the wall is removed/added back, and passability is changed if desired.
	public void getButtoned(float weight, bool makePassable){
		this.amt += weight;
		if (this.amt >= 1) {
			this.amt = 1;
			if(makePassable){
				this.myBTS.amPassable [0] = false;
				this.myBTS.amPassable [1] = false;
				this.myBTS.amPassable [2] = false;
				this.myBTS.amPassable [3] = false;
			}
			this.myBTS.blinkUntilSteppedOn();
			this.gameObject.SetActive (true);
		} else if (this.amt <= -1) {
			this.amt = -1;
			if(makePassable){
				this.myBTS.amPassable [0] = true;
				this.myBTS.amPassable [1] = true;
				this.myBTS.amPassable [2] = true;
				this.myBTS.amPassable [3] = true;
			}
			this.myBTS.blinkUntilSteppedOn();
			this.gameObject.SetActive (false);
		}
	}

}