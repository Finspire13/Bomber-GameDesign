using UnityEngine;
using System.Collections;

public class BaseBuff : MonoBehaviour,Buff,Locatable,RhythmObservable,ScoreCount
{
	protected Position position;
	public Position pos{ 
		get{return position; }
		set{position = value; }
	}
	protected int lifeTime = 25;
	public int LifeTime{
		get{return lifeTime; }
		set{lifeTime = value; }
	}
	public int buffValue = 10;
	public int Value {
		get{return buffValue; }
		set{buffValue = value; }
	}
	protected string gameName = "Buff-Base";
	protected float gameValue = 10f;

	public string getName(){
		return this.gameName;
	}
	public float getValue(){
		return this.gameValue;
	}
	public void addToScore(){
		GameManager.instance.addToPlayerScoreList (this);
	}

	public void actionOnBeat (){
		--lifeTime;
	}

	// Use this for initialization
	void Start ()
	{
		RhythmRecorder.instance.addObservedSubject (this,0.1f);
		GameDataProcessor.instance.addToBenefitMap (this);
		//		this.lifeTime = 200;
		//		this.buffValue = 5;
	}

	// Update is called once per frame
	void Update ()
	{
		if (!this.position.Equals(null)) {
			ArrayList objs = GameDataProcessor.instance.getObjectAtPostion (this.position);
			for (int i = 0; i < objs.Count; ++i) {
				if (objs[i] is SetBomb) {
					
					if(GameManager.instance.isBuffValid(objs[i])){
						//your effect;
					}
					lifeTime = 0;
					if (objs [i] is PlayerConrol) {
						this.addToScore ();
					}
				}
			}

		} else {
			Debug.Log ("buff positon is null!!!");
		}
		if (lifeTime <= 0) {
			this.distroy ();
		}
	}

	void distroy(){
		RhythmRecorder.instance.removeObserver (this);
		GameDataProcessor.instance.removeFromBenefitMap (this);
		Destroy (this.gameObject, 0);
	}

	void OnDestroy(){
		this.distroy ();
	}
}

