using UnityEngine;
using System.Collections;

public struct Position{
	public int _x;  //left-bottom tp right-top, from 0 to mapSize-1
	public int _y;

	public Position(int newX,int newY){
		_x = newX;
		_y = newY;
	}
	public int x
	{
		get{ return _x; }
		set{ _x = value; }
	}
	public int y
	{
		get{ return _y; }
		set{ _y = value; }
	}
}

public interface Locatable{
	Position pos{ get; set;}
}

public class Test:Locatable{
	private Position _pos;

	public Position pos
	{ 
		get{return _pos;}
		set{_pos=value;}
	}
}

public class GameDataProcessor : MonoBehaviour {

	public static GameDataProcessor instance = null;

	public int mapSizeX;
	public int mapSizeY;

	private ArrayList gameObjects;
	// Use this for initialization

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);    

		DontDestroyOnLoad (gameObject);

		gameObjects = new ArrayList ();
		mapSizeX = 5;
		mapSizeY = 5;
	}

	void Start () {
		Locatable test1 = new Test ();
		test1.pos = new Position (2, 3);
		Locatable test2 = new Test ();
		test2.pos = new Position (2, 4);
		GameDataProcessor.instance.addObject (test1);
		GameDataProcessor.instance.addObject (test2);

//		Debug.Log(GameDataProcessor.instance.getFrontalObjects (test1, 1).Count);
//		Debug.Log (GameDataProcessor.instance.getFrontalObjects (test1, 1)[0].GetType());

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool addObject(Locatable item){
		if (item.pos.x < 0 || item.pos.x >= mapSizeX || item.pos.y < 0 || item.pos.y >= mapSizeY)
			return false;
	
		gameObjects.Add (item);
		return true;
	}

	public void removeObject(Locatable item){
		gameObjects.Remove (item);
	}

	public void clearObjects(){
		gameObjects.Clear ();
	}

	public bool updatePosition(Locatable target, Position pos){
		if (gameObjects.IndexOf (target) == -1)
			return false;
		if (pos.x < 0 || pos.x >= mapSizeX || pos.y < 0 || pos.y >= mapSizeY)
			return false;

		target.pos = pos;
		return true;
	}

	public ArrayList getFrontalObjects(Locatable item,int range=1){

		if (gameObjects.IndexOf (item) == -1)
			return null;


		int itemY = item.pos.y;
		int itemX = item.pos.x;
		ArrayList result = new ArrayList ();

		for (int i = 0; i < gameObjects.Count; i++) {
			Locatable temp = (Locatable) gameObjects [i];
			if (temp.pos.x == itemX && temp.pos.y > itemY && temp.pos.y <= itemY + range) {
				result.Add (temp);
			}

		}
		return result;
	}

	public ArrayList getBackObjects(Locatable item,int range=1){

		if (gameObjects.IndexOf (item) == -1)
			return null;

		int itemY = item.pos.y;
		int itemX = item.pos.x;
		ArrayList result = new ArrayList ();

		for (int i = 0; i < gameObjects.Count; i++) {
			Locatable temp = (Locatable) gameObjects [i];
			if (temp.pos.x == itemX && temp.pos.y < itemY && temp.pos.y >= itemY - range) {
				result.Add (temp);
			}
		}
		return result;
	}

	public ArrayList getRightObjects(Locatable item,int range=1){

		if (gameObjects.IndexOf (item) == -1)
			return null;

		int itemY = item.pos.y;
		int itemX = item.pos.x;
		ArrayList result = new ArrayList ();

		for (int i = 0; i < gameObjects.Count; i++) {
			Locatable temp = (Locatable) gameObjects [i];
			if (temp.pos.y == itemY && temp.pos.x > itemX && temp.pos.x <= itemX + range) {
				result.Add (temp);
			}
		}
		return result;
	}

	public ArrayList getLeftObjects(Locatable item,int range=1){

		if (gameObjects.IndexOf (item) == -1)
			return null;

		int itemY = item.pos.y;
		int itemX = item.pos.x;
		ArrayList result = new ArrayList ();

		for (int i = 0; i < gameObjects.Count; i++) {
			Locatable temp = (Locatable) gameObjects [i];
			if (temp.pos.y == itemY && temp.pos.x < itemX && temp.pos.x >= itemX - range) {
				result.Add (temp);
			}
		}
		return result;
	}

	public ArrayList getObjectsAtTheSamePosition(Locatable item){

		if (gameObjects.IndexOf (item) == -1)
			return null;

		int itemY = item.pos.y;
		int itemX = item.pos.x;
		ArrayList result = new ArrayList ();

		for (int i = 0; i < gameObjects.Count; i++) {
			Locatable temp = (Locatable) gameObjects [i];
			if (temp.pos.y == itemY && temp.pos.x == itemX) {
				result.Add (temp);
			}
		}
		return result;
	}


}
