using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class BasicRhythmProducer
{

	public string currentRhythm;

	public float startTime;
	public ArrayList beats;
	public bool isPlaying;
	public float onBeatThreshold=0.1f;

	public ArrayList currentBeatIndice;
	public ArrayList observedSubjects;
	public ArrayList timeOffsets;
	public int standardBeatIndex;

	public ArrayList rhythmFlagOwners;
	public ArrayList isFlagsChangable;

	public BasicRhythmProducer ()
	{
		init ();
	}

	public virtual void init(){
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

	public virtual void removeAllObservedSubjects(){
		observedSubjects.Clear ();
		currentBeatIndice.Clear ();
		timeOffsets.Clear ();
	}

	public virtual void addObservedSubject(RhythmObservable newSubject, float timeOffset=0.2f){
		observedSubjects.Add (newSubject);
		timeOffsets.Add (timeOffset);
		currentBeatIndice.Add (standardBeatIndex);
		//		newSubject.actionOnBeat ();
	}

	public virtual void removeAllRhythmFlagOwners(){
		rhythmFlagOwners.Clear ();
		isFlagsChangable.Clear ();
	}

	public virtual void addRhythmFlagOwner(RhythmFlagOwner newOwner){
		rhythmFlagOwners.Add (newOwner);
		isFlagsChangable.Add (true);
	}

	public virtual void removeObserver(RhythmObservable subject){
		int tempIndex = observedSubjects.IndexOf (subject);
		if (tempIndex == -1)
			return;
		currentBeatIndice.RemoveAt (tempIndex);
		timeOffsets.RemoveAt (tempIndex);
		observedSubjects.Remove (subject);
	}

	public virtual void removeFlagOwner(RhythmFlagOwner owner){
		int indx = rhythmFlagOwners.IndexOf (owner);
		if (indx == -1)
			return;
		isFlagsChangable.RemoveAt (indx);
		rhythmFlagOwners.Remove (owner);
	}

	public virtual bool setRhythm(string newRhythm){

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

	public virtual bool startRhythm()
	{
		if (isPlaying)
			return false;

		startTime = Time.time;
		isPlaying = true;
		standardBeatIndex = 0;

		return true;
	}
		
	public virtual void resetRhythm(){

		startTime = 0;
		isPlaying = false;
		standardBeatIndex = 0;
	}

	protected virtual bool isOnBeat()   
	{
		if (isFinished()||!isPlaying)
			return false;
		float currentStandardBeat = (float)beats [standardBeatIndex];
		return System.Math.Abs (currentStandardBeat - (Time.time - startTime)) < onBeatThreshold;//0.2
	}

	public virtual void updateFlagInOwners(){
		if (isOnBeat ()) {
			for (int i = 0; i < rhythmFlagOwners.Count; i++) {
				if ((bool)isFlagsChangable [i]) {
					RhythmFlagOwner owner = (RhythmFlagOwner)rhythmFlagOwners [i];
					if (owner is MoveAble) {
						owner.rhythmFlag = ((MoveAble)owner).Speed;
					} else {
						owner.rhythmFlag = 1;
					}
					isFlagsChangable [i] = false;
				}
			}
		} 
		else {
			for (int i = 0; i < rhythmFlagOwners.Count; i++) {
				RhythmFlagOwner owner = (RhythmFlagOwner)rhythmFlagOwners [i];
				owner.rhythmFlag = 0;
			}
		}
	}

	public virtual void notifyObserver(){
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

	public virtual bool isFinished(){
		return standardBeatIndex >= beats.Count;
	}

	public virtual void resetGame(){
	}
}


