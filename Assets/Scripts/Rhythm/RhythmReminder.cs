using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RhythmReminder : MonoBehaviour,RhythmObservable  {

	public GameObject leftSlider;
	public GameObject rightSlider;
	// Use this for initialization
	void Start () {
		RhythmRecorder.instance.addObservedSubject (this,-2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void actionOnBeat(){
		//StartCoroutine(BeatIt());
		GameObject leftSliderInstance = Instantiate (leftSlider) as GameObject;
		leftSliderInstance.transform.SetParent(gameObject.transform);
		leftSliderInstance.GetComponent<RectTransform>().localPosition = new Vector3 (-152f, 125f, 0f);
		leftSliderInstance.GetComponent<RectTransform> ().SetAsFirstSibling ();

		GameObject rightSliderInstance = Instantiate (rightSlider) as GameObject;
		rightSliderInstance.transform.SetParent(gameObject.transform);
		rightSliderInstance.GetComponent<RectTransform>().localPosition = new Vector3 (152f, 125f, 0f);
		rightSliderInstance.GetComponent<RectTransform> ().SetAsFirstSibling ();
	}
		
}
