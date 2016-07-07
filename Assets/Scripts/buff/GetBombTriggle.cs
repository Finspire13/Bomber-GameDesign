using UnityEngine;
using System.Collections;

public class GetBombTriggle : BaseBuff
{

	// Use this for initialization
	void Start ()
	{
		RhythmRecorder.instance.addObservedSubject (this,0.1f);
		GameDataProcessor.instance.addToBenefitMap (this);
		transform.SetParent (MapDataHelper.instance.getMapModel().transform);
		this.gameName = "Tool-Pusher";
		this.gameValue = 100f;
	}

	// Update is called once per frame
	void Update ()
	{
		if (!this.position.Equals(null)) {
			ArrayList objs = GameDataProcessor.instance.getObjectAtPostion (this.position);
			for (int i = 0; i < objs.Count; ++i) {
				if (objs[i] is SetBomb) {

					if(GameManager.instance.isBuffValid(objs[i]) && objs[i] is PlayerConrol){
						//your effect;
						BombTriggleTool triggle = new BombTriggleTool ((SetBomb)objs[i]);
						((SetBomb)objs [i]).obtainTools (triggle);
						((SetBomb)objs [i]).BombLifeTime = 10;
					}
					Debug.Log ("player get triggle");

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

