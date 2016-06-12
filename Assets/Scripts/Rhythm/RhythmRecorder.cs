﻿using System.IO;
using UnityEngine;
using System.Collections;

public interface RhythmObservable{
	void actionOnBeat ();
}

public interface RhythmFlagOwner{
	bool rhythmFlag{ get; set;}
}

public class RhythmRecorder: MonoBehaviour{

	public static RhythmRecorder instance = null;

	private string currentRhythm;

	private float startTime;
	private ArrayList beats;
	private bool isPlaying;
	private float onBeatThreshold=0.1f;

	private ArrayList currentBeatIndice;
	private ArrayList observedSubjects;
	private ArrayList timeOffsets;
	private int standardBeatIndex;

	private ArrayList rhythmFlagOwners;
	private ArrayList isFlagsChangable;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);    

		DontDestroyOnLoad (gameObject);


		currentRhythm = RhythmList.Default;

		startTime = 0;
		beats = new ArrayList ();
		isPlaying = false;
		currentBeatIndice=new ArrayList ();
		observedSubjects = new ArrayList ();
		timeOffsets = new ArrayList ();
		standardBeatIndex = 0;
		rhythmFlagOwners = new ArrayList ();
		isFlagsChangable = new ArrayList ();
	}

	void Start(){

	}

	public void removeAllObservedSubjects(){
		observedSubjects.Clear ();
		currentBeatIndice.Clear ();
		timeOffsets.Clear ();
	}

	public void addObservedSubject(RhythmObservable newSubject, float timeOffset=0.2f){
		observedSubjects.Add (newSubject);
		timeOffsets.Add (timeOffset);
		currentBeatIndice.Add (standardBeatIndex);
//		newSubject.actionOnBeat ();
	}

	public void removeAllRhythmFlagOwners(){
		rhythmFlagOwners.Clear ();
		isFlagsChangable.Clear ();
	}

	public void addRhythmFlagOwner(RhythmFlagOwner newOwner){
		rhythmFlagOwners.Add (newOwner);
		isFlagsChangable.Add (true);
	}

	public void removeObserver(RhythmObservable subject){
		int tempIndex = observedSubjects.IndexOf (subject);
		if (tempIndex == -1)
			return;
		currentBeatIndice.RemoveAt (tempIndex);
		timeOffsets.RemoveAt (tempIndex);
		observedSubjects.Remove (subject);
	}

	public void removeFlagOwner(RhythmFlagOwner owner){
		int indx = rhythmFlagOwners.IndexOf (owner);
		if (indx == -1)
			return;
		isFlagsChangable.RemoveAt (indx);
		rhythmFlagOwners.Remove (owner);
	}

	/*private void notifyAllObservedSubjects(){
		int count = observedSubjects.Count;
		for (int i = 0; i < count; i++) {
			RhythmObservable subject = (RhythmObservable)observedSubjects [i];

			subject.actionOnBeat ();
		}
	}*/
		

	private void updateFlagInOwners(){
		if (isOnBeat ()) {
			for (int i = 0; i < rhythmFlagOwners.Count; i++) {
				if ((bool)isFlagsChangable [i]) {
					RhythmFlagOwner owner = (RhythmFlagOwner)rhythmFlagOwners [i];
					owner.rhythmFlag = true;
					isFlagsChangable [i] = false;
				}
			}
		} 
		else {
			for (int i = 0; i < rhythmFlagOwners.Count; i++) {
				RhythmFlagOwner owner = (RhythmFlagOwner)rhythmFlagOwners [i];
				owner.rhythmFlag = false;
			}
		}
	}

	public bool setRhythm(string newRhythm){
		
		currentRhythm = newRhythm;
		resetRhythm ();

		if (!File.Exists (currentRhythm)) {
			return false;
		}

		string text = File.ReadAllText(currentRhythm);
		string[] sArray=text.Split(' ') ;

		beats = new ArrayList ();
		foreach (string i in sArray) {
			beats.Add (float.Parse (i));
		}
		return true;
	}

	public bool startRhythm()
	{
		if (isPlaying)
			return false;

		startTime = Time.time;
		isPlaying = true;
		standardBeatIndex = 0;

		return true;
	}


	/*bool stopRhythm(){
		if (!isPlaying)
			return false;

		isPlaying = false;
		return true;
	}*/

	public void resetRhythm(){
			
		startTime = 0;
		isPlaying = false;
		standardBeatIndex = 0;
	}

	private bool isOnBeat()   
	{
		if (isFinished()||!isPlaying)
			return false;
		float currentStandardBeat = (float)beats [standardBeatIndex];
		return System.Math.Abs (currentStandardBeat - (Time.time - startTime)) < onBeatThreshold;//0.2
	}
		
	void Update(){

		updateFlagInOwners ();

		if (isPlaying&&!isFinished()) {
//			Debug.Log ("random:" + GameDataProcessor.instance.getRandom (6));
			for (int i = 0; i < observedSubjects.Count; i++) {
				int tempBeatIndex = (int)currentBeatIndice [i];
				if (tempBeatIndex < beats.Count) {
					float currentBeat = (float)beats [tempBeatIndex];
					if ((Time.time - startTime) - currentBeat > (float)timeOffsets [i]) {
						((RhythmObservable)observedSubjects [i]).actionOnBeat ();
						currentBeatIndice [i] = tempBeatIndex + 1;
					}
				}
			}


			float currentStandardBeat = (float)beats [standardBeatIndex];
			if ((Time.time - startTime) - currentStandardBeat > onBeatThreshold) {

//				Debug.Log("OnBeats");
				standardBeatIndex++;
				for (int i = 0; i < isFlagsChangable.Count; i++) {
					isFlagsChangable [i] = true;
				}
			}
		}
	}

	public bool isFinished(){
		return standardBeatIndex >= beats.Count;
	}

	public ArrayList getPlayersPosition(){
		ArrayList result = new ArrayList ();
		for (int i = 0; i < rhythmFlagOwners.Count; ++i) {
			if (rhythmFlagOwners [i] is Controlable && rhythmFlagOwners [i] is Locatable) {
				result.Add (rhythmFlagOwners [i]);
			}
		}

		return result;
	}

	/*public void testPrint()
	{
		Debug.Log (beats);
	}*/

}