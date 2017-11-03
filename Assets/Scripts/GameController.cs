using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public Text questionDisplayText;
	public Text scoreDisplayText;
	public Text timeRemainingDisplayText;
	public SimpleObjectPool answerButtonObjectPool;
	public Transform answerButtonParent;
	public GameObject questionDisplay;
	public GameObject roundEndDisplay;

	private DataController dataController;
	private RoundData currentRoundData;
	private QuestionData[] questionPool;

	private bool isRoundActive;
	private float timeRemaining;
	private int questionIndex;
	private int playerScore;
	private List<GameObject> answerButtonGameObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
		dataController = FindObjectOfType<DataController> ();
		currentRoundData = dataController.GetCurrentRoundData ();
		questionPool = currentRoundData.questions;
		timeRemaining = currentRoundData.timeLimitInSeconds;

		UpdateTimeRemainingDisplay ();

		playerScore = 0;
		questionIndex = 0;

		ShowQuestion ();

		isRoundActive = true;

	}// end of Start method

	private void ShowQuestion(){
		RemoveAnswerButtons ();
		QuestionData questionData = questionPool [questionIndex];
		questionDisplayText.text = questionData.questionText;

		// loop answers and show buttons
		for (int i = 0; i < questionData.answers.Length; i++) {

			GameObject answerButtonGameObject = answerButtonObjectPool.GetObject ();
			answerButtonGameObject.transform.SetParent (answerButtonParent);
			answerButtonGameObjects.Add (answerButtonGameObject);

			AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton> ();
			answerButton.Setup (questionData.answers [i]);
		}// end of for 

	}// end of ShowQuestion

	private void RemoveAnswerButtons(){
		while (answerButtonGameObjects.Count > 0) {
			// the button will be returned to the pool, so it'll be deactivated and made ready to be recycled/reused
			answerButtonObjectPool.ReturnObject (answerButtonGameObjects [0]);

			// remove of the answerButtonGameObjects list
			answerButtonGameObjects.RemoveAt (0);
		}// end of while
	}// end of RemoveAnswerButtons

	public void AnswerButtonClicked(bool isCorrect){

		if (isCorrect == true) {
			playerScore += currentRoundData.pointsAddedForCorrectAnswer;
			scoreDisplayText.text = "Score: " + playerScore.ToString ();
		}

		// do we still have questions left
		if (questionPool.Length > questionIndex + 1) {
			questionIndex++;
			ShowQuestion ();
		} else {
			// end round
			EndRound();
		}
	}// end of AnswerButtonClicked

	public void EndRound(){
		isRoundActive = false;
		questionDisplay.SetActive (false);
		roundEndDisplay.SetActive (true);
	}// end of EndRound

	public void ReturnToMenu(){
		// we never get back to the persistent scene
		SceneManager.LoadScene ("MenuScreen");
	}// end of ReturnToMenu

	private void UpdateTimeRemainingDisplay(){
		timeRemainingDisplayText.text = "Time: " + Mathf.Round (timeRemaining).ToString ();
	}// end of UpdateTimeRemainingDisplay
	
	// Update is called once per frame
	void Update () {
		if (isRoundActive) {
			timeRemaining -= Time.deltaTime;
			UpdateTimeRemainingDisplay ();

			if (timeRemaining <= 0) {
				EndRound ();
			}
		}// end of if
	}// end of Update

}// end of GameController
