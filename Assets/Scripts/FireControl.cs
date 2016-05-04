using UnityEngine;
using System.Collections;

public class FireControl : MonoBehaviour {
	private int duration = 10;
	public int Duration{
		get{return duration;}
		set{duration = value;}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		--duration;
		if (duration <= 0) {
			Destroy(this.gameObject,2);
		}
	}
}
