using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	private int stepLenth = 1;
	private int power = 4;
	public int Power   
	{  
		get {return power;}  
		set {power = value;}  
	} 
	private bool active = false;
	public bool Active   
	{  
		get {return active;}  
		set {active = value;}  
	} 
	private int lifeTime = 10;
	public int LifeTime   
	{  
		get {return lifeTime;}  
		set {lifeTime = value;}  
	} 
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () {
		if (active) {
			lifeTime--;
		}

//		Debug.Log ("lifeTime:" + lifeTime.ToString());
		if (lifeTime <= 0 && active) {
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

			active = false;
			Destroy(this.gameObject,5);
		}
	}
}
