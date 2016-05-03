using System.IO;
using UnityEngine;
using System.Collections;

public interface RhythmObservable{
	void actionOnBeat ();
}

public class RhythmRecorder: MonoBehaviour{

	public static RhythmRecorder instance = null;

	private string currentRhythm;

	private float startTime;
	private ArrayList beats;
	private bool isPlaying;
	private int currentBeatIndex;
	private int currentFixedBeatIndex;
	private ArrayList observedSubjects;

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
		currentFixedBeatIndex = 0;
		observedSubjects = new ArrayList ();
	}

	void Start(){

	}

	public void removeAllObservedSubjects(){
		observedSubjects.Clear ();
	}

	public void addObservedSubjects(RhythmObservable newSubjects){
		observedSubjects.Add (newSubjects);
	}

	private void notify(){
		for (int i = 0; i < observedSubjects.Count; i++) {
			RhythmObservable subject = (RhythmObservable)observedSubjects [i];
			subject.actionOnBeat ();
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
		currentFixedBeatIndex = 0;

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
		currentFixedBeatIndex = 0;
	}

	public bool isOnBeat()   //call by non-fixed subjects, such as player movement
	{
		if (isFinished()||!isPlaying)
			return false;
		float currentBeat = (float)beats [currentBeatIndex];
		if (System.Math.Abs (currentBeat - (Time.time - startTime)) < 0.2) {
			currentBeatIndex++;
			return true;
		} else
			return false;
	}
		
	void Update(){
		if (isPlaying&&!isFinished()) {
			float currentBeat = (float)beats [currentBeatIndex];
			if ((Time.time - startTime) - currentBeat > 0.2)
				currentBeatIndex++;
		}
		if (isPlaying&&!isFinished()) {
			float currentFixedBeat = (float)beats [currentFixedBeatIndex];
			if ((Time.time - startTime) - currentFixedBeat > 0.2) {
				currentFixedBeatIndex++;
				notify ();
			}
		}
	}
		

	public bool isFinished(){
		return currentBeatIndex >= beats.Count||currentFixedBeatIndex >=beats.Count;
	}

	public void testPrint()
	{
		Debug.Log (beats);
	}

}