using UnityEngine;
using System.Collections;

public class NormalBombFire : MonoBehaviour,Distroyable,BombFire,Locatable
{
	private int lifeTime;
	private int damge;
	private SetBomb owner;
	public SetBomb Owner {
		get{return this.owner;}
		set{this.owner = value;}
	}
	private Position position;

	public Position pos{ 
		get{ return position; } 
		set{ position=value; }
	}

	public void setProperties(SetBomb owner,int lifeTime){
		this.owner = owner;
		this.lifeTime = lifeTime;
	}

	// Use this for initialization
	void Start ()
	{
		GameDataProcessor.instance.addObject (this);
		RhythmRecorder.instance.addObservedSubject (this);
		lifeTime = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lifeTime <= 0) {
			attack ();
			distroy();
		}
	}

	public int Damage{
		get{return this.damge;}
		set{this.damge=value;}
	}
	// set and get lifeTime
	public int Blood 
	{
		get{return this.lifeTime;}
		set{this.lifeTime = value;}
	}
	// set and get lifeTime
	public int LifeTime 
	{
		get{return this.lifeTime;}
		set{this.lifeTime = value;}
	}

	public void attackBy(Attackable source){

	}

	public void actionOnBeat (){
		--lifeTime;
//		Debug.Log ("--lifeTime");
	}

	public void distroy(){

		GameDataProcessor.instance.removeObject (this);
		RhythmRecorder.instance.removeObserver (this);
		Destroy(this.gameObject,0);
	}

	public void attack(){
		ArrayList objs = GameDataProcessor.instance.getObjectsAtTheSamePosition (this);
		if (objs != null) {
			for (int i = 0; i < objs.Count; ++i) {
				if (objs [i] is Distroyable) {
					((Distroyable)objs [i]).attackBy (this);
				}
			}
		}
	}
}
