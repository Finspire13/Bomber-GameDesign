﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private int idleMovementPosition;
	public GameObject m_ArmLeft;
	public GameObject m_ArmRight;

	enum moveDirection{
		forward,back,left,right
	};

	// Use this for initialization
	void Start () {
		idleMovementPosition = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		CheckMovement ();
		if (Time.frameCount % 10==0) {
			IdleMovement ();
		}
	}

	void FixedUpdate(){

	}

	void CheckMovement()
	{
		if (Input.GetKeyDown ("up")) {
			StartCoroutine(Move (moveDirection.forward));
		}
		if (Input.GetKeyDown ("down")) {
			StartCoroutine(Move (moveDirection.back));
		}
		if (Input.GetKeyDown ("right")) {
			StartCoroutine(Move (moveDirection.right));
		}
		if (Input.GetKeyDown ("left")) {
			StartCoroutine(Move (moveDirection.left));
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