using UnityEngine;
using System.Collections;

public class NormalBomb :MonoBehaviour,Bomb,Distroyable,Locatable
{
	private GameObject owner = null;
	private BombFire fire = null;
	private int lifeTime = 5;
	private int power = 4;

	private bool isActive = true;
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
	public void setProperties(GameObject owner,BombFire fire,int power,int lifeTime){
		this.owner = owner;
		this.lifeTime = lifeTime;
		this.fire = fire;
		this.power = power;
	}
	
	public GameObject Owner {
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
	public int Power   
	{  
		get {return power;}  
		set {power = value;}  
	} 


	public void attackBy(Attackable source){

	}
	public void distroy(){
		Destroy(this.gameObject,2);
	}

	public void actionOnBeat(){
		--lifeTime;
		if (lifeTime <= 0 && isActive) {
			this.createFire();
			isActive = false;
			this.distroy();
		}
	}
	private void createFire(){
		Debug.Log ("lifeTime:" + lifeTime.ToString());
		
		GameObject fire = Resources.Load("firebase") as GameObject;
		GameObject[] fires = new GameObject[power*4+1];
		//(GameObject)Instantiate(fire,this.gameObject.transform.position,this.gameObject.transform.rotation);
		Vector3 currPos = this.gameObject.transform.position;
		fires[0] = (GameObject)Instantiate(fire,currPos,this.gameObject.transform.rotation);
		Vector3 tempPos = currPos;
		for(int i = 1;i < power+1;++i){
			tempPos.x += stepLenth;
			fires[i] = (GameObject)Instantiate(fire,tempPos,this.gameObject.transform.rotation);
		}
		tempPos = currPos;
		for(int i = power+1;i < power*2+1;++i){
			tempPos.x -= stepLenth;
			fires[i] = (GameObject)Instantiate(fire,tempPos,this.gameObject.transform.rotation);
		}
		tempPos = currPos;
		for(int i = power*2+1;i < power*3+1;++i){
			tempPos.z += stepLenth;
			fires[i] = (GameObject)Instantiate(fire,tempPos,this.gameObject.transform.rotation);
		}
		tempPos = currPos;
		for(int i = power*3+1;i < power*4+1;++i){
			tempPos.z -= stepLenth;
			fires[i] = (GameObject)Instantiate(fire,tempPos,this.gameObject.transform.rotation);
		}
	}

}

