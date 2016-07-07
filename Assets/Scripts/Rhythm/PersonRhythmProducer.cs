using System;
using System.IO;
using UnityEngine;
using System.Collections;


public class PersonRhythmProducer:BasicRhythmProducer
{
	float currentBeat = 0f;

	public PersonRhythmProducer ()
	{
		this.init();
	}

//	public override void init(){
//		currentRhythm = RhythmList.Default;
//
//		startTime = 0;
//		beats = new ArrayList ();
//		isPlaying = false;
//		currentBeatIndice=new ArrayList (); // change to record whether the object can be notified now.
//		observedSubjects = new ArrayList ();
//		timeOffsets = new ArrayList ();
//		standardBeatIndex = 0;
//		rhythmFlagOwners = new ArrayList ();
//		isFlagsChangable = new ArrayList ();
//	}

//	public void removeAllObservedSubjects(){
//		observedSubjects.Clear ();
//		currentBeatIndice.Clear ();
//		timeOffsets.Clear ();
//	}

	public override void addObservedSubject(RhythmObservable newSubject, float timeOffset=0.2f){
		observedSubjects.Add (newSubject);
		timeOffsets.Add (timeOffset);
		currentBeatIndice.Add (true);
		//		newSubject.actionOnBeat ();
	}

//	public void removeAllRhythmFlagOwners(){
//		rhythmFlagOwners.Clear ();
//		isFlagsChangable.Clear ();
//	}
//
//	public void addRhythmFlagOwner(RhythmFlagOwner newOwner){
//		rhythmFlagOwners.Add (newOwner);
//		isFlagsChangable.Add (true);
//	}
//
//	public void removeObserver(RhythmObservable subject){
//		int tempIndex = observedSubjects.IndexOf (subject);
//		if (tempIndex == -1)
//			return;
//		currentBeatIndice.RemoveAt (tempIndex);
//		timeOffsets.RemoveAt (tempIndex);
//		observedSubjects.Remove (subject);
//	}

//	public void removeFlagOwner(RhythmFlagOwner owner){
//		int indx = rhythmFlagOwners.IndexOf (owner);
//		if (indx == -1)
//			return;
//		isFlagsChangable.RemoveAt (indx);
//		rhythmFlagOwners.Remove (owner);
//	}

	public override bool setRhythm(string newRhythm){

//		currentRhythm = newRhythm;
//		resetRhythm ();
//
//		if (!File.Exists (currentRhythm)) {
//			return false;
//		}
//
//		string text = File.ReadAllText(currentRhythm);
//		string[] sArray=text.Split(' ') ;
//
//		beats = new ArrayList ();
//		foreach (string i in sArray) {
//			beats.Add (float.Parse (i));
//		}
//		return true;
		return true;
	}

//	public bool startRhythm()
//	{
//		if (isPlaying)
//			return false;
//
//		startTime = Time.time;
//		isPlaying = true;
//		standardBeatIndex = 0;
//
//		return true;
//	}
//
//	public void resetRhythm(){
//
//		startTime = 0;
//		isPlaying = false;
//		standardBeatIndex = 0;
//	}


	protected override bool isOnBeat()   
	{
		return true;
	}

	public override void updateFlagInOwners(){
		if (Input.anyKeyDown) {
			if (Time.time - currentBeat > 0.2f) {
				currentBeat = Time.time;

				for (int i = 0; i < rhythmFlagOwners.Count; i++) {
					RhythmFlagOwner owner = (RhythmFlagOwner)rhythmFlagOwners [i];
					if (owner is MoveAble) {
						owner.rhythmFlag = ((MoveAble)owner).Speed;
					} else {
						owner.rhythmFlag = 1;
					}
				}
				for(int i = 0;i < currentBeatIndice.Count;++i){
					currentBeatIndice [i] = true;
				}
			} else {
				for (int i = 0; i < rhythmFlagOwners.Count; i++) {
					RhythmFlagOwner owner = (RhythmFlagOwner)rhythmFlagOwners [i];
					owner.rhythmFlag = 0;
				}
			}
		}
	}

	public override void notifyObserver(){
		for (int i = 0; i < observedSubjects.Count; i++) {
			if (Time.time - currentBeat > (float)timeOffsets [i]  && (bool)currentBeatIndice [i]) {
				((RhythmObservable)observedSubjects [i]).actionOnBeat ();
				currentBeatIndice [i] = false;
			}
		}
	}

	public override bool isFinished(){
		return false;
	}

}


