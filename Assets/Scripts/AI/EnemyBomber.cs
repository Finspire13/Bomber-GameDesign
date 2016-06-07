using UnityEngine;
using System.Collections;

public enum EnemyState{EMEMY_IDLE,EMEMY_WALK,EMEMY_SETBOMB};

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

		this.bombLifeTime = 3;
		this.bombPower = 2;
		this.bombFireTime = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		control ();
	}

	public void control(){

	}

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
	private int getRandom(int count)
	{
		return new System.Random().Next(count);

	}
	private EnemyState enemyThink(){
		return EnemyState.EMEMY_IDLE;
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
	}

	private void idleAction(){

	}
	private void walkAction(){

	}
	private void setBombAction(){
		installBomb ();
	}

}

