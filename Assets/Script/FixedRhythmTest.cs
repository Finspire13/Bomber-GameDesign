using UnityEngine;
using System.Collections;

public class FixedRhythmTest : MonoBehaviour,RhythmObservable {

	// Use this for initialization
	void Start () {

		RhythmRecorder.instance.addObservedSubjects (this);

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void actionOnBeat(){
		Debug.Log ("Fixed:" + Time.time);

	}
}
