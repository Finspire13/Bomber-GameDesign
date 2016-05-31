using System.IO;
using UnityEngine;
using System.Collections;

public interface RhythmObservable{
	void actionOnBeat ();
	void actionAfterBeat();
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
	private int currentBeatIndex;
	private ArrayList observedSubjects;
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
		currentBeatIndex = 0;
		observedSubjects = new ArrayList ();
		rhythmFlagOwners = new ArrayList ();
		isFlagsChangable = new ArrayList ();
	}

	void Start(){

	}

	public void removeAllObservedSubjects(){
		observedSubjects.Clear ();
	}

	public void addObservedSubject(RhythmObservable newSubject){
		observedSubjects.Add (newSubject);
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
		observedSubjects.Remove (subject);
	}

	public void removeFlagOwner(RhythmFlagOwner owner){
		int indx = rhythmFlagOwners.IndexOf (owner);
		isFlagsChangable.RemoveAt (indx);
		rhythmFlagOwners.Remove (owner);

	}

	private void notifyAllObservedSubjects(){
//		Debug.Log ("number of observers:" + observedSubjects.Count);
//		Debug.Log ("currentBeatIndex:" + currentBeatIndex.ToString());
		int count = observedSubjects.Count;
		for (int i = 0; i < count; i++) {
			RhythmObservable subject = (RhythmObservable)observedSubjects [i];

			subject.actionOnBeat ();
		}
	}



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
		currentBeatIndex = 0;

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
		currentBeatIndex = 0;
	}

	private bool isOnBeat()   
	{
		if (isFinished()||!isPlaying)
			return false;
		float currentBeat = (float)beats [currentBeatIndex];
		return System.Math.Abs (currentBeat - (Time.time - startTime)) < 0.2;
	}
		
	void Update(){

		updateFlagInOwners ();

		if (isPlaying&&!isFinished()) {

			float currentBeat = (float)beats [currentBeatIndex];
			if ((Time.time - startTime) - currentBeat > 0.2) {

//				Debug.Log("number of observers: "+observedSubjects.Count);

				currentBeatIndex++;
				notifyAllObservedSubjects ();

				for (int i = 0; i < isFlagsChangable.Count; i++) {
					isFlagsChangable [i] = true;
				}
			}
		}
	}
		

	public bool isFinished(){
		return currentBeatIndex >= beats.Count;
	}

	/*public void testPrint()
	{
		Debug.Log (beats);
	}*/

}