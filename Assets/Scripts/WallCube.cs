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
		Debug.Log("NormalCube->attackBy(),damage:"+source.Damage);
//        blood -= source.Damage;
		blood = 0;
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
//		Debug.Log(Mathf.RoundToInt(transform.localPosition.z)+", "+Mathf.RoundToInt(transform.localPosition.x));
        blood = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (blood <= 0)
        {
            Debug.Log("cube died!!!");
			createBuff ();

            distroy();
			GameDataProcessor.instance.removeObject (this);
        }
    }

	void createBuff(){
		GameObject buff = Resources.Load("BombPowerUp") as GameObject;
		GameObject obj = (GameObject)Instantiate(buff,this.gameObject.transform.position,this.gameObject.transform.rotation);
		Buff script = (Buff)obj.GetComponent("Buff");
		if (script == null) {
			Debug.Log ("not script");
		} else {
			if (script is Locatable) {
				Debug.Log ("script is Locatable");
				((Locatable)script).pos = new Position (this.pos.x,this.pos.y);
			}
		}
	}
}
