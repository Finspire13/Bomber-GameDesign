using UnityEngine;
using System.Collections;

public class PlayerConrol : MonoBehaviour,Controlable,Locatable,SetBomb
{
	private bool canSetBomb = true;

	private int speed = 1;
	public int Speed 
	{
		get{return speed;}
		set{speed = value;}
	}
	private bool rhmFlag = false;
	public bool rhythmFlag{ 
		get{return rhmFlag;} 
		set{rhmFlag=value;}
	}

	private Position position;
	public Position pos{ 
		get{ return position; } 
		set{ position=value; }
	}

	//capacity of bomb at a time
	private int maxNum;
	public int MaxNum {
		get{return this.maxNum;}
		set{maxNum = value;}
	}
	//already used number of bomb
	private int currNum;
	public int CurrNum {
		get{return this.currNum;}
		set{currNum = value;}
	}
	private GameObject bombType;
	public void setBomb(GameObject bombType){
		this.bombType = bombType;
	}
	public void installBomb(){
		if(currNum < maxNum){
			currNum++;
			if(bombType == null){
				Debug.Log("not bomb");
			}else{
				GameObject go = (GameObject)Instantiate(this.bombType,this.gameObject.transform.position,this.gameObject.transform.rotation);
				NormalBomb script = (NormalBomb)go.GetComponent("Bomb");
				if(script == null){
					Debug.Log("not script");
				}else{
					Debug.Log("find script interface Bomb");
					//script.LifeTime = bomblifeTime;
					//script.isActive = true;
//					script.LifeTime = 3;
					script.setProperties(this,bombPower,bombLifeTime,bombFireTime);

					GameDataProcessor.instance.addToDangerMap (script);
				}
			}
		}
	}
	private int bombPower = 2;
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
		if(currNum > 0){
			currNum--;
		}
	}

	private int idleMovementPosition;
	public GameObject m_ArmLeft;
	public GameObject m_ArmRight;

	enum moveDirection{
		forward,back,left,right
	};

	// Use this for initialization
	void Start ()
	{
		//should initize position of player 
		this.position = new Position (1, 1);
//		this.position = new Position(Mathf.CeilToInt(transform.localPosition.z),Mathf.CeilToInt(transform.localPosition.x));

//		Debug.Log ("x:"+transform.localPosition.z+",y:"+transform.localPosition.x);
		this.bombType = Resources.Load("NormalBomb") as GameObject;
//		GameDataProcessor.instance.addObject (this);
		rhmFlag = false;
		RhythmRecorder.instance.addRhythmFlagOwner (this);

		idleMovementPosition = 0;

		this.maxNum = 3;
		this.currNum = 0;
		this.bombLifeTime = 3;
		this.bombPower = 1;
		this.bombFireTime = 1;
		this.canSetBomb = true;

//		Debug.Log("PlayerControl Done..");
	}

	// Update is called once per frame
	void Update () {
		//this.position = new Position(Mathf.RoundToInt(transform.localPosition.z)+1,Mathf.RoundToInt(transform.localPosition.x)+1);
//		Debug.Log ("Player:"+this.position.x+","+this.position.y);
		control ();
	}

	public void control()
	{
		if (rhmFlag) {
			canSetBomb = true;
			CheckMovement ();

		}
		if (Time.frameCount % 10==0) {
			IdleMovement ();
		}
	}
	
	public void actionOnBeat ()
	{
	}

	//need to add move check to avoid walking out of the map
	void CheckMovement()
	{
		if (Input.GetKeyDown (KeyCode.Space) && canSetBomb) {
			//			Debug.Log ("press down space");
			installBomb ();
			canSetBomb = false;
			//rhmFlag = false;
		}
		if (Input.GetKeyDown ("up") || Input.GetKeyDown (KeyCode.W)) {
//			Debug.Log("up..");

			ArrayList frontalObjects=GameDataProcessor.instance.getBackObjects (this);
			bool tempFlag = true;
			foreach (Locatable l in frontalObjects) {
				if (l is WallCube||l is NormalCube)
					tempFlag = false;
			}

			if (tempFlag) {
				StartCoroutine (Move (moveDirection.forward));
				this.position.y -= speed;
			}
			rhmFlag = false;
		}
		if (Input.GetKeyDown ("down") || Input.GetKeyDown (KeyCode.S)) {
//			Debug.Log("down..");

			ArrayList frontalObjects=GameDataProcessor.instance.getFrontalObjects (this);
			bool tempFlag = true;
			foreach (Locatable l in frontalObjects) {
				if (l is WallCube||l is NormalCube)
					tempFlag = false;
			}

			if (tempFlag) {
				StartCoroutine (Move (moveDirection.back));
				this.position.y += speed;
			}
			rhmFlag = false;
		}
		if (Input.GetKeyDown ("right") || Input.GetKeyDown (KeyCode.D)) {
//			Debug.Log("right..");

			ArrayList frontalObjects=GameDataProcessor.instance.getRightObjects (this);
			bool tempFlag = true;
			foreach (Locatable l in frontalObjects) {
				if (l is WallCube||l is NormalCube)
					tempFlag = false;
			}

			if (tempFlag) {
				StartCoroutine (Move (moveDirection.right));
				this.position.x += speed;
			}
			rhmFlag = false;
		}
		if (Input.GetKeyDown ("left") || Input.GetKeyDown (KeyCode.A)) {
//			Debug.Log("left..");

			ArrayList frontalObjects=GameDataProcessor.instance.getLeftObjects (this);
			bool tempFlag = true;
			foreach (Locatable l in frontalObjects) {
				if (l is WallCube||l is NormalCube)
					tempFlag = false;
			}

			if (tempFlag) {
				StartCoroutine (Move (moveDirection.left));
				this.position.x -= speed;
			}
			rhmFlag = false;
		}
	}
	
	IEnumerator Move(moveDirection dir)
	{
		for (int i = 0; i < 4; i++) {
			
			if (i == 0||i == 1) {
				transform.position += 0.5F*Vector3.up;
				m_ArmLeft.transform.position += 0.5F*Vector3.up;
				m_ArmRight.transform.position += 0.5F*Vector3.up;
			}
			else if (i == 2||i == 3) {
				transform.position += 0.5F*Vector3.down;
				m_ArmLeft.transform.position += 0.5F*Vector3.down;
				m_ArmRight.transform.position += 0.5F*Vector3.down;
			}
			
			switch (dir) {
			case moveDirection.forward:
				transform.position += 0.25F*Vector3.forward;
				m_ArmLeft.transform.position += 0.25F*Vector3.forward;
				m_ArmRight.transform.position += 0.25F*Vector3.forward;

				GameDataProcessor.instance.updatePosition(this,this.position);
				break;
			case moveDirection.back:
				transform.position += 0.25F*Vector3.back;
				m_ArmLeft.transform.position += 0.25F*Vector3.back;
				m_ArmRight.transform.position += 0.25F*Vector3.back;

				GameDataProcessor.instance.updatePosition(this,this.position);
				break;
			case moveDirection.right:
				transform.position += 0.25F*Vector3.right;
				m_ArmLeft.transform.position += 0.25F*Vector3.right;
				m_ArmRight.transform.position += 0.25F*Vector3.right;

				GameDataProcessor.instance.updatePosition(this,this.position);
				break;
			case moveDirection.left:
				transform.position += 0.25F*Vector3.left;
				m_ArmLeft.transform.position += 0.25F*Vector3.left;
				m_ArmRight.transform.position += 0.25F*Vector3.left;

				GameDataProcessor.instance.updatePosition(this,this.position);
				break;
			}
			yield return null;
		}
	}
	
	void IdleMovement()	{
		if (idleMovementPosition == 1) {
			m_ArmLeft.transform.position += 0.3F*Vector3.down;
			m_ArmRight.transform.position += 0.3F*Vector3.down;
			idleMovementPosition = 0;
		} 
		else if (idleMovementPosition == 0) {
			m_ArmLeft.transform.position += 0.3F*Vector3.up;
			m_ArmRight.transform.position += 0.3F*Vector3.up;
			idleMovementPosition = 1;
		}
	}
}

