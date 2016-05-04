using UnityEngine;
using System.Collections;

public class RhythmTest : MonoBehaviour,RhythmFlagOwner {

	private bool _rhythmFlag;

	public bool rhythmFlag
	{ 
		get{return _rhythmFlag;}
		set{_rhythmFlag=value;}
	}

	// Use this for initialization
	void Start () {
		rhythmFlag = false;
		RhythmRecorder.instance.addRhythmFlagOwner (this);
	}
	
	// Update is called once per frame
	void Update () {

		if (rhythmFlag) {   
			Debug.Log (Time.time);
			rhythmFlag = false;
		}
	
	}
}
