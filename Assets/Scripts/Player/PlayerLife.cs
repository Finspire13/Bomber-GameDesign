using UnityEngine;
using System.Collections;

public class PlayerLife : MonoBehaviour,Distroyable
{
	private int blood;
	public int Blood 
	{
		get{return blood;}
		set{blood = value;}
	}
	public void attackBy(Attackable source){
		blood -= source.Damage;
	}
	public void distroy(){
		Destroy (this.gameObject, 0);
	}

	public void actionOnBeat(){
		
	}
	// Use this for initialization
	void Start ()
	{
		blood = 5;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (blood <= 0) {
			Debug.Log("game over!!!");
			distroy();
		}
	}
}

