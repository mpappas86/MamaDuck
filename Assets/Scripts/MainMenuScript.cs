using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour
	//This script creates the main menu.
{
	
	//Main determines whether we are at the basic main menu (Play vs Settings vs High Scores).
	//Similarly, settingsMenu, levelMenu, and soundTest tell us we are in other deeper menus.
	//backButton determines whether there currently needs to be a button allowing return to the main menu.
	public bool main;
	public bool backButton;
	public bool pressed1;
	public bool pressed2;
	public bool settingsMenu;
	public bool levelMenu;
	public bool soundTest;
	
	public GUISkin customSkin;
	//The audioSources are for the sound test. We need to know what they are, what they are called, etc.
	public AudioSource level1;
	public AudioSource mainTheme;
	public AudioSource endCredits;
	private AudioSource[] allSources;
	private int soundTestGridInt = 0;
	private string[] soundTestStrings = {
		"MainTheme",
		"Level1",
		"endCredits",
	};

	//The guiText object will display information such as "There are no settings yet."
	private string text = "";
	public GUIStyle theGuiTextStyle;
	//The titleStyle determines the format of the title.
	public GUIStyle titleStyle;
	
	//This is for the level grid, which will automatically nicely format our level selection screen.
	private int levelGridInt = -1;
	private string[] levelStrings = {"1", "2", "3", "4"};
	
	private float theVolume = 0.5f;
	private float theVolumeWas = 0.5f;
	private GUIStyle volStyle;

	private GameControllerScript gcScript;
	
	void Start ()
	{
		volStyle = new GUIStyle();
		volStyle.alignment = TextAnchor.LowerCenter;
		volStyle.fontStyle = FontStyle.BoldAndItalic;
		allSources = new AudioSource[3]{mainTheme, level1, endCredits};
		gcScript = (GameControllerScript) GameObject.FindGameObjectWithTag ("GameController").GetComponent(typeof(GameControllerScript));
	}
	
	//Stop all music playing in SoundTest as well as pausing the main menu music.
	private void stopMusic ()
	{
		foreach (AudioSource a in allSources) {
			a.Stop ();
		}
		GetComponent<AudioSource>().Pause ();
	}

	//Essentially unncessary. Might move more logic here eventually, since Update() is called less often than OnGUI() so logic
	//should optimally be executed here as much as possible.
	void Update ()
	{
		if(theVolumeWas != theVolume){
			theVolumeWas = theVolume;
			AudioListener.volume = theVolume;
			//GameControllerScript.Instance.setVolume(theVolume);
		}
	}
	
	void OnGUI ()
	{
		GUI.skin = customSkin;
		//Generate the buttons in locations based on screen size to keep a consistent positioning.
		//Start by declaring the title just as a text box.
		//We also declare a currently empty text box which we will use if we need to put text onscreen anywhere in the menu.
		GUI.Box (new Rect (.1f * Screen.width, .1f * Screen.height, .8f*Screen.width, .15f * Screen.height), "Mama Duck", titleStyle);
		GUI.Box (new Rect (.1f * Screen.width, .25f * Screen.height, .8f*Screen.width, .7f * Screen.height), text, theGuiTextStyle);
		
		//If we are not on the main menu, we need a back button to return to it.
		if (backButton) {
			if (GUI.Button (new Rect (.1f * Screen.width, .8f * Screen.height, .8f*Screen.width, .12f * Screen.height), "Return to Main Menu")) {
				main = true;
				backButton = false;
				levelMenu = false;
				settingsMenu = false;
				soundTest = false;
				text = "";
				stopMusic ();
				GetComponent<AudioSource>().Play ();
			}
		}
		
		//If we are on the main menu, show it.
		//Notice that it is only in these few options that we need to declare the backButton bool as true, since otherwise it just holds its value.
		if (main) {
			if (GUI.Button (new Rect (.1f * Screen.width, .35f * Screen.height, .8f*Screen.width, .12f * Screen.height), "Play Game")) {
				main = false;
				levelMenu = true;
				backButton = true;
			}
			if (GUI.Button (new Rect (.1f * Screen.width, .5f * Screen.height, .8f*Screen.width, .12f * Screen.height), "Options")) {
				settingsMenu = true;
				main = false;
				backButton = true;
			}
		} else if (levelMenu) {
			//The selectionGrid will allow you to choose exactly one of a set of buttons. levelGridInt is initialized to -1 so no level starts out selected.
			int tempInt = GUI.SelectionGrid (new Rect (.1f * Screen.width, .4f * Screen.height, .8f*Screen.width, .4f * Screen.height), levelGridInt, levelStrings, 4);
			//This if statement is used to prevent us from running the remaining logic too often, especially in OnGUI.
			if (tempInt != levelGridInt) {
				if (GameControllerScript.Instance.getCurrentUnlockedLevel () <= tempInt) {
					text = "You haven't unlocked level " + (tempInt + 1).ToString () + " yet.";
				} else {
					switch (tempInt) {
					case 0:
						Application.LoadLevel (1);
						break;
					case 1:
						Application.LoadLevel (2);
						break;
					case 2:
						Application.LoadLevel (3);
						break;
					case 3:
						Application.LoadLevel (4);
						break;
					}
				}
			}
			levelGridInt = tempInt;
		} else if (settingsMenu) {
			GUI.Label(new Rect(.1f * Screen.width, .1f * Screen.height, .8f*Screen.width, .08f * Screen.height), "Volume", volStyle);
			theVolume = GUI.HorizontalSlider(new Rect(.1f * Screen.width, .2f * Screen.height, .8f*Screen.width, .2f * Screen.height), theVolume, 0.0F, 1.0F);
			if (GUI.Button (new Rect (.1f * Screen.width, .3f * Screen.height, .8f*Screen.width, .12f * Screen.height), "Sound Test")) {
				settingsMenu = false;
				soundTest = true;
			}
			bool vibrationSetting = gcScript.GetVibrations();
			string vibrationSettingString = "Disable Vibrations?";
			if(!vibrationSetting){
				vibrationSettingString = "Enable Vibrations?";
			}
			if (GUI.Button (new Rect (.1f * Screen.width, .65f * Screen.height, .8f*Screen.width, .12f * Screen.height), vibrationSettingString)) {
				gcScript.SetVibrations(!vibrationSetting);
			}
		} else if (soundTest) {
			
			//Another selection grid. This time soundTestGridInt is initialized to 0 so MainMenu music starts out selected.
			int tempInt = GUI.SelectionGrid (new Rect (.5f * Screen.width - 125, .3f * Screen.height, 250, .5f * Screen.height), soundTestGridInt, soundTestStrings, 3);
			if (tempInt != soundTestGridInt) {
				stopMusic ();
				switch (tempInt) {
				case 0:
					mainTheme.Play ();
					break;
				case 1:
					level1.Play ();
					break;
				case 2:
					endCredits.Play ();
					break;
				}
			}
			soundTestGridInt = tempInt;
			
		}
		
	}
}