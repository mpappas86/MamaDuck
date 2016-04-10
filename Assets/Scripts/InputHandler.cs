using System;
using UnityEngine;

public class InputHandler
{
	// Up, Down, Left, Right = 0, 1, 2, 3 for dir
	static public int getInputDir(){
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

	static float[] getHorizVertMov(){
		bool left = Input.GetKeyUp (KeyCode.LeftArrow);
		bool right = Input.GetKeyUp (KeyCode.RightArrow);
		bool up = Input.GetKeyUp (KeyCode.UpArrow);
		bool down = Input.GetKeyUp (KeyCode.DownArrow);
		if(left){
			return new float[]{-1, 0};
		} else if(right){
			return new float[]{1, 0};
		} else if (up){
			return new float[]{0, 1};
		} else if (down){
			return new float[]{0, -1};
		}
		return new float[]{0, 0};
	}

	static public int reduceXDir(int dir){
		if(dir == 2){
			return -1;
		} else if (dir == 3){
			return 1;
		}
		return 0;
	}

	static public int reduceYDir(int dir){
		if(dir == 0){
			return -1;
		} else if (dir == 1){
			return 1;
		}
		return 0;
	}
}

