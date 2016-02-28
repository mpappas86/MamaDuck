using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	private float timer;

	public int[] timeCutoffs; // in seconds
	public int[] cutoffBonuses;
	public Text timerText;
	private List<Color> cutoffColors = new List<Color>();

	private bool runTimer = true;

	void Start () {
		if (timeCutoffs.Length != cutoffBonuses.Length) {
			throw new System.Exception("Time cutoffs and cutoff bonuses must have the same length!");
		}
		timer = 0;
		cutoffColors.Add (Color.green);
		cutoffColors.Add (Color.black);
		cutoffColors.Add (Color.red);
	}

	void Update () {
		if (runTimer) {
			timer += Time.deltaTime;
			setTimerText();
		}
	}

	private int getCurCutoff(){
		int index = 0;
		foreach (float cutoff in timeCutoffs) {
			if (this.timer >= cutoff){
				index += 1;
				continue;
			} else {
				return index;
			}
		}
		return -1;
	}

	public int getTimeBonus(){
		int curCutoff = this.getCurCutoff ();
		if (curCutoff == -1){
			return 0;
		}
		return this.cutoffBonuses [curCutoff];
	}

	public void setTimerText(){
		this.timerText.text = this.timer.ToString ("F2");
		int curCutoff = this.getCurCutoff();
		if (curCutoff == -1){
			curCutoff = this.cutoffColors.Count - 1;
		}
		this.timerText.color = this.cutoffColors [curCutoff];
	}
}
