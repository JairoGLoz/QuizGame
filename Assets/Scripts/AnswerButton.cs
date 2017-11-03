using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {

	public Text answerText;
	private AnswerData answerData;
	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = FindObjectOfType<GameController> ();
	}// end of Start()

	public void Setup(AnswerData data){
		answerData = data;
		answerText.text = answerData.answerText;
	}// end of Setup

	public void HandleClick(){
		gameController.AnswerButtonClicked (answerData.isCorrect);
	}// end of HandleClick

}// end of AnswerButton
