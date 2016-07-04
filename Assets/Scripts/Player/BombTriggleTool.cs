using System;
using UnityEngine;
using System.Collections;

public class BombTriggleTool: BombTool
{
	private KeyCode code = KeyCode.Z;
	private SetBomb owner;
	private String toolName="BombTriggleTool";

	public BombTriggleTool (SetBomb owner)
	{
		this.owner = owner;
	}

	public string getToolName(){
		return toolName;
	}

	public void useToolBy(SetBomb user){
		ArrayList bombList = user.getAllBomb ();
		for (int i = 0; i < bombList.Count; ++i) {
			if (bombList [i] is Bomb) {
				((Bomb)bombList [i]).LifeTime = 0;
			}
		}
		bombList.Clear ();
	}

	public KeyCode getKeyCode (){
		return code;
	}

	public SetBomb Owner {
		get{ return owner;}
		set{ this.owner = value;}
	}

}

