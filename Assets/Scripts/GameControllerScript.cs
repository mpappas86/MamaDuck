//using UnityEngine;
//using System.Collections;
//
//public class GameControllerScript : Singleton<GameControllerScript>
//{
//	protected GameControllerScript(){}
//	private int currentUnlockedLevel; //The highest level that has currently been unlocked.
//	private int currentLevel; //The level that is currently or most recently played.
//	private float gameVolume; //The relative volume level for the game.
//	
//	void Awake ()
//	{
//		DontDestroyOnLoad(this);
//
//		//Check PlayerPrefs to see if the three main things are saved. If not, set them to default levels.
//		if(PlayerPrefs.HasKey("Score")){
//			score = PlayerPrefs.GetFloat("Score");
//		} else{
//			score = 10;
//		}
//		if(PlayerPrefs.HasKey("Volume")){
//			gameVolume = PlayerPrefs.GetFloat("Volume");
//		} else{
//			gameVolume = 0.5f;
//		}
//		if(PlayerPrefs.HasKey("CurrentUnlockedLevel")){
//			currentUnlockedLevel = PlayerPrefs.GetInt("CurrentUnlockedLevel");
//		} else{
//			currentUnlockedLevel = 1;
//		}
//		if(PlayerPrefs.HasKey("CurrentLevel")){
//			currentLevel = PlayerPrefs.GetInt("CurrentLevel");
//		} else {
//			currentLevel = 0;
//		}
//
//		LoadSettings();
//	}
//	
//	private void LoadSettings(){
//		AudioListener.volume = gameVolume;
//	}
//	
//	public void setVolume(float V){
//		gameVolume = V;
//		AudioListener.volume = gameVolume;
//	}
//	
//	//A series of getters and setters to modify the prefabs. 
//	//Setters for various ships and other gameObjects will be the upgrade methods called.
//	public void prefSetCurrentLevel(int L)
//	{
//		currentLevel = L;
//		PlayerPrefs.SetInt("CurrentLevel", currentLevel);
//	}
//	public void prefSetCurrentUnlockedLevel(int L)
//	{
//		currentUnlockedLevel = L;
//		PlayerPrefs.SetInt("CurrentUnlockedLevel", currentUnlockedLevel);
//	}
//	public void prefSetScore(float s)
//	{
//		score = s;
//		PlayerPrefs.SetFloat("Score", score);
//	}
//	public void setScore(float s){
//		score = s;
//	}
//	public void setCurrentUnlockedLevel(int L){
//		currentUnlockedLevel = L;
//	}
//	public void setCurrentLevel(int L){
//		currentLevel = L;
//	}
//	public float getScore(){
//		return score;
//	}
//	public int getCurrentLevel(){
//		return currentLevel;
//	}
//	public int getCurrentUnlockedLevel(){
//		return currentUnlockedLevel;
//	}
//}