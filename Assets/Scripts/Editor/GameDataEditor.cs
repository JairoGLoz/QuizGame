using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GameDataEditor : EditorWindow {

	private string gameDataProjectFilePath = "/StreamingAssets/data.json";
	public GameData gameData;

	[MenuItem ("Window/Game Data Editor")]
	static void Init(){
		GameDataEditor window = (GameDataEditor)EditorWindow.GetWindow (typeof(GameDataEditor));
		window.Show ();
	}// end of Init

	void OnGUI() {
		if (gameData != null) {
			SerializedObject serializedObject = new SerializedObject (this);
			SerializedProperty serializedProperty = serializedObject.FindProperty ("gameData");

			EditorGUILayout.PropertyField (serializedProperty, true);

			serializedObject.ApplyModifiedProperties ();

			if (GUILayout.Button ("Save data")) {
				SaveGameData ();
			}
		}// end of if

		if (GUILayout.Button ("Load data")) {
			LoadGameData ();
		}
	}// end of OnGUI

	private void LoadGameData(){
		string filePath = Application.dataPath + gameDataProjectFilePath;

		if (File.Exists (filePath)) {
			string dataAsJson = File.ReadAllText (filePath);
			gameData = JsonUtility.FromJson<GameData> (dataAsJson);

		}// end of if
		else {
			// the file does not exists
			gameData = new GameData();
		}
	}// end of LoadGameData

	private void SaveGameData(){
		string dataAsJson = JsonUtility.ToJson (gameData);

		string filePath = Application.dataPath + gameDataProjectFilePath;

		File.WriteAllText (filePath, dataAsJson);
	}// end of SaveGameData

}// end of GameDataEditor
