using UnityEngine;
using System.Collections;

public class BeatsIcon : MonoBehaviour,RhythmObservable {


	// Use this for initialization
	void Start () {
		RhythmRecorder.instance.addObservedSubject (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void actionOnBeat(){
		StartCoroutine(BeatIt());

	}

	IEnumerator BeatIt()
	{
		for (int i = 0; i < 10; i++) {

			if (i <4) {
				this.GetComponent<RectTransform>().localScale += 0.1F * Vector3.one;
			}
			else if (i >5) {
				this.GetComponent<RectTransform>().localScale -= 0.1F * Vector3.one;
			}
			yield return null;
		}
	}
}
