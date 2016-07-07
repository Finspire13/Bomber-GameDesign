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

	protected BasicRhythmProducer producer;
//	protected string currentRhythm;
//
//	protected float startTime;
//	protected ArrayList beats;
//	protected bool isPlaying;
//	protected float onBeatThreshold=0.1f;
//
//	protected ArrayList currentBeatIndice;
//	protected ArrayList observedSubjects;
//	protected ArrayList timeOffsets;
//	protected int standardBeatIndex;
//
//	protected ArrayList rhythmFlagOwners;
//	protected ArrayList isFlagsChangable;

	void Awake()
	{
//		if (instance == null)
//			instance = this;
//		else if (instance != this)
//			Destroy (gameObject);    
//
//		DontDestroyOnLoad (gameObject);
//	
//
//		currentRhythm = RhythmList.Default;
//
//		startTime = 0;
//		beats = new ArrayList ();
//		isPlaying = false;
//		currentBeatIndice=new ArrayList ();
//		observedSubjects = new ArrayList ();
//		timeOffsets = new ArrayList ();
//		standardBeatIndex = 0;
//		rhythmFlagOwners = new ArrayList ();
//		isFlagsChangable = new ArrayList ();

		//************
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);    

		DontDestroyOnLoad (gameObject);
		producer = new BasicRhythmProducer ();
//		producer = new PersonRhythmProducer();
	}

	void Start(){

	}

	public void setProducer(BasicRhythmProducer producer){
		if (producer.GetType () != this.producer.GetType ()) {
			this.producer = producer;
		}
	}

	public void removeAllObservedSubjects(){
//		producer.observedSubjects.Clear ();
//		producer.currentBeatIndice.Clear ();
//		producer.timeOffsets.Clear ();
		producer.removeAllObservedSubjects();
	}

	public void addObservedSubject(RhythmObservable newSubject, float timeOffset=0.2f){
		producer.addObservedSubject (newSubject,timeOffset);
//		newSubject.actionOnBeat ();
	}

	public void removeAllRhythmFlagOwners(){
//		producer.rhythmFlagOwners.Clear ();
//		producer.isFlagsChangable.Clear ();
		producer.removeAllRhythmFlagOwners();
	}

	public void addRhythmFlagOwner(RhythmFlagOwner newOwner){
//		producer.rhythmFlagOwners.Add (newOwner);
//		producer.isFlagsChangable.Add (true);
		producer.addRhythmFlagOwner(newOwner);
	}

	public void removeObserver(RhythmObservable subject){
//		int tempIndex = producer.observedSubjects.IndexOf (subject);
//		if (tempIndex == -1)
//			return;
//		producer.currentBeatIndice.RemoveAt (tempIndex);
//		producer.timeOffsets.RemoveAt (tempIndex);
//		producer.observedSubjects.Remove (subject);
		producer.removeObserver(subject);
	}

	public void removeFlagOwner(RhythmFlagOwner owner){
//		int indx = producer.rhythmFlagOwners.IndexOf (owner);
//		if (indx == -1)
//			return;
//		producer.isFlagsChangable.RemoveAt (indx);
//		producer.rhythmFlagOwners.Remove (owner);

		producer.removeFlagOwner (owner);
	}

	/*private void notifyAllObservedSubjects(){
		int count = observedSubjects.Count;
		for (int i = 0; i < count; i++) {
			RhythmObservable subject = (RhythmObservable)observedSubjects [i];

			subject.actionOnBeat ();
		}
	}*/

	public bool setRhythm(string newRhythm){
		
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
		return producer.setRhythm(newRhythm);
	}

	public bool startRhythm()
	{
//		if (isPlaying)
//			return false;
//
//		startTime = Time.time;
//		isPlaying = true;
//		standardBeatIndex = 0;
//
//		return true;
		return producer.startRhythm();
	}

	public void resetRhythm(){
			
//		startTime = 0;
//		isPlaying = false;
//		standardBeatIndex = 0;
		producer.resetRhythm();
	}
		
	void Update(){

		producer.updateFlagInOwners ();
		producer.notifyObserver ();
	}

//	protected bool isOnBeat()   
//	{
//		if (isFinished()||!isPlaying)
//			return false;
//		float currentStandardBeat = (float)beats [standardBeatIndex];
//		return System.Math.Abs (currentStandardBeat - (Time.time - startTime)) < onBeatThreshold;//0.2
//	}
//
//	protected void updateFlagInOwners(){
//		if (isOnBeat ()) {
//			for (int i = 0; i < rhythmFlagOwners.Count; i++) {
//				if ((bool)isFlagsChangable [i]) {
//					RhythmFlagOwner owner = (RhythmFlagOwner)rhythmFlagOwners [i];
//					if (owner is MoveAble) {
//						owner.rhythmFlag = ((MoveAble)owner).Speed;
//					} else {
//						owner.rhythmFlag = 1;
//					}
//					isFlagsChangable [i] = false;
//				}
//			}
//		} 
//		else {
//			for (int i = 0; i < rhythmFlagOwners.Count; i++) {
//				RhythmFlagOwner owner = (RhythmFlagOwner)rhythmFlagOwners [i];
//				owner.rhythmFlag = 0;
//			}
//		}
//	}
//
//	protected void notifyObserver(){
//		if (isPlaying&&!isFinished()) {
//			//			Debug.Log ("random:" + GameDataProcessor.instance.getRandom (6));
//			for (int i = 0; i < observedSubjects.Count; i++) {
//				int tempBeatIndex = (int)currentBeatIndice [i];
//				if (tempBeatIndex < beats.Count) {
//					float currentBeat = (float)beats [tempBeatIndex];
//					if ((Time.time - startTime) - currentBeat > (float)timeOffsets [i]) {
//						((RhythmObservable)observedSubjects [i]).actionOnBeat ();
//						currentBeatIndice [i] = tempBeatIndex + 1;
//					}
//				}
//			}
//
//			float currentStandardBeat = (float)beats [standardBeatIndex];
//			if ((Time.time - startTime) - currentStandardBeat > onBeatThreshold) {
//
//				//				Debug.Log("OnBeats");
//				standardBeatIndex++;
//				for (int i = 0; i < isFlagsChangable.Count; i++) {
//					isFlagsChangable [i] = true;
//				}
//			}
//		}
//	}
//
//	public bool isFinished(){
//		return standardBeatIndex >= beats.Count;
//	}

//	public ArrayList getPlayersPosition(){
//		ArrayList result = new ArrayList ();
//		for (int i = 0; i < rhythmFlagOwners.Count; ++i) {
//			if (rhythmFlagOwners [i] is Controlable && rhythmFlagOwners [i] is Locatable) {
//				result.Add (rhythmFlagOwners [i]);
//			}
//		}
//
//		return result;
//	}

//	public void testPrint()
//	{
//		Debug.Log (beats);
//	}

	public void resetGame(){
		producer.resetGame ();
	}
}