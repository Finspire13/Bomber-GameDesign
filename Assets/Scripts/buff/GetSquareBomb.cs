using UnityEngine;
using System.Collections;

public class GetSquareBomb : BaseBuff
{
	private GameObject squareBomb;

	// Use this for initialization
	void Start ()
	{
		RhythmRecorder.instance.addObservedSubject (this,0.1f);
		GameDataProcessor.instance.addToBenefitMap (this);
		squareBomb = Resources.Load("SquareBomb") as GameObject;
		this.gameName = "Bomb-PosionBomb";
		this.gameValue = 100f;
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
						((SetBomb)objs [i]).setBomb (squareBomb);
					}
					Debug.Log ("player get SquareBomb!");
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

