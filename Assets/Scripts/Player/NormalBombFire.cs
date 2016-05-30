using UnityEngine;
using System.Collections;

public class NormalBombFire : MonoBehaviour,Distroyable,BombFire,Locatable
{
	private int lifeTime;
	private int damge;
	private GameObject onwer;
	private Position position;

	public Position pos{ 
		get{ return position; } 
		set{ position=value; }
	}

	public void setProperties(GameObject onwer,int lifeTime){
		this.onwer = onwer;
		this.lifeTime = lifeTime;
	}

	// Use this for initialization
	void Start ()
	{
		lifeTime = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	public GameObject Onwer {
		get;
		set;
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
	public void distroy(){
		--lifeTime;
		if (lifeTime <= 0) {
			Destroy(this.gameObject,2);
		}
	}
	public void attack(){
	}

	void attack(){

	}
}

