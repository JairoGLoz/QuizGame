using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DataController : MonoBehaviour {

    private RoundData[] allRoundData;

	private PlayerProgress playerProgress;

	private string gameDataFileNName = "data.json";

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
		LoadGameData ();
		LoadPlayerProgress ();

        SceneManager.LoadScene("MenuScreen");
	}// end of Start

    public RoundData GetCurrentRoundData()
    {
        return allRoundData[0];
    }

	public void SubmitNewPlayerScore( int newScore) {
		if (newScore > playerProgress.highestScore) {
			playerProgress.highestScore = newScore;
			SavePlayerProgress ();
		}// end of if
	}// end of SubmitNewPlayerScore

	public int GetHighestPlayerScore(){
		return playerProgress.highestScore;
	}// end of GetHighestPlayerScore
	
	private void LoadPlayerProgress(){
		playerProgress = new PlayerProgress ();

		if (PlayerPrefs.HasKey ("highestScore")) {
			playerProgress.highestScore = PlayerPrefs.GetInt ("highestScore");
		}// end of if

	}// end of LoadPlayerProgress

	private void SavePlayerProgress(){
		PlayerPrefs.SetInt ("highestScore", playerProgress.highestScore);
	}// end of SavePlayerProgress

	private void LoadGameData(){

		// path to look for our data
		string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileNName);

		if (File.Exists (filePath)) {
			// attempt to load
			string dataAsJson = File.ReadAllText (filePath);
			GameData loadedData = JsonUtility.FromJson<GameData> (dataAsJson);

			allRoundData = loadedData.allRoundData;
		} else {
			Debug.LogError ("Cannot load game data!");
		}// end of if/else
	}// end of LoadGameData

}// end of DataController
