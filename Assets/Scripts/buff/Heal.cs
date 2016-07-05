﻿using UnityEngine;
using System.Collections;

public class Heal : MonoBehaviour,Buff,Locatable,RhythmObservable,ScoreCount {

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
	public int buffValue = 15;
	public int Value {
		get{return buffValue; }
		set{buffValue = value; }
	}
	private string gameName = "Buff-Heal";
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

	private int effectValue = 10;
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
				if (objs[i] is CanBuffed) {
					if (GameManager.instance.isBuffValid (objs [i])) {
						((Distroyable)objs [i]).Blood += effectValue;
					}
					Debug.Log ("Heal!");
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
			RhythmRecorder.instance.removeObserver (this);
			GameDataProcessor.instance.removeFromBenefitMap (this);
			Destroy (this.gameObject, 0);
		}	
	}
}
