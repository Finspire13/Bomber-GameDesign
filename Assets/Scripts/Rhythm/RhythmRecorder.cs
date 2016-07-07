using System.IO;
using UnityEngine;
using System.Collections;

public interface RhythmObservable{
	void actionOnBeat ();
}

public interface RhythmFlagOwner{
	int rhythmFlag{ get; set;}
}

public class RhythmRecorder: MonoBehaviour{

	public static RhythmRecorder instance = null;

	protected string currentRhythm;

	protected float startTime;
	protected ArrayList beats;
	protected bool isPlaying;
	protected float onBeatThreshold=0.1f;

	protected ArrayList currentBeatIndice;
	protected ArrayList observedSubjects;
	protected ArrayList timeOffsets;
	protected int standardBeatIndex;

	protected ArrayList rhythmFlagOwners;
	protected ArrayList isFlagsChangable;

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

	public void setRhythmRecord(RhythmRecorder record){
		RhythmRecorder.instance = record;
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
		

	protected void updateFlagInOwners(){
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

	protected bool isOnBeat()   
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

	public void resetGame(){
	}
}