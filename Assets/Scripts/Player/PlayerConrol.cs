using UnityEngine;
using System.Collections;

public class PlayerConrol : MonoBehaviour,Controlable,Locatable,SetBomb,Distroyable,CanBuffed,MoveAble
{
	private int blood;
	public int Blood 
	{
		get{return blood;}
		set{blood = value;}
	}
	public void attackBy(Attackable source){
		blood -= source.Damage;
	}
	public void distroy(){
		Destroy (this.gameObject, 0);
	}

	private bool canSetBomb = true;
	private bool isGhost=false;
	public bool IsGhost 
	{
		get{return isGhost;}
		set{isGhost = value;}
	}

	private int speed = 1;
	public int Speed 
	{
		get{return speed;}
		set{speed = value;}
	}
	private int rhmFlag = 0;
	public int rhythmFlag{ 
		get{return rhmFlag;} 
		set{rhmFlag=value;}
	}
//	private int moveAbility = 1;
//	public int MoveAbility{ 
//		get{return moveAbility;} 
//		set{this.moveAbility = value;}
//	}

	private Position position;
	public Position pos{ 
		get{ return position; } 
		set{ position=value; }
	}

	//capacity of bomb at a timeobtainTools
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
//			Debug.Log("player currNum:"+currNum);
			if(bombType == null){
				Debug.Log("not bomb");
			}else{
				GameObject go = (GameObject)Instantiate(this.bombType,this.gameObject.transform.position,this.gameObject.transform.rotation);
				Bomb script = (Bomb)go.GetComponent("Bomb");
				if(script == null){
					Debug.Log("not script");
				}else{
//					Debug.Log("find script interface Bomb");
					//script.LifeTime = bomblifeTime;
					//script.isActive = true;
//					script.LifeTime = 3;
					script.setProperties(this,bombPower,bombLifeTime,bombFireTime);
					this.registerBomb (script);
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
	private int bombLifeTime = 3;
	public int BombLifeTime {
		get{return bombLifeTime; }
		set{bombLifeTime = value; }
	}
	private int bombFireTime = 1;
	public int BombFireTime {
		get{return bombFireTime; }
		set{bombFireTime = value; }
	}

	public void notifyExplosion (Bomb bomb){
		if(currNum > 0){
			currNum--;
		}
		bombList.Remove (bomb);
//		Debug.Log ("Player notifyExplosion");
	}

	private int idleMovementPosition;
	public GameObject m_ArmLeft;
	public GameObject m_ArmRight;

	enum moveDirection{
		forward,back,left,right
	};

	private GameObject followCamera;

	// Use this for initialization
	void Start ()
	{
		BombTriggleTool tool = new BombTriggleTool (this);
		this.obtainTools (tool);
		this.obtainTools (tool);


		this.position = new Position(Mathf.CeilToInt(transform.localPosition.z),Mathf.CeilToInt(transform.localPosition.x));

		//Add follow camera to player
		followCamera = GameObject.FindGameObjectWithTag("MainCamera");

//		Debug.Log ("player position: x:"+transform.localPosition.z+",y:"+transform.localPosition.x);
//		Debug.Log ("player: x:"+this.pos.x+",y:"+this.pos.y);

		this.bombType = Resources.Load("NormalBomb") as GameObject;
//		GameDataProcessor.instance.addObject (this);
		rhmFlag = 0;
		RhythmRecorder.instance.addRhythmFlagOwner (this);

		idleMovementPosition = 0;

		this.maxNum = 10;
		this.currNum = 0;
		this.bombLifeTime = 30;
		this.bombPower = 1;
		this.bombFireTime = 1;
		this.canSetBomb = true;
		this.isGhost = false;
		this.blood = 50;


//		Debug.Log("PlayerControl Done..");
	}

	// Update is called once per frame
	void Update () {
		//this.position = new Position(Mathf.RoundToInt(transform.localPosition.z)+1,Mathf.RoundToInt(transform.localPosition.x)+1);
//		Debug.Log ("Player:"+this.position.x+","+this.position.y);
		control ();

		GameManager.instance.PlayerBlood = this.blood;
		if (blood <= 0) {
			Debug.Log("game over!!!");
			distroy();
		}

		setFollowCamera ();
	}

	void setFollowCamera(){
		if (followCamera != null) {
			followCamera.transform.position = new Vector3(transform.position.x, followCamera.transform.position.y, transform.position.z);
		}
	}

	public void control()
	{
		if (rhmFlag > 0) {
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
//		if (Input.GetKeyDown (KeyCode.Z)) {
//			if (activeToolState == 1) {
//				triggleAllBomb ();
//			}
//			if (activeToolState == 2) {
//			}
//			Debug.Log ("player uses tool");
//		}

		//check of using of bomb tools
		for (int i = 0; i < playerTools.Count; ++i) {
			if(playerTools[i] is BombTool){
				BombTool currTool = playerTools [i] as BombTool;
				if (Input.GetKeyDown (currTool.getKeyCode())) {
					currTool.useToolBy (this);
//					Debug.Log (currTool.getToolName());
				}
			}
		}

		if (Input.GetKeyDown ("up") || Input.GetKeyDown (KeyCode.W)) {
//			Debug.Log("up..");

			ArrayList frontalObjects=GameDataProcessor.instance.getBackObjects (this);
			bool tempFlag = true;
			foreach (Locatable l in frontalObjects) {
				if (l is WallCube||l is NormalCube)
					tempFlag = false;
			}

			if ((tempFlag||isGhost)&&this.position.y>0) {
				StartCoroutine (Move (moveDirection.forward));
				this.position.y -= 1;
			}
			--rhmFlag;
		}
		if (Input.GetKeyDown ("down") || Input.GetKeyDown (KeyCode.S)) {
//			Debug.Log("down..");

			ArrayList frontalObjects=GameDataProcessor.instance.getFrontalObjects (this);
			bool tempFlag = true;
			foreach (Locatable l in frontalObjects) {
				if (l is WallCube||l is NormalCube)
					tempFlag = false;
			}

			if ((tempFlag||isGhost)&&this.position.y<GameDataProcessor.instance.mapSizeY-1) {
				StartCoroutine (Move (moveDirection.back));
				this.position.y += 1;
			}
			--rhmFlag;
		}
		if (Input.GetKeyDown ("right") || Input.GetKeyDown (KeyCode.D)) {
//			Debug.Log("right..");

			ArrayList frontalObjects=GameDataProcessor.instance.getRightObjects (this);
			bool tempFlag = true;
			foreach (Locatable l in frontalObjects) {
				if (l is WallCube||l is NormalCube)
					tempFlag = false;
			}

			if ((tempFlag||isGhost)&&this.position.x<GameDataProcessor.instance.mapSizeX-1) {
				StartCoroutine (Move (moveDirection.right));
				this.position.x += 1;
			}
			--rhmFlag;
		}
		if (Input.GetKeyDown ("left") || Input.GetKeyDown (KeyCode.A)) {
//			Debug.Log("left..");

			ArrayList frontalObjects=GameDataProcessor.instance.getLeftObjects (this);
			bool tempFlag = true;
			foreach (Locatable l in frontalObjects) {
				if (l is WallCube||l is NormalCube)
					tempFlag = false;
			}

			if ((tempFlag||isGhost)&&this.position.x>0) {
				StartCoroutine (Move (moveDirection.left));
				this.position.x -= 1;
			}
			--rhmFlag;
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
				//m_ArmLeft.transform.position += 0.25F*Vector3.forward;
				//m_ArmRight.transform.position += 0.25F*Vector3.forward;

				GameDataProcessor.instance.updatePosition(this,this.position);
				break;
			case moveDirection.back:
				transform.position += 0.25F*Vector3.back;
				//m_ArmLeft.transform.position += 0.25F*Vector3.back;
				//m_ArmRight.transform.position += 0.25F*Vector3.back;

				GameDataProcessor.instance.updatePosition(this,this.position);
				break;
			case moveDirection.right:
				transform.position += 0.25F*Vector3.right;
				//m_ArmLeft.transform.position += 0.25F*Vector3.right;
				//m_ArmRight.transform.position += 0.25F*Vector3.right;

				GameDataProcessor.instance.updatePosition(this,this.position);
				break;
			case moveDirection.left:
				transform.position += 0.25F*Vector3.left;
				//m_ArmLeft.transform.position += 0.25F*Vector3.left;
				//m_ArmRight.transform.position += 0.25F*Vector3.left;

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
		
	private ArrayList bombList = new ArrayList();
	public ArrayList playerTools = new ArrayList();

	public void registerBomb (Bomb bomb){
		bombList.Add (bomb);
	}
	public ArrayList getAllBomb(){
		return bombList;
	}

	public bool obtainTools (BombTool tool){
		
		foreach(BombTool temp in playerTools){
			Debug.Log (temp.getToolName()+"has already existed");
			return false;
		}
		playerTools.Add (tool);
		return true;
	}
}

