using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MonsterState{MON_IDLE,MON_MOVE,MON_PURCHASE,MON_PRE_ATTACK,MON_ATTACK,MON_BACK};
public class MonsterControl : MonoBehaviour,Distroyable,Attackable,Locatable,MoveAble,ScoreCount
{
	protected Locatable goal;
	protected Position initialPos;
	MonsterState lastState;

	protected Position[] dirs;

	Queue<Position> currPath = new Queue<Position>();

	int blood = 50;
	public int Blood 
	{
		get{return blood;}
		set{blood = value;}
	}

	public void attackBy(Attackable source){
		this.blood -= source.Damage;
		if (blood <= 0) {
			if (source is BombFire && ((BombFire)source).Owner is PlayerConrol) {
				this.addToScore ();
			}
		}
	}
	public void distroy(){
	}
	int damage = 20;
	public int Damage 
	{
		get{return damage;}
		set{damage = value;}
	}
	public void attack(){
		int dirIndx = getRandom (3);
		int range = 2;
		for (int i = 0; i < range; ++i) {
			ArrayList objs = GameDataProcessor.instance.getObjectAtPostion (dirs [dirIndx]);
			foreach (Distroyable obj in objs) {
				obj.attackBy (this);
			}
			dirIndx = (dirIndx + 1) % 4;
		}
	}

	Position position;
	public Position pos{ 
		get{return position;} 
		set{position = value;}
	}
	int speed = 1;
	public int Speed 
	{
		get{return speed;}
		set{speed = value;}
	}

	protected string gameName = "Monster";
	protected float gameValue = 100f;
	public string getName(){
		return this.gameName;
	}
	public float getValue(){
		return this.gameValue;
	}
	public void addToScore(){
		GameManager.instance.addToPlayerScoreList (this);
	}
		
