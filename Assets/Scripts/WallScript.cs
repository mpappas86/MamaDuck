using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

	private float amt;
	private BaseTileScript myBTS;

	// Use this for initialization
	void Start () {
		this.myBTS = (BaseTileScript)this.transform.GetComponentInParent (typeof(BaseTileScript));
		this.amt = 0;
	}

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
			this.gameObject.SetActive (true);
		} else if (this.amt <= -1) {
			this.amt = -1;
			if(makePassable){
				this.myBTS.amPassable [0] = true;
				this.myBTS.amPassable [1] = true;
				this.myBTS.amPassable [2] = true;
				this.myBTS.amPassable [3] = true;
			}
			this.gameObject.SetActive (false);
		}
	}

}