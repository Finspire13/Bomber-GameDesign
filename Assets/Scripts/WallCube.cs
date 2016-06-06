using UnityEngine;
using System.Collections;
using System;

public class WallCube : MonoBehaviour, Locatable, Distroyable {

    private Position position;

    public Position pos
    {
        get { return position; }
        set { position = value; }
    }

    private int blood;
    public int Blood
    {
        get { return blood; }
        set { blood = value; }
    }
    public void attackBy(Attackable source)
    {
        blood -= source.Damage;
    }
    public void distroy()
    {
        Destroy(this.gameObject, 0);
    }

    public void actionOnBeat()
    {

    }

    // Use this for initialization
    void Start () {
        //Initize position of cube 
        //Debug.Log(Mathf.RoundToInt(transform.localPosition.x)+", "+Mathf.RoundToInt(transform.localPosition.z));
		this.position = new Position(Mathf.RoundToInt(transform.localPosition.z),Mathf.RoundToInt(transform.localPosition.x));
		//Debug.Log(Mathf.RoundToInt(transform.localPosition.x)+", "+Mathf.RoundToInt(transform.localPosition.z));
        blood = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (blood <= 0)
        {
            Debug.Log("game over!!!");
            distroy();
        }
    }

}
