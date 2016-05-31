using UnityEngine;
using System.Collections;

public class PlayerConrol : MonoBehaviour,Controlable,Locatable,SetBomb
{
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
				NormalBomb script = (NormalBomb)go.GetComponent("NormalBomb");
				if(script == null){
					Debug.Log("not script");
				}else{
					Debug.Log("find script");
					//script.LifeTime = bomblifeTime;
					script.isActive = true;
					script.LifeTime = 5;
				}
			}
		}
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
		this.position = new Position (0, 0);

		this.bombType = Resources.Load("NormalBomb") as GameObject;
		GameDataProcessor.instance.addObject (this);
		rhmFlag = false;
		RhythmRecorder.instance.addRhythmFlagOwner (this);

		idleMovementPosition = 0;

		this.maxNum = 3;
		this.currNum = 0;

		Debug.Log("PlayerControl Done..");
	}

	// Update is called once per frame
	void Update () {
		if (rhmFlag) {
			control ();
		}
	}

	public void control()
	{
		CheckMovement ();
		if (Time.frameCount % 10==0) {
			IdleMovement ();
		}
	}
	
	public void actionOnBeat ()
	{
	}

	void CheckMovement()
	{
		if (Input.GetKeyDown ("up") || Input.GetKeyDown (KeyCode.W)) {
			Debug.Log("up..");
			StartCoroutine(Move (moveDirection.forward));
		}
		if (Input.GetKeyDown ("down") || Input.GetKeyDown (KeyCode.S)) {
			Debug.Log("down..");
			StartCoroutine(Move (moveDirection.back));
		}
		if (Input.GetKeyDown ("right") || Input.GetKeyDown (KeyCode.D)) {
			Debug.Log("right..");
			StartCoroutine(Move (moveDirection.right));
		}
		if (Input.GetKeyDown ("left") || Input.GetKeyDown (KeyCode.A)) {
			Debug.Log("left..");
			StartCoroutine(Move (moveDirection.left));
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log("press down space");
			installBomb();
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

				this.position.y += speed;
				GameDataProcessor.instance.updatePosition(this,this.position);
				break;
			case moveDirection.back:
				transform.position += 0.25F*Vector3.back;
				m_ArmLeft.transform.position += 0.25F*Vector3.back;
				m_ArmRight.transform.position += 0.25F*Vector3.back;

				this.position.y -= speed;
				GameDataProcessor.instance.updatePosition(this,this.position);
				break;
			case moveDirection.right:
				transform.position += 0.25F*Vector3.right;
				m_ArmLeft.transform.position += 0.25F*Vector3.right;
				m_ArmRight.transform.position += 0.25F*Vector3.right;

				this.position.x += speed;
				GameDataProcessor.instance.updatePosition(this,this.position);
				break;
			case moveDirection.left:
				transform.position += 0.25F*Vector3.left;
				m_ArmLeft.transform.position += 0.25F*Vector3.left;
				m_ArmRight.transform.position += 0.25F*Vector3.left;

				this.position.x -= speed;
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

