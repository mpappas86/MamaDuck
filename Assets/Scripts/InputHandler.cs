using System;
using UnityEngine;

public class InputHandler: MonoBehaviour
{
	
	private Vector2 initialMousePos; //Track position of initial contact.
	private Vector2 finalMousePos;	//Track position of final contact.
	private Vector2 movingMousePos;	//Track current position during motion.
	private Vector2 deltaMovingMousePos;	//Track change in position since previous frame.
	private bool clickDown = false;	//Is the mouse down/screen being touched?
	private bool movingClick = false;	//Is the mouse moving/screen being swiped?
	private bool clickUp = true;	//Is the mouse up/screen not being touched?
	private bool clickStarted = false;	//Has a click/touch just started?
	private bool clickEnded = false;	//Has a click/touch just ended?
	private bool clickMoved = false;	//Has a click/touch just moved while down?
	private bool onTouchScreen = false;	//Are we on a touch-screen device?
	private bool trigger = false; //Have we input the trigger command for special behavior?
	

	void Awake ()
	{
		DontDestroyOnLoad (transform.gameObject);
	}

	public void Start ()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			this.onTouchScreen = true;
		}
	}

	void Update ()
	{
		if (!onTouchScreen) {
			//If we thought the mouse was up but now it is down, it was just clicked down. So a click started.
			if (clickUp && Input.GetMouseButton (0)) {
				clickEnded = false;
				clickDown = true;
				clickUp = false;	
				clickStarted = true;
				initialMousePos = Input.mousePosition;
				//If we thought the mouse was just clicked down and it is still down in the next frame, it is moving.
			} else if (clickDown && Input.GetMouseButton (0)) {
				clickMoved = true;
				clickDown = false;
				movingClick = true;
				movingMousePos = Input.mousePosition;
				//If the mouse is moving and still down, it is still moving, so update the position and note the change in position.
			} else if (movingClick && Input.GetMouseButton (0)) {
				clickMoved = true;
				deltaMovingMousePos = (Vector2)Input.mousePosition - movingMousePos;
				movingMousePos = Input.mousePosition;
				//If the mouse was moving but is now up, the drag has ended.
			} else if (movingClick && !Input.GetMouseButton (0)) {
				movingClick = false;
				clickUp = true;
				clickEnded = true;
				finalMousePos = Input.mousePosition;;
				//If the mouse was down but is now up, the click has ended.
			} else if (clickDown && !Input.GetMouseButton (0)) {
				clickDown = false;
				clickUp = true;
				clickEnded = true;
				finalMousePos = initialMousePos;
			}
			
			if(Input.GetKeyDown("space")){
				trigger = true;
			}
		} else {
			//If a touch began, mark it down.
			if (Input.GetTouch (0).phase == TouchPhase.Began) {	
				clickEnded = false;
				clickStarted = true;
				clickDown = true;
				initialMousePos = Input.GetTouch (0).position;
				//If a swipe began, mark it down.
			} else if (clickDown && Input.GetTouch (0).phase == TouchPhase.Moved) {
				clickMoved = true;
				clickDown = false;
				movingMousePos = Input.GetTouch (0).position;
				//If a swipe continued, update the position info.
			} else if (!clickDown && Input.GetTouch (0).phase == TouchPhase.Moved) {
				clickMoved = true;
				deltaMovingMousePos = (Vector2)Input.GetTouch (0).position - movingMousePos;
				movingMousePos = Input.GetTouch (0).position;
				//If a touch ended, mark it down.
			} else if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				finalMousePos = Input.GetTouch (0).position;
				clickEnded = true;
			}
			
			if(Input.acceleration.sqrMagnitude > 5){
				trigger = true;
			}
		}
	}
	
	public bool Began ()
	{
		if (clickStarted) {
			clickStarted = false;
			return true;
		}
		return false;
	}
	
	public bool Moved ()
	{
		if (clickMoved) {
			clickMoved = false;
			return true;
		}
		return false;
	}
	
	public bool Ended ()
	{
		if (clickEnded) {
			clickEnded = false;
			return true;
		}
		return false;
	}
	
	public Vector2 startPos ()
	{
		return initialMousePos;
	}
	
	public Vector2 deltaPos ()
	{
		return deltaMovingMousePos;
	}
	
	public Vector2 endPos ()
	{
		return finalMousePos;
	}
	
	public Vector2 currentDragPos ()
	{
		return movingMousePos;
	}
	
	public Vector2 currentHoverPos ()
	{
		return Input.mousePosition;
	}
	
	public bool isTrigger(){
		if(trigger){
			trigger = false;
			return true;
		}
		return false;
	}

	// Down, Up, Left, Right = 0, 1, 2, 3 for dir
	public int getInputDir(){
		float[] vals = getHorizVertMov ();
		float moveHorizontal = vals [0];
		float moveVertical = vals [1];
		int movingDir = -1;
		// I don't want it moving both directions at once, so just move horizontally if both are pressed.
		if (moveHorizontal != 0) {
			movingDir = (int)(2.5 + Mathf.Sign (moveHorizontal) * 0.5);;
		} else if (moveVertical != 0) {
			movingDir = (int)(0.5 - Mathf.Sign (moveVertical) * 0.5);
		}
		return movingDir;
	}
	
	float[] getHorizVertMov(){
		if (Ended ()) {
			Vector2 swipe = endPos () - startPos ();
			Vector2 normalized_swipe = new Vector2 (swipe.x / Screen.width, swipe.y / Screen.height);
			if (normalized_swipe.magnitude >= 0.15) {
				float x = Mathf.Abs (normalized_swipe.x);
				float y = Mathf.Abs (normalized_swipe.y);
				if (x > y) {
					return new float[]{Mathf.Sign (normalized_swipe.x), 0};
				} else {
					return new float[]{0, Mathf.Sign (normalized_swipe.y)};
				}
			}
		} else if (!onTouchScreen) {
			bool left = Input.GetKeyUp (KeyCode.LeftArrow);
			bool right = Input.GetKeyUp (KeyCode.RightArrow);
			bool up = Input.GetKeyUp (KeyCode.UpArrow);
			bool down = Input.GetKeyUp (KeyCode.DownArrow);
			if (left) {
				return new float[]{-1, 0};
			} else if (right) {
				return new float[]{1, 0};
			} else if (up) {
				return new float[]{0, 1};
			} else if (down) {
				return new float[]{0, -1};
			}
			return new float[]{0, 0};
		}
		return new float[]{0, 0};
	}
	
	public int reduceXDir(int dir){
		if(dir == 2){
			return -1;
		} else if (dir == 3){
			return 1;
		}
		return 0;
	}
	
	public int reduceYDir(int dir){
		if(dir == 0){
			return 1;
		} else if (dir == 1){
			return -1;
		}
		return 0;
	}

}


