using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour
{
	private float score; //The total score across multiple levels.
	private int currentUnlockedLevel; //The highest level that has currently been unlocked.
	private int currentLevel; //The level that is currently or most recently played.

	public static GameControllerScript Instance;

	private int vibrations;

	private InputHandler ih;

	void Awake ()
	{
		if (Instance){
			Destroy(gameObject);
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
		if(PlayerPrefs.HasKey("Vibrations")){
			vibrations = PlayerPrefs.GetInt("Vibrations");
		} else {
			vibrations = 1;
		}
		ih = (InputHandler)gameObject.GetComponent(typeof(InputHandler));
	}

	public void SetVibrations (bool val){
		if (val) {
			vibrations = 1;
		} else {
			vibrations = 0;
		}
		PlayerPrefs.SetInt ("Vibrations", vibrations);
	}
	
	public bool GetVibrations(){
		return vibrations == 1;
	}

	public void vibrate(){
		if(GetVibrations()){
			if (ih.onTouchScreen) {
				Handheld.Vibrate();
			} else {
				Debug.Log("Vibrate");
			}
		}
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
}
