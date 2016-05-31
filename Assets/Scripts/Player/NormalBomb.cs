using UnityEngine;
using System.Collections;

public class NormalBomb :MonoBehaviour,Bomb,Distroyable,Locatable
{
	private SetBomb owner = null;
	private BombFire fire = null;
	private int lifeTime = 2;
	private int power = 5;
	private Position position;
	public Position pos{ 
		get{ return position; } 
		set{ position=value; }
	}
	

	public bool isActive = true;
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
//		Debug.Log ("NormalBomb :MonoBehaviour,Bomb,Distroyable,Locatable");
		GameDataProcessor.instance.addObject (this);
		RhythmRecorder.instance.addObservedSubject (this);
	}
	
	// Update is called once per frame
	void Update () {
		if (lifeTime <= 0 && isActive) {
			this.createFire();
			isActive = false;
			this.distroy();
		}
	}


	public void setProperties(SetBomb owner,int power,int lifeTime){
		this.owner = owner;
		this.lifeTime = lifeTime;
		this.power = power;
	}
	
	public SetBomb Owner {
		get{return this.owner;}
		set{this.owner = value;}
	}
	public BombFire Fire{
		get{return this.fire;}
		set{this.fire = value;}
	}
	public int LifeTime{
		get{return lifeTime;}
		set{lifeTime=value;}
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

	}
	public void distroy(){
		if (owner.CurrNum > 0) {
			owner.CurrNum -= 1;
		}
		GameDataProcessor.instance.removeObject (this);
		RhythmRecorder.instance.removeObserver (this);
		Destroy(this.gameObject,0);
	}

	public void actionOnBeat(){
//		Debug.Log ("lifeTime:" + lifeTime.ToString());
		--lifeTime;

	}
	private void createFire(){
		
		GameObject fire = Resources.Load("firebase") as GameObject;
		GameObject[] fires = new GameObject[power*4+1];
		//(GameObject)Instantiate(fire,this.gameObject.transform.position,this.gameObject.transform.rotation);
		Vector3 currPos = this.gameObject.transform.position;
		fires[0] = (GameObject)Instantiate(fire,currPos,this.gameObject.transform.rotation);
		NormalBombFire bfScript = (NormalBombFire)fires[0].GetComponent("NormalBombFire");
		bfScript.pos = new Position(this.position.x,this.position.y);

		Vector3 tempPos = currPos;
		int tempX = this.position.x;
		for(int i = 1;i < power+1;++i){
			tempPos.x += stepLenth;
			tempX += stepLenth;

			fires[i] = (GameObject)Instantiate(fire,tempPos,this.gameObject.transform.rotation);
			bfScript = (NormalBombFire)fires[i].GetComponent("NormalBombFire");
			bfScript.pos = new Position(tempX,this.position.y);
		}

		tempPos = currPos;
		tempX = this.position.x;
		for(int i = power+1;i < power*2+1;++i){
			tempPos.x -= stepLenth;
			tempX -= stepLenth;

			fires[i] = (GameObject)Instantiate(fire,tempPos,this.gameObject.transform.rotation);
			bfScript = (NormalBombFire)fires[i].GetComponent("NormalBombFire");
			bfScript.pos = new Position(tempX,this.position.y);
		}

		tempPos = currPos;
		int tempY = this.position.y;
		for(int i = power*2+1;i < power*3+1;++i){
			tempPos.z += stepLenth;
			tempY += stepLenth;

			fires[i] = (GameObject)Instantiate(fire,tempPos,this.gameObject.transform.rotation);
			bfScript = (NormalBombFire)fires[i].GetComponent("NormalBombFire");
			bfScript.pos = new Position(this.position.x,tempY);
		}
		tempPos = currPos;
		tempY = this.position.y;
		for(int i = power*3+1;i < power*4+1;++i){
			tempPos.z -= stepLenth;
			tempY -= stepLenth;

			fires[i] = (GameObject)Instantiate(fire,tempPos,this.gameObject.transform.rotation);
			bfScript = (NormalBombFire)fires[i].GetComponent("NormalBombFire");
			bfScript.pos = new Position(this.position.x,tempY);
		}
	}

}

