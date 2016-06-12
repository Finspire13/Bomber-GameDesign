using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum EnemyState{EMEMY_IDLE,EMEMY_WALK,EMEMY_SETBOMB};

public class EnemyBomber : MonoBehaviour,Distroyable,SetBomb,Locatable
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

		this.bombLifeTime = 3;
		this.bombPower = 2;
		this.bombFireTime = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		control ();
//		if (mapInitClock && GameDataProcessor.isInitialized()) {
//			decisionMap = new int[GameDataProcessor.mapSizeX, GameDataProcessor.mapSizeY];
//			mapInitClock = false;
//		}
	}

//	public void control(){
//
//	}

	public bool rhythmFlag{ 
		get{ return rhmFlag;} 
		set{ rhmFlag = value;}
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

	private int bombPower = 4;
	public int BombPower {
		get{return bombPower; }
		set{bombPower = value; }
	}
	private int bombLifeTime = 5;
	public int BombLifeTime {
		get{return bombLifeTime; }
		set{bombLifeTime = value; }
	}
	private int bombFireTime = 1;
	public int BombFireTime {
		get{return bombFireTime; }
		set{bombFireTime = value; }
	}

	public void notifyExplosion (){
		
	}

	private GameObject bombType;
	public void setBomb(GameObject bombType){
		this.bombType = bombType;
	}

	public void installBomb(){
		if(currNum < maxNum){
			currNum++;
			if(bombType == null){
				Debug.Log("not bomb");
			}else{
				GameObject go = (GameObject)Instantiate(this.bombType,this.gameObject.transform.position,this.gameObject.transform.rotation);
				NormalBomb script = (NormalBomb)go.GetComponent("Bomb");
				if(script == null){
					Debug.Log("not script");
				}else{
					Debug.Log("find script interface Bomb");
					//script.LifeTime = bomblifeTime;
					//script.isActive = true;
					//					script.LifeTime = 3;
					script.setProperties(this,bombPower,bombLifeTime,bombFireTime);

					GameDataProcessor.instance.addToDangerMap (script);
				}
			}
		}
	}

	// *************** AI control ***************
//	private int[,] decisionMap = null;
//	private bool mapInitClock = true;
	Queue currPath;
	EnemyState lastState;

	private int getRandom(int count)
	{
		return new System.Random().Next(count);

	}

	public void actionOnBeat (){
		EnemyState result  = enemyThink();
		switch (result) {
		case EnemyState.EMEMY_IDLE:
			idleAction ();
			break;
		case EnemyState.EMEMY_WALK:
			walkAction ();
			break;
		case EnemyState.EMEMY_SETBOMB:
			setBombAction ();
			break;
		}
		lastState = result;
	}

	private EnemyState enemyThink(){
		ArrayList decisionMap = computDecisionMap ();

		int indx = getRandom (3);
		if (currPath != null && currPath.Count < 0) {
			MyPair pair = decisionMap [indx] as MyPair;
			Position dest = new Position (pair.px, pair.py);
			this.currPath = findPathTo(dest);
			return EnemyState.EMEMY_WALK;
		}

		indx = getRandom (15);
		if (indx < 4 && decisionMap.Count > indx) {
			MyPair pair = decisionMap [indx] as MyPair;
			Position dest = new Position (pair.px, pair.py);
			this.currPath = findPathTo(dest);

			Debug.Log ("dest:"+dest.x + "," + dest.y);

			return EnemyState.EMEMY_WALK;
		} else if (indx < 6) {
			return EnemyState.EMEMY_IDLE;
		} else if(indx < 8){
			return EnemyState.EMEMY_SETBOMB;
		}

		return EnemyState.EMEMY_WALK;
	}
	private void idleAction(){
//		Debug.Log("Enemy idle");
	}
	private void walkAction(){
//		Debug.Log("Enemy walk");
	}
	private void setBombAction(){
//		Debug.Log("Enemy setBomb");
//		installBomb ();
	}
	//寻路算法
	private Queue findPathTo(Position dest){
		Queue path = new Queue ();
		return path;
	}

	private ArrayList computDecisionMap(){
		ArrayList topValuePlace = new ArrayList();
		int[,] dangerMap = GameDataProcessor.instance.dangerMap;
		int[,] benefitMap = GameDataProcessor.instance.benefitMap;

		for (int i = 0; i < dangerMap.GetLength (0); ++i) {
			for (int j = 0; j < dangerMap.GetLength (1); ++j) {
				int value = 0;
				if (dangerMap [i, j] == -1) {
					value = 15 + benefitMap[i,j]*2;
				} else {
					value = dangerMap [i, j] + benefitMap[i,j]*2;
				}
				topValuePlace.Add (new MyPair (i, j, value));
			}
		}
		topValuePlace.Sort ();

		return topValuePlace;
	}

	public class MyPair:IComparable{
		public int value;
		public int px;
		public int py;
		public MyPair(int x,int y,int value){
			this.px = x;
			this.py = y;
			this.value = value;
		}

		public int CompareTo(object obj)
		{
			if (obj is MyPair) {
				MyPair other = obj as MyPair;
				return this.value > other.value ? -1 : 1;
			} else {
				return 0;
			}		
		}
	}

}
	
