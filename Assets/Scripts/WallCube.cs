using UnityEngine;
using System.Collections;
using System;

public class WallCube : MonoBehaviour, Locatable {

    private Position position;

    public Position pos
    {
        get { return position; }
        set { position = value; }
    }

    // Use this for initialization
    void Start () {
        //Initize position of cube 
        //Debug.Log(Mathf.RoundToInt(transform.localPosition.x)+", "+Mathf.RoundToInt(transform.localPosition.z));
		this.position = new Position(Mathf.RoundToInt(transform.localPosition.z),Mathf.RoundToInt(transform.localPosition.x));
//		Debug.Log(Mathf.RoundToInt(transform.localPosition.z)+", "+Mathf.RoundToInt(transform.localPosition.x));
        

    }

    // Update is called once per frame

}
