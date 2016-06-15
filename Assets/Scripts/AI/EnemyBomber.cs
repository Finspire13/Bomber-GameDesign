using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum EnemyState{EMEMY_IDLE,EMEMY_WALK,EMEMY_SETBOMB,EMEMY_AVOID};

public class EnemyBomber : MonoBehaviour,Distroyable,SetBomb,Locatable,CanBuffed
{
	int speed = 1;
	public int Speed 
	{
		get{return speed; }
		set{speed = value; }
	}
	private bool rhmFlag;
	private int blood;

	private int maxNum;
	private int currNum;
	private Position position;

	public Position pos{ 
		get{ return position;}
		set{ position=value;}
	}

	// Use this for initialization
	void Start ()
	{
		this.bombType = Resources.Load("NormalBomb") as GameObject;
		RhythmRecorder.instance.addObservedSubject (this);
		this.position = new Position(Mathf.RoundToInt(transform.localPosition.z),Mathf.RoundToInt(transform.localPosition.x));

		GameDataProcessor.instance.addObject (this);


		Debug.Log ("enemy pos:x="+position.x+",y="+position.y);

		this.maxNum = 3;
		this.currNum = 0;

		this.bombLifeTime = 3;
		this.bombPower = 2;
		this.bombFireTime = 1;
		this.currPath = new Queue<Position>();
	}
	
	// Update is called once per frame
	void Update ()
	{
//		control ();
//		if (mapInitClock && GameDataProcessor.isInitialized()) {
		//			decisionMap = new int[GameDataProcessor.mapSizeY, GameDataProcessor.mapSizeX];
//			mapInitClock = false;
//		}
	}

//	public void control(){
//
//	}

	public bool rhythmFlag{ 
		get{ return rhmFlag;} 
		set{ rhmFlag = value;}
	}

	public int Blood 
	{
		get{return blood; }
		set{blood = value; }
	}
	public void attackBy(Attackable source){
		
	}
	public void distroy(){
		Destroy (this.gameObject, 0);
	}

	public int MaxNum {
		get{return maxNum;}
		set{maxNum = value;}
	}

	//already used number of bomb
	public int CurrNum {
		get{return currNum;}
		set{currNum = value;}
	}

	private int bombPower = 4;
	public int BombPower {
		get{return bombPower; }
		set{bombPower = value; }
	}
	private int bombLifeTime = 5;
	public int BombLifeTime {
		get{return bombLifeTime; }
		set{bombLifeTime = value; }
	}
	private int bombFireTime = 1;
	public int BombFireTime {
		get{return bombFireTime; }
		set{bombFireTime = value; }
	}

	public void notifyExplosion (){
		
	}

	private GameObject bombType;
	public void setBomb(GameObject bombType){
		this.bombType = bombType;
	}

	public void installBomb(){
		if(currNum < maxNum){
			currNum++;
			if(bombType == null){
//				Debug.Log("not bomb");
			}else{
				GameObject go = (GameObject)Instantiate(this.bombType,this.gameObject.transform.position,this.gameObject.transform.rotation);
				NormalBomb script = (NormalBomb)go.GetComponent("Bomb");
				if(script == null){
//					Debug.Log("not script");
				}else{
//					Debug.Log("find script interface Bomb");
					//script.LifeTime = bomblifeTime;
					//script.isActive = true;
					//					script.LifeTime = 3;
					script.setProperties(this,bombPower,bombLifeTime,bombFireTime);

					GameDataProcessor.instance.addToDangerMap (script);
				}
			}
		}
	}

	// *************** AI control ***************
//	private int[,] decisionMap = null;
//	private bool mapInitClock = true;
	Queue<Position> currPath;
	EnemyState lastState;

	private int getRandom(int count)
	{
		return new System.Random().Next(count);

	}

	public void actionOnBeat (){
		EnemyState result  = enemyThink();
		switch (result) {
		case EnemyState.EMEMY_AVOID:
			walkAction ();
			break;
		case EnemyState.EMEMY_IDLE:
			idleAction ();
			break;
		case EnemyState.EMEMY_WALK:
			walkAction ();
			break;
		case EnemyState.EMEMY_SETBOMB:
			setBombAction ();
			break;
		}
		lastState = result;
	}

