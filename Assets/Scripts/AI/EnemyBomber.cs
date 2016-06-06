using UnityEngine;
using System.Collections;

public class EnemyBomber : MonoBehaviour,Controlable,Distroyable,SetBomb,Locatable
{
	int speed = 1;
	public int Speed 
	{
		get{return speed; }
		set{speed = value; }
	}
	private bool rhmFlag;
	private int blood;

	private int maxNum;
	private int currNum;
	private Position position;

	public Position pos{ 
		get{ return position;}
		set{ position=value;}
	}

	// Use this for initialization
	void Start ()
	{
		this.bombType = Resources.Load("NormalBomb") as GameObject;
		RhythmRecorder.instance.addObservedSubject (this);
		this.position = new Position(Mathf.RoundToInt(transform.localPosition.z),Mathf.RoundToInt(transform.localPosition.x));
		GameDataProcessor.instance.addObject (this);

		this.maxNum = 3;
		this.currNum = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void control(){

	}

	public bool rhythmFlag{ 
		get{ return rhmFlag;} 
		set{ rhmFlag = value;}
	}

	public void actionOnBeat (){
		
	}

	public int Blood 
	{
		get{return blood; }
		set{blood = value; }
	}
	public void attackBy(Attackable source){
		
	}
	public void distroy(){
		Destroy (this.gameObject, 0);
	}

	public int MaxNum {
		get{return maxNum;}
		set{maxNum = value;}
	}

	//already used number of bomb
	public int CurrNum {
		get{return currNum;}
		set{currNum = value;}
	}

	//	int BombLifeTime {
	//		get;
	//		set;
	//	}

	public void notifyExplosion (){
		
	}

	private GameObject bombType;
	public void setBomb(GameObject bombType){
		this.bombType = bombType;
	}

	public void installBomb(){
		
	}
}

