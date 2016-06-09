﻿using UnityEngine;
using System.Collections;

public class Heal : MonoBehaviour,Buff,Locatable,RhythmObservable {

	private Position position;
	public Position pos{ 
		get{return position; }
		set{position = value; }
	}
	private int lifeTime = 5;
	public int LifeTime{
		get{return lifeTime; }
		set{lifeTime = value; }
	}
	private int buffValue = 20;
	public int Value {
		get{return buffValue; }
		set{buffValue = value; }
	}
	public void effect(){

	}

	public void actionOnBeat (){
		--lifeTime;
		if (!this.position.Equals(null)) {
			ArrayList objs = GameDataProcessor.instance.getObjectAtPostion (this.position);
			for (int i = 0; i < objs.Count; ++i) {
				if (objs[i] is PlayerConrol) {
					((PlayerConrol)objs[i]).Blood +=buffValue;
					Debug.Log ("Heal!");
					lifeTime = 0;
				}
			}

		} else {
			Debug.Log ("buff positon is null!!!");
		}
	}

	// Use this for initialization
	void Start ()
	{
		RhythmRecorder.instance.addObservedSubject (this);
		GameDataProcessor.instance.addToBenefitMap (this);
		this.lifeTime = 20;
		this.buffValue = 20;
	}

	// Update is called once per frame
	void Update ()
	{
		if (lifeTime <= 0) {
			RhythmRecorder.instance.removeObserver (this);
			GameDataProcessor.instance.removeFromBenefitMap (this);
			Destroy (this.gameObject, 0);
		}	
	}
}
