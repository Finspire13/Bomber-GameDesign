using System;
using UnityEngine;
using System.Collections;

public class BombPushTool: BombTool
{
	private KeyCode code = KeyCode.X;
	private SetBomb owner;
	private String toolName="BombPushTool";

	public BombPushTool (SetBomb owner)
	{
		this.owner = owner;
	}

	public string getToolName(){
		return toolName;
	}

	public void useToolBy(SetBomb user){
//		ArrayList bombList = owner.getAllBomb ();
//		for (int i = 0; i < bombList.Count; ++i) {
//			if (bombList [i] is Bomb) {
//				((Bomb)bombList [i]).LifeTime = 0;
//			}
//		}
//		bombList.Clear ();
		Debug.Log("Push bomb......");
	}

	public KeyCode getKeyCode (){
		return code;
	}

	public SetBomb Owner {
		get{ return owner;}
		set{ this.owner = value;}
	}

}