	private EnemyState enemyThink(){
		if (isNowSafe ()) {
		
			ArrayList decisionMap = computDecisionMap ();
			//run before initialization of gamedataprocessor.dangermap;
			if (decisionMap == null) {
				return EnemyState.EMEMY_IDLE;
			}

			int indx = getRandom (3);
			if (currPath == null || currPath.Count <= 0) {
				MyPair pair = decisionMap [indx] as MyPair;
				Position dest = new Position (pair.px, pair.py);
				this.currPath = findPathTo (dest);
				Debug.Log ("walk to dest and think again!");
				return EnemyState.EMEMY_WALK;
			}

			indx = getRandom (15);
			if (indx < 3 && decisionMap.Count > indx) {
				MyPair pair = decisionMap [indx] as MyPair;
				Position dest = new Position (pair.px, pair.py);
//				Debug.Log ("dest:" + dest.x + "," + dest.y);
				this.currPath = findPathTo (dest);

//			Debug.Log ("EMEMY_WALK");
				return EnemyState.EMEMY_WALK;
			} else if (indx < 5) {
//			Debug.Log ("EMEMY_IDLE");
				return EnemyState.EMEMY_IDLE;
			} else if (indx < 9 && lastState == EnemyState.EMEMY_WALK) {
//			Debug.Log ("EMEMY_SETBOMB");
				return EnemyState.EMEMY_SETBOMB;
			}

//		Debug.Log ("EMEMY_WALK");
			return EnemyState.EMEMY_WALK;
		} else {
//			Debug.Log ("Unsafe!!");
			currPath.Clear ();
			int[,] dangerMap = GameDataProcessor.instance.dangerMap;
	
			int maxX = GameDataProcessor.instance.mapSizeX;
			int maxY = GameDataProcessor.instance.mapSizeY;
			if (!isWall(new Position (this.pos.x + 1, this.pos.y)) && this.pos.x + 1 < maxX &&
				(dangerMap [this.pos.y, this.pos.x + 1] >= 2 || dangerMap [this.pos.y, this.pos.x + 1] == -1)) {
				currPath.Enqueue (new Position (this.pos.x + 1, this.pos.y));
				Debug.Log ("to right:");
			} else if (!isWall(new Position (this.pos.x - 1, this.pos.y)) && this.pos.x - 1 >= 0 && 
				(dangerMap [this.pos.y, this.pos.x - 1] >= 2 || dangerMap [this.pos.y, this.pos.x - 1] == -1)) {
				currPath.Enqueue (new Position (this.pos.x - 1, this.pos.y));
				Debug.Log ("to left:");
			} else if (!isWall(new Position (this.pos.x, this.pos.y+1)) && this.pos.y+1 < maxY && 
				(dangerMap [this.pos.y+1, this.pos.x] >= 2 || dangerMap [this.pos.y+1, this.pos.x ] == -1)) {
				currPath.Enqueue (new Position (this.pos.x, this.pos.y+1));
				Debug.Log ("to down:");
			} else if (!isWall(new Position (this.pos.x, this.pos.y-1)) && this.pos.y-1 >= 0 && 
				(dangerMap [this.pos.y-1, this.pos.x] >= 2 || dangerMap [this.pos.y-1, this.pos.x ] == -1)) {
				currPath.Enqueue (new Position (this.pos.x , this.pos.y-1));
				Debug.Log ("to up:");
			}

			return EnemyState.EMEMY_AVOID;
		}
	}
	private bool isWall(Position position){
		ArrayList objs = GameDataProcessor.instance.getObjectAtPostion (position);
		for (int i = 0; i < objs.Count; ++i) {
			if (objs [i] is NormalBomb || objs [i] is WallCube) {
				return true;
			}
		}
		return false;
	}
	private bool isNowSafe(){
		int p = this.getRandom (20);
		if (p < 18) {
			ArrayList objs = GameDataProcessor.instance.getObjectAtPostion (this.pos);
			for (int i = 0; i < objs.Count; ++i) {
				if (objs [i] is BombFire) {
//					Debug.Log ("safe: BombFire");
					return false;
				}
			}
			int[,] dangerMap = GameDataProcessor.instance.dangerMap;
			if (dangerMap [this.pos.y, this.pos.x] < 2 && dangerMap [this.pos.y, this.pos.x] >= 0) {
//				Debug.Log ("safe: danger");
				return false;
			}
			return true;
		} else {
//			Debug.Log ("ignore safety");
			return true;
		}
	}
	private void idleAction(){
//		Debug.Log("Enemy idle");
		this.currPath.Clear();
	}
	private void walkAction(){
//		while(currPath.Count > 0){
//			Position temp = currPath.Dequeue();
//			Debug.Log("->("+temp.x+","+temp.y+")");
//		}
//		Debug.Log("Enemy walk");
		if (currPath != null && currPath.Count > 0) {
			Position temp = currPath.Dequeue ();
			Debug.Log("->("+temp.x+","+temp.y+")");
			float deltaX = ((float)(temp.y - this.pos.y)/4f);
			float deltaY = ((float)(temp.x - this.pos.x)/4f);

//			Debug.Log ("move to y:"+((float)(temp.y - this.pos.y))/4f);
//			Debug.Log ("move to x:"+((float)(temp.x - this.pos.x))/4f);

			StartCoroutine(enemyMove (deltaX,deltaY));
//			this.transform.position += (temp.y - this.pos.y) * Vector3.back;
//			this.transform.position += (temp.x - this.pos.x) * Vector3.right;

			this.pos = temp;
//			Debug.Log ("position: " + this.transform.position + " ");
		}

	}
	private void setBombAction(){
//		Debug.Log("Enemy setBomb");
		installBomb ();
	}
//	private void avoidAction(){
//		if (currPath != null && currPath.Count > 0) {
//			Position temp = currPath.Dequeue ();
//			Debug.Log("->("+temp.x+","+temp.y+")");
//			this.transform.position += (temp.y - this.pos.y) * Vector3.back;
//			this.transform.position += (temp.x - this.pos.x) * Vector3.right;
//			this.pos = temp;
//			Debug.Log ("position: " + this.transform.position + " ");
//		}
//	}
	private IEnumerator enemyMove(float deltaX,float deltaY){
		for (int i = 0; i < 4; ++i) {
			this.transform.position += deltaX * Vector3.back;
			this.transform.position += deltaY * Vector3.right;
//			Debug.Log ("move to y:"+((float)(direction.y - this.pos.y)/4f));
//			Debug.Log ("move to x:"+((float)(direction.x - this.pos.x)/4f));
			yield return null;
		}
	}
	//寻路算法
	private Queue<Position> findPathTo(Position dest){
		Queue<Position> path = new Queue<Position> ();

		int[,] floodMark = new int[GameDataProcessor.instance.mapSizeY,GameDataProcessor.instance.mapSizeX];
		bool isReachDest = false;
		for (int i = 0; i < floodMark.GetLength (0); ++i) {
			for (int j = 0; j < floodMark.GetLength (1); ++j) {
				floodMark[i,j] = -1;
			}
		}
		floodMark [this.pos.y, this.pos.x] = 0;
		bool canReach = markPath (this.pos,dest,1,ref floodMark,ref isReachDest);

		ArrayList objs = GameDataProcessor.instance.getObjectAtPostion (dest);
		for (int i = 0; i < objs.Count; ++i) {
			if (objs [i] is NormalCube || objs [i] is WallCube) {
				canReach = false;
				break;
			}
		}
//		canReach = !isWall(dest);

		if (canReach) {
			Stack<Position> pathStack = new Stack<Position>();
			pathStack.Push (dest);
			this.createPath (dest, floodMark [dest.y, dest.x],ref floodMark, ref pathStack);

			while(pathStack.Count > 0){
				path.Enqueue(pathStack.Pop());
			}
		}
//		Position[] arrPath = path.ToArray ();
		return path;
	}
	private bool markPath (Position lastPos,Position target,int pathDistance,ref int[,] floodMark,ref bool isReachDest){
		if (lastPos.x == target.x && lastPos.y == target.y) {
//			floodMark [lastPos.y, lastPos.x]
			isReachDest = true;
			return true;
		}

		ArrayList objs;
		Position curr = new Position(lastPos.x+1,lastPos.y);
		bool rightAccess = true;
		if (floodMark [curr.y, curr.x] != -1) {
			rightAccess = false;
		} else {
			objs = GameDataProcessor.instance.getObjectAtPostion(curr);
			for (int i = 0; i < objs.Count; ++i) {
				if (objs [i] is NormalCube || objs [i] is WallCube) {
					floodMark [curr.y, curr.x] = 999999;
				rightAccess = false;
					break;
				}
			}
		}
		if (rightAccess) {
			floodMark [curr.y, curr.x] = pathDistance;
		}

		curr = new Position(lastPos.x-1,lastPos.y);
		bool leftAccess = true;
		if (floodMark [curr.y, curr.x] != -1) {
			leftAccess = false;
		} else {
			objs = GameDataProcessor.instance.getObjectAtPostion (curr);
			for (int i = 0; i < objs.Count; ++i) {
				if (objs [i] is NormalCube || objs [i] is WallCube) {
					floodMark [curr.y, curr.x] = 999999;
					leftAccess = false;
					break;
				}
			}
		}
		if (leftAccess) {
			floodMark [curr.y, curr.x] = pathDistance;
		}

		curr = new Position(lastPos.x,lastPos.y+1);
		bool downAccess = true;
		if (floodMark [curr.y, curr.x] != -1) {
			downAccess = false;
		} else {
			objs = GameDataProcessor.instance.getObjectAtPostion (curr);
			for (int i = 0; i < objs.Count; ++i) {
				if (objs [i] is NormalCube || objs [i] is WallCube) {
					floodMark [curr.y, curr.x] = 999999;
					downAccess = false;
					break;
				}
			}
		}
		if (downAccess) {
			floodMark [curr.y, curr.x] = pathDistance;
		}

		curr = new Position(lastPos.x,lastPos.y-1);
		bool upAccess = true;
		if (floodMark [curr.y, curr.x] != -1) {
			upAccess = false;
		} else {
			objs = GameDataProcessor.instance.getObjectAtPostion (curr);
			for (int i = 0; i < objs.Count; ++i) {
				if (objs [i] is NormalCube || objs [i] is WallCube) {
					floodMark [curr.y, curr.x] = 999999;
					upAccess = false;
					break;
				}
			}
		}
		if (upAccess) {
			floodMark [curr.y, curr.x] = pathDistance;
		}

		if (rightAccess) {
			curr = new Position (lastPos.x + 1, lastPos.y);
			rightAccess = markPath (curr, target, pathDistance + 1, ref floodMark, ref isReachDest);
			if (isReachDest) {
				return true;
			}
		}
		if (leftAccess) {
			curr = new Position (lastPos.x - 1, lastPos.y);
			leftAccess = markPath (curr, target, pathDistance + 1, ref floodMark, ref isReachDest);
			if (isReachDest) {
				return true;
			}
		}
		if (downAccess) {
			curr = new Position (lastPos.x, lastPos.y + 1);
			downAccess = markPath (curr, target, pathDistance + 1, ref floodMark, ref isReachDest);
			if (isReachDest) {
				return true;
			}
		}
		if (upAccess) {
			curr = new Position (lastPos.x, lastPos.y - 1);
			upAccess = markPath (curr, target, pathDistance + 1, ref floodMark, ref isReachDest);
			if (isReachDest) {
				return true;
			}
		}
		return upAccess || rightAccess || downAccess || upAccess;

	}

