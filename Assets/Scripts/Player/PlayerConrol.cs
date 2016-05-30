using UnityEngine;
using System.Collections;

public class PlayerConrol : MonoBehaviour,Controlable,Locatable
{
	private int speed = 1;
	private bool rhmFlag = false;
	private int idleMovementPosition;
	public GameObject m_ArmLeft;
	public GameObject m_ArmRight;

	enum moveDirection{
		forward,back,left,right
	};

	public int Speed 
	{
		get{return speed;}
		set{speed = value;}
	}

	// Use this for initialization
	void Start ()
	{
		GameDataProcessor.instance.addObject (this);
		idleMovementPosition = 0;
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

	public bool rhythmFlag{ 
		get{return rhmFlag;} 
		set{rhmFlag=value;}
	}
	
	void CheckMovement()
	{
		if (Input.GetKeyDown ("up") || Input.GetKeyDown (KeyCode.W)) {
			StartCoroutine(Move (moveDirection.forward));
		}
		if (Input.GetKeyDown ("down") || Input.GetKeyDown (KeyCode.S)) {
			StartCoroutine(Move (moveDirection.back));
		}
		if (Input.GetKeyDown ("right") || Input.GetKeyDown (KeyCode.D)) {
			StartCoroutine(Move (moveDirection.right));
		}
		if (Input.GetKeyDown ("left") || Input.GetKeyDown (KeyCode.A)) {
			StartCoroutine(Move (moveDirection.left));
		}
//		if (Input.GetKeyDown (KeyCode.Space)) {
//			Debug.Log("press down space");
//			GameObject bomb = Resources.Load("bomb") as GameObject;
//			if(bomb == null){
//				Debug.Log("not bomb");
//			}else{
//				GameObject go = (GameObject)Instantiate(bomb,this.gameObject.transform.position,this.gameObject.transform.rotation);
//				Explosion script = (Explosion)go.GetComponent("Explosion");
//				if(script == null){
//					Debug.Log("not script");
//				}else{
//					Debug.Log("find script");
//					//script.LifeTime = bomblifeTime;
//					script.Active = true;
//					script.LifeTime = bomblifeTime;
//				}
//			}
//		}
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
				break;
			case moveDirection.back:
				transform.position += 0.25F*Vector3.back;
				m_ArmLeft.transform.position += 0.25F*Vector3.back;
				m_ArmRight.transform.position += 0.25F*Vector3.back;
				break;
			case moveDirection.right:
				transform.position += 0.25F*Vector3.right;
				m_ArmLeft.transform.position += 0.25F*Vector3.right;
				m_ArmRight.transform.position += 0.25F*Vector3.right;
				break;
			case moveDirection.left:
				transform.position += 0.25F*Vector3.left;
				m_ArmLeft.transform.position += 0.25F*Vector3.left;
				m_ArmRight.transform.position += 0.25F*Vector3.left;
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

