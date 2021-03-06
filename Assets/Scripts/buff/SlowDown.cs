﻿using UnityEngine;
using System.Collections;

public class SlowDown : MonoBehaviour,Buff,Locatable,RhythmObservable,ScoreCount
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
	public int buffValue = 0;
	public int Value {
		get{return buffValue; }
		set{buffValue = value; }
	}
	private string gameName = "Debuff-SlowDown";
	private float gameValue = -5f;
	public string getName(){
		return this.gameName;
	}
	public float getValue(){
		return this.gameValue;
	}
	public void addToScore(){
		GameManager.instance.addToPlayerScoreList (this);
	}

	private int effectValue = 1;
	public void actionOnBeat (){
		--lifeTime;
	}

	// Use this for initialization
	void Start ()
	{
		RhythmRecorder.instance.addObservedSubject (this,0.1f);
		GameDataProcessor.instance.addToBenefitMap (this);
		//		this.lifeTime = 200;
		//		this.buffValue = 15;
	}

	// Update is called once per frame
	void Update ()
	{
		if (!this.position.Equals(null)) {
			ArrayList objs = GameDataProcessor.instance.getObjectAtPostion (this.position);
			for (int i = 0; i < objs.Count; ++i) {
				if (objs[i] is MoveAble) {
					if (((MoveAble)objs [i]).Speed >= 1 + effectValue && GameManager.instance.isBuffValid (objs [i])) {
						((MoveAble)objs [i]).Speed -= effectValue;
	
					}
					Debug.Log ("Slow down!");

					lifeTime = 0;
					if (objs [i] is PlayerConrol) {
						AudioPlayer.instance.playSFX (SFX.Debuff);
						this.addToScore ();
					}
				}
			}

		} else {
			Debug.Log ("buff positon is null!!!");
		}
		if (lifeTime <= 0) {
			distroy ();
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

