using System;
using UnityEngine;
using System.Collections;

public class BombPushTool: BombTool,ScoreCount
{
	private KeyCode code = KeyCode.X;
	private SetBomb owner;
	private string toolName="BombPushTool";
//	private string gameName = "Tool-BombPushTool";
	private float gameValue = 50f;
	public string getName(){
		return "Tool-"+this.toolName;
	}
	public float getValue(){
		return this.gameValue;
	}
	public void addToScore(){
//		GameManager.instance.addToPlayerScoreList (this);
	}

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
		if (user is Locatable) {
			Position userPos = (user as Locatable).pos;
			Position[] dirs = {new Position (userPos.x+1,userPos.y),new Position (userPos.x-1,userPos.y),
				new Position (userPos.x,userPos.y+1),new Position (userPos.x,userPos.y-1) };
			for (int i = 0; i < dirs.GetLength (0); ++i) {
				ArrayList objs = GameDataProcessor.instance.getObjectAtPostion (dirs[i]);
				for (int j = 0; j < objs.Count; ++j) {
					if (objs [j] is Bomb && objs [j] is Locatable) {
						Locatable bomb = objs [j] as Locatable;
						Position finalPos = new Position(bomb.pos.x,bomb.pos.y);

						int deltaX = bomb.pos.x - userPos.x;
						int deltaY = bomb.pos.y - userPos.y;
						bool isFinalPosition = false;
						while (!isFinalPosition) {
							finalPos.x += deltaX;
							finalPos.y += deltaY;
							ArrayList obstacles = GameDataProcessor.instance.getObjectAtPostion (finalPos);
							foreach (Locatable obs in obstacles) {
								finalPos = new Position (obs.pos.x, obs.pos.y);
								isFinalPosition = true;
								break;
							}
						}
						((Bomb)bomb).pushTo (new Position(finalPos.x-deltaX,finalPos.y-deltaY));
					}
				}
			}
		}
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


