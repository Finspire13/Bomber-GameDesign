using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {

	public GameObject rhythmRecorder;
	public GameObject mapDataHelper;

	void Awake(){
		if (RhythmRecorder.instance == null) {
			Instantiate (rhythmRecorder);
		}
		if (MapDataHelper.instance == null) {
			Instantiate (mapDataHelper);
		}
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("Start:");
		Debug.Log (Time.time);
		if (!RhythmRecorder.instance.setRhythm (RhythmList.Test)) {
			Debug.Log ("error");
		}
		if (!RhythmRecorder.instance.startRhythm ()) {
			Debug.Log ("error");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
