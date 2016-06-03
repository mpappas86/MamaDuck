using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour
{
	private float score; //The total score across multiple levels.
	private int currentUnlockedLevel; //The highest level that has currently been unlocked.
	private int currentLevel; //The level that is currently or most recently played.

	public static GameControllerScript Instance;
	
	private bool paused = false;
	private float prevTimeRate;

	private int movementHasMomentum;

	void Awake ()
	{
		if (Instance){
			DestroyImmediate(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		//Check PlayerPrefs to see if the three main things are saved. If not, set them to default levels.
		if(PlayerPrefs.HasKey("Score")){
			score = PlayerPrefs.GetFloat("Score");
		} else{
			score = 0;
		}
		if(PlayerPrefs.HasKey("CurrentUnlockedLevel")){
			currentUnlockedLevel = PlayerPrefs.GetInt("CurrentUnlockedLevel");
		} else{
			currentUnlockedLevel = 10;
		}
		if(PlayerPrefs.HasKey("CurrentLevel")){
			currentLevel = PlayerPrefs.GetInt("CurrentLevel");
		} else {
			currentLevel = 0;
		}
		if(PlayerPrefs.HasKey("MovementHasMomentum")){
			movementHasMomentum = PlayerPrefs.GetInt("MovementHasMomentum");
		} else {
			movementHasMomentum = 0;
		}
	}

	public void SetMovementHasMomentum (bool val){
		if (val) {
			movementHasMomentum = 1;
		} else {
			movementHasMomentum = 0;
		}
		PlayerPrefs.SetInt ("MovementHasMomentum", movementHasMomentum);
	}

	public bool GetMovementHasMomentum(){
		return movementHasMomentum == 1;
	}

	void Pause(){
		this.paused = true;
		this.prevTimeRate = Time.timeScale;
		Time.timeScale = 0;
	}
	
	void Unpause(){
		this.paused = false;
		Time.timeScale = this.prevTimeRate;
	}

	//A series of getters and setters to modify the prefabs. 
	//Setters for various ships and other gameObjects will be the upgrade methods called.
	public void prefSetCurrentLevel(int L)
	{
		currentLevel = L;
		PlayerPrefs.SetInt("CurrentLevel", currentLevel);
	}
	public void prefSetCurrentUnlockedLevel(int L)
	{
		currentUnlockedLevel = L;
		PlayerPrefs.SetInt("CurrentUnlockedLevel", currentUnlockedLevel);
	}
	public void prefSetScore(float s)
	{
		score = s;
		PlayerPrefs.SetFloat("Score", score);
	}
	public void setScore(float s){
		score = s;
	}
	public float getScore(){
		return score;
	}
	public int getCurrentLevel(){
		return currentLevel;
	}
	public int getCurrentUnlockedLevel(){
		return currentUnlockedLevel;
	}
	public void levelWin(int levelWon){
		if (levelWon >= currentUnlockedLevel) {
			prefSetCurrentUnlockedLevel(levelWon + 1);
		}
	}

	void OnGUI ()
	{
		if (Application.loadedLevel != 0) {
			if (this.paused) {
				GUI.Window(0, new Rect(.02f*Screen.width, .02f*Screen.height, .96f*Screen.width, .96f*Screen.height), TheMainMenu, "Pause");

			} else {
				if (GUI.Button (new Rect (.9f * Screen.width, .9f * Screen.height, .1f*Screen.width, .05f * Screen.height), "Pause")) {
					// Don't pause if the timescale is already 0 (i.e. we were viewing a different tier.)
					// Allowing a pause would mean we don't know what time scale to return to.
					if (Time.timeScale != 0){
						Pause ();
					}
				}
			}
		}
	}

	void TheMainMenu(int idNum){
		if (GUI.Button (new Rect (.5f * Screen.width - 100, .8f * Screen.height, 200, .12f * Screen.height), "Unpause")) {
			Unpause ();
		}
		if (GUI.Button (new Rect (.5f * Screen.width - 100, .6f * Screen.height, 200, .12f * Screen.height), "Return to Main Menu")) {
			Application.LoadLevel(0);
		}
	}
}
