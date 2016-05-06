using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour
{
	private float score; //The total score across multiple levels.
	private int currentUnlockedLevel; //The highest level that has currently been unlocked.
	private int currentLevel; //The level that is currently or most recently played.

	public static GameControllerScript Instance;

	public GameObject pauseBackground;
	private GameObject pauseBackInstance;
	private bool paused = false;
	private float prevTimeRate;

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
	}

	void Pause(){
		this.paused = true;
		this.prevTimeRate = Time.timeScale;
		Time.timeScale = 0;
		pauseBackInstance = Instantiate (pauseBackground);
	}
	
	void Unpause(){
		this.paused = false;
		Time.timeScale = this.prevTimeRate;
		Destroy (pauseBackInstance);
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
				if (GUI.Button (new Rect (.5f * Screen.width - 100, .8f * Screen.height, 200, .12f * Screen.height), "Unpause")) {
					Unpause ();
				}
				if (GUI.Button (new Rect (.5f * Screen.width - 100, .6f * Screen.height, 200, .12f * Screen.height), "Return to Main Menu")) {
					Application.LoadLevel(0);
				}
			} else {
				if (GUI.Button (new Rect (.9f * Screen.width, .9f * Screen.height, .1f*Screen.width, .05f * Screen.height), "Pause")) {
					Pause ();
				}
			}
		}
	}
}
