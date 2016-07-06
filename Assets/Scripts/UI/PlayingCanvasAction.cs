﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayingCanvasAction : MonoBehaviour,RhythmObservable  {

	public GameObject leftSlider;
	public GameObject rightSlider;
	public GameObject SliderPosition;
	public Button levelEndButton;
	public Text bloodText;
	// Use this for initialization
	void Start () {
		RhythmRecorder.instance.addObservedSubject (this,-2);
		levelEndButton.onClick.AddListener (() => clickLevelEndButton ());
	}
	
	// Update is called once per frame
	void Update () {
		bloodText.text = "HP: " + GameManager.instance.PlayerBlood;
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

	void clickLevelEndButton(){
		GameManager.instance.changeGameState (GameState.levelEnd);
	}

	void OnDestroy() {
		RhythmRecorder.instance.removeObserver (this);
	}
		
}
