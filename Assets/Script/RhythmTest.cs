using UnityEngine;
using System.Collections;

public class RhythmTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (RhythmRecorder.instance.isOnBeat ()) {   //non-fixed beat
			Debug.Log (Time.time);
		}
	
	}
}
