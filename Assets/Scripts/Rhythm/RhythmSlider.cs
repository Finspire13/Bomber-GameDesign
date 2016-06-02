using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RhythmSlider : MonoBehaviour {

	public Slider slider;
	public float timeToCountDown=5;
	public float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}

	// Update is called once per frame
	void Update () {

		float currentPercentage = 1 - (Time.time - startTime) / timeToCountDown;
		if (currentPercentage < 0) {
			Destroy (gameObject);
		}
		slider.value = currentPercentage;
	}
}
