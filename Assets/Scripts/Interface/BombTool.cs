using System;
using UnityEngine;

public interface BombTool
{
	void useToolBy(SetBomb user);
	KeyCode getKeyCode ();
	string getToolName();
	SetBomb Owner {
		get;
		set;
	}
}