	private void createPath(Position target, int distance, ref int[,] floodMark, ref Stack<Position> pathStatck){
//		Stack<Position> pathStatck = new Stack<Position>();
		if (distance == 1) {
			return;
		}
		int[,] dangerMap = GameDataProcessor.instance.dangerMap;
		int maxX = GameDataProcessor.instance.mapSizeX;
		int maxY = GameDataProcessor.instance.mapSizeY;
//		Debug.Log ("right:"+this.pos.y + "," + (this.pos.x + 1));
//		Debug.Log ("left:"+this.pos.y + "," + (this.pos.x - 1));
//		Debug.Log ("down:"+(this.pos.y+1) + "," + (this.pos.x ));
//		Debug.Log ("up:"+(this.pos.y-1) + "," + (this.pos.x));

		//in danger of explosion

		bool isRightSafe = (target.x + 1 < maxX) ? !(dangerMap [target.y, target.x + 1] == distance) : false;
		bool isLeftSafe = (target.x - 1 >= 0) ? !(dangerMap [target.y, target.x-1] == distance):false;
		bool isDownSafe = (target.y + 1 < maxY) ? !(dangerMap [target.y+1, target.x] == distance):false;
		bool isUpSafe = (target.y - 1 >= 0) ? !(dangerMap [target.y-1, target.x] == distance):false;

		if (isRightSafe && floodMark [target.y, target.x + 1] == distance - 1) {
			Position temp = new Position (target.x + 1, target.y);
			pathStatck.Push (temp);
			createPath (temp, distance - 1, ref floodMark, ref pathStatck);

		} else if (isLeftSafe && floodMark [target.y, target.x - 1] == distance - 1) {
			Position temp = new Position (target.x - 1, target.y);
			pathStatck.Push (temp);
			createPath (temp, distance - 1, ref floodMark, ref pathStatck);
			
		} else if (isDownSafe && floodMark [target.y + 1, target.x] == distance - 1) {
			Position temp = new Position (target.x, target.y + 1);
			pathStatck.Push (temp);
			createPath (temp, distance - 1, ref floodMark, ref pathStatck);

		} else if (isUpSafe && floodMark [target.y - 1, target.x] == distance - 1) {
			Position temp = new Position (target.x, target.y - 1);
			pathStatck.Push (temp);
			createPath (temp, distance - 1, ref floodMark, ref pathStatck);
		} else {
			//everywhere is no safe
			return;
		}


//		Queue<Position> path = new Queue<Position> ();
//		while(pathStatck.Count > 0){
//			path.Enqueue(pathStatck.Pop());
//		}
//		return pathStatck;
	}

