using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RhythmReminder : MonoBehaviour,RhythmObservableInAdvance  {

	public GameObject leftSlider;
	public GameObject rightSlider;
	// Use this for initialization
	void Start () {
		RhythmRecorder.instance.addObservedSubjectInAdvance (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void actionOnBeatInAdvance(){
		//StartCoroutine(BeatIt());
		GameObject leftSliderInstance = Instantiate (leftSlider) as GameObject;
		leftSliderInstance.transform.SetParent(gameObject.transform);
		leftSliderInstance.GetComponent<RectTransform>().localPosition = new Vector3 (-117.5f, 125f, 0f);
		GameObject rightSliderInstance = Instantiate (rightSlider) as GameObject;
		rightSliderInstance.transform.SetParent(gameObject.transform);
		rightSliderInstance.GetComponent<RectTransform>().localPosition = new Vector3 (117.5f, 125f, 0f);
	}
		
}
