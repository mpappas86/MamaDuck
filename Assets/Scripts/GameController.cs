using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	private float timer;

	public GameObject[] tiers;
	public int currentTier = 0;
	public int[] timeCutoffs; // in seconds
	public int[] cutoffBonuses;
	public Text timerText;
	private List<Color> cutoffColors = new List<Color>();

	private bool runTimer = true;
	private bool levelOver = false;
	private int finalScore = 0;

	void Start () {
		if (timeCutoffs.Length != cutoffBonuses.Length) {
			throw new System.Exception("Time cutoffs and cutoff bonuses must have the same length!");
		}
		Time.timeScale = 1;
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

	public void TierChange(bool up){
		int up_down = up ? 1 : -1;
		GameObject newTier = tiers [currentTier + up_down];
		GameObject oldTier = tiers [currentTier];
		float ydiff = newTier.transform.position.y - oldTier.transform.position.y;
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		player.transform.position += new Vector3 (0, ydiff, 0);
		GameObject[] ducklings = GameObject.FindGameObjectsWithTag("Duckling");
		foreach (GameObject ducky in ducklings) {
			DucklingScript dc = ducky.GetComponent<DucklingScript>();
			if (dc.contactWithMama){
				dc.transform.position += new Vector3 (0, ydiff, 0);
			}
		}
		newTier.SetActive (true);
		oldTier.SetActive (false);
		this.currentTier = currentTier + up_down;
		
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

	public void endLevel(){
		this.runTimer = false;
		this.levelOver = true;
		this.finalScore = this.getTimeBonus () + this.getPlayerScore ();
		Time.timeScale = 0;
	}

	private int getPlayerScore(){
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		PlayerControl playerScript = (PlayerControl)player.GetComponent(typeof(PlayerControl));
		return playerScript.getScore();
	}

	void OnGUI ()
	{
		if (levelOver) {
			GUIStyle winStyle = new GUIStyle ();
			winStyle.fontStyle = FontStyle.BoldAndItalic;
			winStyle.fontSize = 20;
			winStyle.alignment = TextAnchor.UpperCenter;
			string winText = "You Won!\nFinal score: " + this.finalScore.ToString();
			GUI.Box (new Rect (.5f * Screen.width - 200, .1f * Screen.height, 400, .18f * Screen.height), winText, winStyle);
			
			
			if (GUI.Button (new Rect (.5f * Screen.width - 200, .75f * Screen.height, 400, .08f * Screen.height), "Return to Menu?")) {
				Application.LoadLevel ("MainMenu");
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 200, .55f * Screen.height, 400, .08f * Screen.height), "Replay?")) {
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}
}
