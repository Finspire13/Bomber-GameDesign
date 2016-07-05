using UnityEngine;
using System.Collections;

public class MapDataEncoder{

	//Represent MapData componet
	public static int C_NORMAL_CUBE = 3;
	public static int C_WALL_CUBE = 4;
	public static int C_EMPTY = 0;
	public static int C_PLAYER = 1;
	public static int C_ENEMY = 2;

	public enum MapDataCodeEnum{
		EMPTY = 0, 
		PLAYER = 1, 
		ENEMY = 2, 
		NORMAL_CUBE = 3, 
		WALL_CUBE = 4
	};
}
