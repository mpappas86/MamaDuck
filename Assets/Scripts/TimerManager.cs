using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimerManager : MonoBehaviour {
	
	private float timer;            // Track how long the level has been going.
	public int[] timeCutoffs;       // Times (in seconds) at which point we change the bonus we give for how long the level took
	public int[] cutoffBonuses;     // Corresponding bonuses we offer for each level of timing.
	public Text timerText;          // Pointer to the text object that displays the timer.
	private List<Color> cutoffColors = new List<Color>();  // Colors for the timer to be at each level of timing.
	private bool runTimer = true;   // Whether the timer should currently be running.

	public virtual void Start () {
		if (timeCutoffs.Length != cutoffBonuses.Length) {
			throw new System.Exception("Time cutoffs and cutoff bonuses must have the same length!");
		}
		timer = 0;
		cutoffColors.Add (Color.green);
		cutoffColors.Add (Color.black);
		cutoffColors.Add (Color.red);
	}
	
	// Every frame update the timer.
	public virtual void Update () {
		if (runTimer) {
			timer += Time.deltaTime;
			setTimerText();
		}
	}

	// Determine which cutoff point we're currently at.
	// TODO: Look into doing this more efficiently - perhaps drop cutoff values that we've passed?
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

	// Given the current time, determine the corresponding bonus.
	public int getTimeBonus(){
		int curCutoff = this.getCurCutoff ();
		if (curCutoff == -1){
			return 0;
		}
		return this.cutoffBonuses [curCutoff];
	}
	
	public void setTimerText(){
		this.timerText.text = this.timer.ToString ("F2");  // F2 simply formats a float as a nice string w/ 2 decimal points.
		int curCutoff = this.getCurCutoff();
		if (curCutoff == -1){
			curCutoff = this.cutoffColors.Count - 1;
		}
		this.timerText.color = this.cutoffColors [curCutoff];
	}

	// Accessor for GameController to turn on/off the active timer.
	public void activeTimer(bool active){
		this.runTimer = active;
	}
}
