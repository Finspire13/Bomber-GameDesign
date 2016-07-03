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

public class GameDataProcessor : MonoBehaviour,RhythmObservable {

	public static GameDataProcessor instance = null;

	public int mapSizeX;
	public int mapSizeY;

	public ArrayList gameObjects;
	// Use this for initialization

	public int[,] dangerMap = null; //for ai
	public float[,] benefitMap = null; //for ai

	private bool mapInitClock = true;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);    

		DontDestroyOnLoad (gameObject);

		gameObjects = new ArrayList ();

	}

	void Start () {
//		Locatable test1 = new Test ();
//		test1.pos = new Position (2, 3);
//		Locatable test2 = new Test ();
//		test2.pos = new Position (2, 4);
//		GameDataProcessor.instance.addObject (test1);
//		GameDataProcessor.instance.addObject (test2);

//		Debug.Log(GameDataProcessor.instance.getFrontalObjects (test1, 1).Count);
//		Debug.Log (GameDataProcessor.instance.getFrontalObjects (test1, 1)[0].GetType());
		RhythmRecorder.instance.addObservedSubject(this);
//		initizeMap ();
//		Debug.Log ("mapSizeX:"+mapSizeX+",mapSizeY:"+mapSizeY);
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log ("game data");
//		Debug.Log (gameObjects.Count);
//		Debug.Log ("mapSizeX:"+mapSizeX+",mapSizeY:"+mapSizeY);
		if (mapInitClock && mapSizeX*mapSizeY != 0 ) {
			dangerMap = new int[mapSizeX, mapSizeY];
			benefitMap = new float[mapSizeX, mapSizeY];
			initizeMap ();
			mapInitClock = false;
			int temp = mapSizeX;
			mapSizeX = mapSizeY;
			mapSizeY = temp;
		}
	}

	public bool isInitialized(){
		return (!mapInitClock);
	}

	public bool addObject(Locatable item){
//		if (item.pos.x < 0 || item.pos.x >= mapSizeX || item.pos.y < 0 || item.pos.y >= mapSizeY)
//			return false;
	
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

	public void actionOnBeat (){
//		int ran = this.getRandom (10);
//		Debug.Log ("random:"+ran);
		if (this.getRandom (10) == 0) {
//			Debug.Log ("refresh danger map");
			refreshDangerMap ();
		}
		if (this.getRandom(15) == 0){
//			Debug.Log ("refresh benefit map!!!");
			refreshBenefitMap ();
		}
		updateDangerMap ();
		updateBenefitMap ();
	}
	public void refreshDangerMap(){
		if (dangerMap != null) {
			for (int i = 0; i < dangerMap.GetLength (0); ++i) {
				for (int j = 0; j < dangerMap.GetLength (1); ++j) {
					dangerMap [i, j] = -1;
				}
			}
		}
	}

	public void initizeMap(){
		for (int i = 0; i < dangerMap.GetLength (0); ++i) {
			for (int j = 0; j < dangerMap.GetLength (1); ++j) {
				dangerMap [i,j] = -1;
			}
		}

		for (int i = 0; i < benefitMap.GetLength (0); ++i) {
			for (int j = 0; j < benefitMap.GetLength (1); ++j) {
				benefitMap [i,j] = 0f;
			}
		}
	}

	public ArrayList getObjectAtPostion(Position pos){

		int itemY = pos.y;
		int itemX = pos.x;

		ArrayList result = new ArrayList ();

		for (int i = 0; i < gameObjects.Count; i++) {
			Locatable temp = (Locatable) gameObjects [i];
			if (temp.pos.y == itemY && temp.pos.x == itemX) {
				result.Add (temp);
			}
		}

		return result;
	}

	private void refreshDangerMap(Position pos,int dangerValue){
		if (pos.x>=0 && pos.x<mapSizeX && pos.y>=0 && pos.y<mapSizeY &&
			dangerMap [pos.y, pos.x] != -1 && dangerMap [pos.y, pos.x] > dangerValue) {
			dangerMap [pos.y, pos.x] = dangerValue;
			ArrayList objs = getObjectAtPostion (pos);
			if (objs != null) {
				for (int indx = 0; indx < objs.Count; ++indx) {
					if (objs [indx] is Bomb) {
						Bomb bomb = (Bomb)objs [indx];
						for (int i = pos.x - bomb.Power; i < pos.x + bomb.Power + 1; ++i) {
							if (i >= 0 && i < this.mapSizeX && dangerMap [pos.y, i] > dangerValue) {
								dangerMap [pos.y, i] = dangerValue;
							}
							refreshDangerMap (new Position (i, pos.y), dangerValue);
						}
						for (int j = pos.y - bomb.Power; j < pos.y + bomb.Power + 1; ++j) {
							if (j >= 0 && j < this.mapSizeY && dangerMap [j, pos.x] > dangerValue) {
								dangerMap [j, pos.x] = dangerValue;
							}
							refreshDangerMap (new Position (pos.x, j), dangerValue);
						}
					}
				}
			}
		}
	}

	public void addToDangerMap(Bomb bomb){
		if(bomb is Locatable){
			Position pos = ((Locatable)bomb).pos;
			if (dangerMap [pos.y, pos.x] == -1) {
				dangerMap [pos.y, pos.x] = bomb.LifeTime;
				for (int i = pos.x - bomb.Power; i < pos.x + bomb.Power + 1; ++i) {
					if (i >= 0 && i < this.mapSizeX  && dangerMap [pos.y, i] == -1) {
						dangerMap [pos.y, i] = bomb.LifeTime;
					}
					Position currPosition = new Position (i, pos.y);
					refreshDangerMap (currPosition, bomb.LifeTime);
				}

				for (int j = pos.y - bomb.Power; j < pos.y + bomb.Power + 1; ++j) {
					if (j >= 0 && j < this.mapSizeY && dangerMap [j, pos.x] == -1) {
						dangerMap [j, pos.x] = bomb.LifeTime;
					}
					refreshDangerMap (new Position (pos.x, j), bomb.LifeTime);
				}
			} else {
				if (dangerMap [pos.y, pos.x] <= bomb.LifeTime) {
					for (int i = pos.x - bomb.Power; i < pos.x + bomb.Power + 1; ++i) {
						if (i >= 0 && i < this.mapSizeX && (dangerMap [pos.y, i] == -1 || dangerMap [pos.y, i] > dangerMap [pos.y, pos.x])) {
							dangerMap [pos.y, i] = dangerMap [pos.y, pos.x];
						}
					}

					for (int j = pos.y - bomb.Power; j < pos.y + bomb.Power + 1; ++j) {
						if (j >= 0 && j < this.mapSizeY && (dangerMap [j, pos.x] == -1 || dangerMap [j, pos.x] > dangerMap [pos.y, pos.x])) {
							dangerMap [j, pos.x] = dangerMap [pos.y, pos.x];
						}
					}
				}
				refreshDangerMap (pos, bomb.LifeTime);
			}
		}
	}

	public void updateDangerMap(){
		if (dangerMap != null) {
			for (int i = 0; i < dangerMap.GetLength (0); ++i) {
				for (int j = 0; j < dangerMap.GetLength (1); ++j) {
					if (dangerMap [i, j] > 0) {
						--dangerMap [i, j];
					}
				}
			}
		}
	}

	public void removeFromDangerMap(BombFire fire){
		if(fire is Locatable){
			Position pos = ((Locatable)fire).pos;
			dangerMap [pos.y, pos.x] = -1;
		}
	}

	public void addToBenefitMap(Buff buff){
		if (buff is Locatable) {
			int x = ((Locatable)buff).pos.x;
			int y = ((Locatable)buff).pos.y;
			this.benefitMap [y,x] += (float)buff.Value;
		}
	}
	public void removeFromBenefitMap(Buff buff){
		if (buff is Locatable) {
			int x = ((Locatable)buff).pos.x;
			int y = ((Locatable)buff).pos.y;
			float value = this.benefitMap [y,x] - buff.Value;
			if (value > 0) {
				this.benefitMap [y,x] = value;
			} else {
				this.benefitMap [y,x] = 0f;
			}
		}
	}

	public void refreshBenefitMap(){
		if (benefitMap != null) {
			for (int i = 0; i < benefitMap.GetLength (0); ++i) {
				for (int j = 0; j < benefitMap.GetLength (1); ++j) {
					if (benefitMap [i, j] >= 5) {
						benefitMap [i, j] = 0f;
					}
				}
			}
		}
	}
	//for benefitMap's value of player position
	private void updatePlayerValue(){
		ArrayList positions =  RhythmRecorder.instance.getPlayersPosition ();
		for (int i = 0; i < positions.Count; ++i) {
			if (positions [i] is Locatable) {
				Position pos = ((Locatable)positions [i]).pos;
				benefitMap [pos.y, pos.x] += (float)getRandom(5);
			}
		}
	}

	public void updateBenefitMap(){
		if (benefitMap != null) {
			for (int i = 0; i < benefitMap.GetLength (0); ++i) {
				for (int j = 0; j < benefitMap.GetLength (1); ++j) {
					if (benefitMap [i, j] > 0) {
						--benefitMap [i, j];
					}
				}
			}
		}
		updatePlayerValue ();
	}

	public int getRandom (int max){
		return new System.Random ().Next (max);
	}

}