	protected int leastDistToPlayer(){
		int dist = 9999;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.GetLength (0); ++i) {
			Locatable local = (players [i].GetComponentInChildren (typeof(PlayerConrol)) as Locatable);
			int tempDist = Mathf.Abs(local.pos.x - this.position.x)
				+Mathf.Abs(local.pos.y - this.position.y);
			
			if (tempDist < dist) {
				dist = tempDist;
				goal = local;
			}
		}
		return dist;
	}
	protected int distToInitalPos(){
		return  Mathf.Abs(initialPos.x - this.position.x)
			+Mathf.Abs(initialPos.y - this.position.y);
	}
	protected MonsterState think(){
		int distance = leastDistToPlayer ();
		int distInitial = distToInitalPos ();
	
		if (distInitial >= 7) {
			this.currPath.Clear ();
			this.currPath = this.findPathTo (initialPos);
			return MonsterState.MON_BACK;
		}else{
			if (this.lastState == MonsterState.MON_PRE_ATTACK) {
				return MonsterState.MON_ATTACK;
			}
			if (distance <= 2) {
				return MonsterState.MON_PRE_ATTACK;
			}
			if (distance < 3 ) {
				this.currPath = this.findPathTo (goal.pos);
				return MonsterState.MON_PURCHASE;
			}
		}
//		Position[] dirs = {new Position (this.position.x + 1, this.position.y), new Position (this.position.x - 1, this.position.y),
//			new Position (this.position.x, this.position.y + 1), new Position (this.position.x, this.position.y - 1)};

		int dirIndx = 0;
		do {
			dirIndx = getRandom (3);
		} while (!isWall (dirs [dirIndx]));
		this.currPath.Clear ();
		this.currPath.Enqueue (dirs [dirIndx]);
		return MonsterState.MON_IDLE;
	}
		
	public void actionOnBeat (){
		MonsterState result = think();
		switch (result) {
		case MonsterState.MON_IDLE:
			MonsterIdle ();
			break;
		case MonsterState.MON_PRE_ATTACK:
			MonsterPreAttack ();
			break;
		case MonsterState.MON_ATTACK:
			MonsterAttack ();
			break;
		case MonsterState.MON_MOVE:
			MonsterMove ();
			break;
		case MonsterState.MON_BACK:
			MonsterBack ();
			break;
		case MonsterState.MON_PURCHASE:
			MonsterPurchase ();
			break;
		}
		lastState = result;
	}

	protected void MonsterIdle(){
		MonsterMove ();
	}
	protected void MonsterMove(){
		if (currPath.Count > 0) {
			Position temp = currPath.Dequeue ();
			StartCoroutine (MoveTo(temp));
			this.position.x = temp.x;
			this.position.y = temp.y;
		}
	}
	protected void MonsterPurchase(){
		MonsterMove ();
	}
	protected void MonsterPreAttack(){
		Debug.Log ("Monster prepare to attack");
	}
	protected void MonsterAttack(){
		this.attack ();
	}
	protected void MonsterBack(){
		MonsterMove ();
	}


	private IEnumerator MoveTo(Position dest){
		int frameCount = 5;
		float deltaX = dest.x - this.position.x;
		float deltaY = dest.y - this.position.y;
		for (int i = 0; i < frameCount; ++i) {
			this.transform.position += (deltaX / frameCount) * Vector3.right;
			this.transform.position += (deltaY / frameCount) * Vector3.back;

			yield return null;
		}
	}

	//寻路算法
	protected Queue<Position> findPathTo(Position dest){
		Queue<Position> path = new Queue<Position> ();

		int[,] floodMark = new int[GameDataProcessor.instance.mapSizeY,GameDataProcessor.instance.mapSizeX];
		for (int i = 0; i < floodMark.GetLength (0); ++i) {
			for (int j = 0; j < floodMark.GetLength (1); ++j) {
				floodMark[i,j] = -1;
			}
		}
		floodMark [this.pos.y, this.pos.x] = 0;
		bool canReach = true;
		markPathWFS(this.pos,dest,ref floodMark);
		if (floodMark [dest.y, dest.x] < 0) {
			canReach = false;
		} else {
			canReach = true;
		}

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

	protected void markPathWFS(Position source,Position dest,ref int[,] floodMark){
		Queue<Position> queSearch = new Queue<Position>();
		queSearch.Enqueue (source);
		floodMark [source.y, source.x] = 0;

		GameDataProcessor gdp = GameDataProcessor.instance;
		while (queSearch.Count > 0) {
			Position curr = queSearch.Dequeue ();
			int pathLen = floodMark[curr.y,curr.x] + 1;
			if (curr.y > 0 && curr.y < gdp.mapSizeY-1  && curr.x > 0 && curr.x < gdp.mapSizeX-1) {
				if (floodMark [curr.y, curr.x + 1] < 0) {
					Position temp = new Position (curr.x + 1, curr.y);
					bool isHereWall = isWall (temp);
					if (isHereWall) {
						floodMark [curr.y, curr.x + 1] = 999999;
					} else {
						floodMark [curr.y, curr.x + 1] = pathLen;
						queSearch.Enqueue (temp);
					}
				}

				if (floodMark [curr.y, curr.x - 1] < 0) {
					Position temp = new Position (curr.x - 1, curr.y);
					bool isHereWall = isWall (temp);
					if (isHereWall) {
						floodMark [curr.y, curr.x - 1] = 999999;
					} else {
						floodMark [curr.y, curr.x - 1] = pathLen;
						queSearch.Enqueue (temp);
					}
				}

				if (floodMark [curr.y + 1, curr.x] < 0) {
					Position temp = new Position (curr.x, curr.y + 1);
					bool isHereWall = isWall (temp);
					if (isHereWall) {
						floodMark [curr.y + 1, curr.x] = 999999;
					} else {
						floodMark [curr.y + 1, curr.x] = pathLen;
						queSearch.Enqueue (temp);
					}
				}

				if (floodMark [curr.y - 1, curr.x] < 0) {
					Position temp = new Position (curr.x, curr.y - 1);
					bool isHereWall = isWall (temp);
					if (isHereWall) {
						floodMark [curr.y - 1, curr.x] = 999999;
					} else {
						floodMark [curr.y - 1, curr.x] = pathLen;
						queSearch.Enqueue (temp);
					}
				}
			}
		}
	}

	protected bool isWall(Position position){
		ArrayList objs = GameDataProcessor.instance.getObjectAtPostion (position);
		for (int i = 0; i < objs.Count; ++i) {
			if (objs [i] is NormalCube || objs [i] is WallCube) {
				return true;
			}
		}
		return false;
	}

	protected void createPath(Position target, int distance, ref int[,] floodMark, ref Stack<Position> pathStatck){
		//		Stack<Position> pathStatck = new Stack<Position>();
//		Position[] dirs = {new Position (target.x + 1, target.y), new Position (target.x - 1, target.y),
//			new Position (target.x, target.y + 1), new Position (target.x, target.y - 1)};
		Position currSearchPos = new Position (target.x,target.y);

		while (distance > 1) {
			for (int i = 0; i < dirs.GetLength (0); ++i) {
				if (floodMark [dirs [i].y, dirs [i].x] == distance-1) {
					pathStatck.Push (dirs [i]);
					currSearchPos.x = dirs [i].x;
					currSearchPos.y = dirs [i].y;
					break;
				}
			}
			--distance;
		}
	}

	private int getRandom(int count)
	{
		return new System.Random().Next(count);
	}

	void Awake(){
		this.dirs = new Position[] {new Position (this.position.x + 1, this.position.y), new Position (this.position.x - 1, this.position.y),
			new Position (this.position.x, this.position.y + 1), new Position (this.position.x, this.position.y - 1)};
	}
	// Use this for initialization
	void Start ()
	{
		this.position = new Position(Mathf.RoundToInt(transform.localPosition.z),Mathf.RoundToInt(transform.localPosition.x));
		this.initialPos = new Position(Mathf.RoundToInt(transform.localPosition.z),Mathf.RoundToInt(transform.localPosition.x));
	}

	// Update is called once per frame
	void Update ()
	{
		if (blood <= 0) {
			this.distroy ();
		}
	}
}

