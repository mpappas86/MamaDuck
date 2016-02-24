using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(BaseTileScript))]
public class TileMapInspector : Editor {
	
	public override void OnInspectorGUI() {
		//base.OnInspectorGUI();
		DrawDefaultInspector();
		
		if(GUILayout.Button("Left")) {
			BaseTileScript tileMap = (BaseTileScript)target;
			tileMap.Move(0, -1);
		}
		if(GUILayout.Button("Right")) {
			BaseTileScript tileMap = (BaseTileScript)target;
			tileMap.Move(0, 1);
		}
		if(GUILayout.Button("Up")) {
			BaseTileScript tileMap = (BaseTileScript)target;
			tileMap.Move(1, 0);
		}
		if(GUILayout.Button("Down")) {
			BaseTileScript tileMap = (BaseTileScript)target;
			tileMap.Move(-1, 0);
		}
	}
}
