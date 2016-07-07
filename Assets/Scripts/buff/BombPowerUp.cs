using UnityEngine;
using System.Collections;

public class BombPowerUp : MonoBehaviour,Buff,Locatable,RhythmObservable,ScoreCount
{
	private Position position;
	public Position pos{ 
		get{return position; }
		set{position = value; }
	}
	private int lifeTime = 25;
	public int LifeTime{
		get{return lifeTime; }
		set{lifeTime = value; }
	}
	public int buffValue = 25;
	public int Value {
		get{return buffValue; }
		set{buffValue = value; }
	}
	private string gameName = "Buff-BombPowerUp";
	private float gameValue = 10f;
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
					if (GameManager.instance.isBuffValid (objs [i])) {
						((SetBomb)objs [i]).BombPower += 1;
					}
					Debug.Log ("player power up !");

					lifeTime = 0;
					if (objs [i] is PlayerConrol) {
						AudioPlayer.instance.playSFX (SFX.Buff);
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

