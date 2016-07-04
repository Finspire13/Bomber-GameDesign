using UnityEngine;
using System.Collections;

public class SquareBomb :MonoBehaviour,Bomb,Distroyable,Locatable
{
	private SetBomb owner = null;
	public GameObject fire = null;
	private int lifeTime = 2;
	private int power = 5;
	private Position position;
	public Position pos{ 
		get{ return position; } 
		set{ position=value; }
	}


	private bool isActive = true;
	public bool IsActive{
		get{ return isActive; } 
		set{ isActive=value; }
	}

	public int stepLenth = 1;
	//	public NormalBomb(GameObject owner,BombFire fire,int lifeTime){
	//		this.owner = owner;
	//		this.fire = fire;
	//		this.lifeTime = lifeTime;
	//		this.isActive = true;
	//	}
	//	public NormalBomb(GameObject owner,BombFire fire){
	//		NormalBomb (owner, fire, 5);
	//	}
	//	public NormalBomb(){
	//		NormalBomb (null, null, 0);
	//	}

	// Use this for initialization
	void Start ()
	{
		if (this.fire == null) {
			this.fire = Resources.Load ("poisonFireBase") as GameObject;
		}
		//		Debug.Log ("NormalBomb :MonoBehaviour,Bomb,Distroyable,Locatable");
		GameDataProcessor.instance.addObject (this);
		RhythmRecorder.instance.addObservedSubject (this);
		GameDataProcessor.instance.addToDangerMap (this);
	}

	// Update is called once per frame
	void Update () {
		if (lifeTime <= 0 && isActive) {
			this.createFire();
			isActive = false;
			this.distroy();
		}
	}


	public void setProperties(SetBomb owner,int power,int lifeTime,int fireTime){
		this.owner = owner;
		this.lifeTime = lifeTime;
		this.power = power;
		this.fireTime = fireTime;
		if(this.owner is Locatable){
			this.position = ((Locatable)this.owner).pos;
		}
		//		Debug.Log ("NormalBomb:(x,y)="+this.position.x+","+this.position.y);
	}

	public SetBomb Owner {
		get{return this.owner;}
		set{this.owner = value;}
	}
	public GameObject Fire{
		get{return this.fire;}
		set{this.fire = value;}
	}
	public int LifeTime{
		get{return lifeTime;}
		set{lifeTime=value;}
	}
	private int fireTime = 1;
	public int FireTime {
		get;
		set;
	}

	public int Blood 
	{
		get{return lifeTime;}
		set{lifeTime=value;}
	}

	public int Power   
	{  
		get {return power;}  
		set {power = value;}  
	} 

	public void attackBy(Attackable source){
		//		Debug.Log ( "Attackable source:"+((Locatable)source).pos.x +","+((Locatable)source).pos.y );
		//		Debug.Log("source is BombFire:"+source is BombFire);
		//		if (source is BombFire && isActive) {
		////			Debug.Log ("attackBy()...");
		//			lifeTime = 0;
		//			this.createFire ();
		//			isActive = false;
		//			this.distroy ();
		//		}
		lifeTime = 0;
	}

	public void distroy(){
//		if (owner.CurrNum > 0) {
//			owner.CurrNum -= 1;
//		}
		owner.notifyExplosion();
		GameDataProcessor.instance.removeObject (this);
		RhythmRecorder.instance.removeObserver (this);
		Destroy(this.gameObject,0);
	}

	public void actionOnBeat(){
		//		Debug.Log ("lifeTime:" + lifeTime.ToString());
		--lifeTime;

	}
	private void createFire(){
		GameObject[] fires = new GameObject[(power*2+1)*(power*2+1)];

		Vector3 tempPos = this.gameObject.transform.position;
		int tempX = this.position.x;
		int tempY = this.position.y;
		int fireIndex = 0;
		for (int i = -power; i <= power; i++)
			for (int j = -power; j <= power; j++) {
				tempPos.x =this.gameObject.transform.position.x+ stepLenth*i;
				tempX = this.position.x+stepLenth*i;
				tempPos.z =this.gameObject.transform.position.z+ stepLenth*j;
				tempY = this.position.y-stepLenth*j;
				//Debug.Log (i+" "+j);

				if(tempX<0 || tempX >= GameDataProcessor.instance.mapSizeX||tempY<0 || tempY >= GameDataProcessor.instance.mapSizeY){
					continue;
				}

//				Debug.Log (tempX+" "+tempY);

				Position currPosition = new Position(tempX,tempY);

				fires [fireIndex] = (GameObject)Instantiate (fire, tempPos, this.gameObject.transform.rotation);
				NormalBombFire bfScript = (NormalBombFire)fires [fireIndex].GetComponent ("NormalBombFire");
				bfScript.setProperties (this.owner, fireTime);
				bfScript.pos = currPosition;
				fireIndex++;
			}
	}

}