	private ArrayList computDecisionMap(){
		ArrayList topValuePlace = new ArrayList();
		int[,] dangerMap = GameDataProcessor.instance.dangerMap;
		float[,] benefitMap = GameDataProcessor.instance.benefitMap;
		if (dangerMap == null || benefitMap == null) {
			return null;
		}

		for (int i = 0; i < dangerMap.GetLength (0); ++i) {
			for (int j = 0; j < dangerMap.GetLength (1); ++j) {
				float value = 0;
				if (dangerMap [i, j] == -1) {
					value = 15 + benefitMap[i,j]*2 + distanceBenefit(j,i);
				} else {
					value = dangerMap [i, j] + benefitMap[i,j]*2 + distanceBenefit(j,i);
				}
				topValuePlace.Add (new MyPair (j, i, value));
			}
		}
		topValuePlace.Sort ();

		return topValuePlace;
	}

	private float distanceBenefit(int x,int y){
		float outDist =  10 - Math.Abs(this.pos.x-x)-Math.Abs(this.pos.y-y);
		float inDist = 2 - Math.Abs (this.pos.x - x) - Math.Abs (this.pos.y - y);
		return (outDist > 0&& inDist < 0) ? getRandom(10) : 0;
	}

	public class MyPair:IComparable{
		public float value;
		public int px;
		public int py;
		public MyPair(int x,int y,float value){
			this.px = x;
			this.py = y;
			this.value = value;
		}

		public int CompareTo(object obj)
		{
			if (obj is MyPair) {
				MyPair other = obj as MyPair;
				return this.value > other.value ? -1 : 1;
			} else {
				return 0;
			}		
		}
	}

}
	
