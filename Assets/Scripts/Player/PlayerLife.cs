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
		Destroy (this.gameObject, 1);
	}

	public void actionOnBeat(){
		
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (blood <= 0) {
			distroy();
		}
	}
}

