using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayingCanvasAction : MonoBehaviour,RhythmObservable  {

	public GameObject leftSlider;
	public GameObject rightSlider;
	public GameObject SliderPosition;
	public Button levelEndButton;
	public Text bloodText;
	public Text bombNumText;
	public Text bombPowerText;
	public Text bombLifeText;
	public GameObject dialog;
	public Text dialogTitle;
	public string gameLoseText = "You Lose";
	public string gameWinText = "You Win";
	// Use this for initialization
	void Start () {
		RhythmRecorder.instance.addObservedSubject (this,-2);
		levelEndButton.onClick.AddListener (() => clickLevelEndButton ());
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.playerList.Count != 0) {
			bloodText.text = "HP: " + GameManager.instance.PlayerBlood;
			PlayerConrol player = (PlayerConrol)GameManager.instance.playerList [0];
			bombNumText.text = "Max Bomb: " + player.MaxNum;
			bombPowerText.text = "Power: " + player.BombPower;
			bombLifeText.text = "Explosion Time: " + player.BombLifeTime;
		}
	}

	public void actionOnBeat(){
		//StartCoroutine(BeatIt());
		GameObject leftSliderInstance = Instantiate (leftSlider) as GameObject;
		//leftSliderInstance.transform.SetParent(gameObject.transform);
		leftSliderInstance.transform.SetParent(SliderPosition.transform);
		leftSliderInstance.GetComponent<RectTransform>().localPosition = new Vector3 (-304f, 0f, 0f);
		leftSliderInstance.GetComponent<RectTransform> ().SetAsFirstSibling ();

		GameObject rightSliderInstance = Instantiate (rightSlider) as GameObject;
		//rightSliderInstance.transform.SetParent(gameObject.transform);
		rightSliderInstance.transform.SetParent(SliderPosition.transform);
		rightSliderInstance.GetComponent<RectTransform>().localPosition = new Vector3 (304f, 0f, 0f);
		rightSliderInstance.GetComponent<RectTransform> ().SetAsFirstSibling ();
	}

	public void clickLevelEndButton(){
		showDialog ();
	}

	public void endLevel(){
		GameManager.instance.changeGameState (GameState.levelEnd);
	}

	public void showDialog(){
		switch (GameManager.instance.levelState) {
		case LevelState.lose:
			dialogTitle.text = gameLoseText;
			break;
		case LevelState.win:
			dialogTitle.text = gameWinText;
			break;
		default:
			break;
		}
		ListViewController controller = dialog.GetComponentInChildren<ListViewController> ();
		Dictionary<string,float> scoreMap = GameManager.instance.computeScore ();
		int totalScore = 0;
		string[] scores = new string[scoreMap.Count + 1];
		int i = 0;
		foreach(KeyValuePair<string, float> kv in scoreMap){
			totalScore = totalScore + (int)kv.Value;
			scores[i] = kv.Key + ": " + (int)kv.Value;
			i++;
		}
		scores [i] = "Total Score: " + totalScore;
		controller.addListContents (scores);
		GameManager.instance.totalGameScore += totalScore;
		dialog.SetActive (true);
	}

	public void hideDialog(){
		dialog.SetActive (false);
	}

	void OnDestroy() {
		RhythmRecorder.instance.removeObserver (this);
	}
		
}
