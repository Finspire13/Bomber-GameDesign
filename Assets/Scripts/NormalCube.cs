using UnityEngine;
using System.Collections;
using System;

public class NormalCube : MonoBehaviour, Locatable,Distroyable,ScoreCount {

	private string gameName = "Wall-NormalWall";
	private float gameValue = 2f;
	public string getName(){
		return this.gameName;
	}
	public float getValue(){
		return this.gameValue;
	}
	public void addToScore(){
		GameManager.instance.addToPlayerScoreList (this);
	}

    private Position position;
    public Position pos
    {
        get { return position; }
        set { position = value; }
    }
	public void actionOnBeat(){
	}

	private int blood;
	public int Blood
	{
		get { return blood; }
		set { blood = value; }
	}
	public void attackBy(Attackable source)
	{
//		Debug.Log("NormalCube->attackBy(),damage:"+source.Damage);
		//        blood -= source.Damage;
		blood = 0;
	}
	public void distroy()
	{
		GameDataProcessor.instance.removeObject (this);
		Destroy(this.gameObject, 0);
	}
    // Use this for initialization
    void Start()
    {
        //Initize position of cube 
        //Debug.Log(Mathf.RoundToInt(transform.localPosition.x) + ", " + Mathf.RoundToInt(transform.localPosition.z));
		this.position = new Position(Mathf.RoundToInt(transform.localPosition.z),Mathf.RoundToInt(transform.localPosition.x));
//		Debug.Log ("NC:"+this.position.x+","+this.position.y);
		blood = 1;
    }

	// Update is called once per frame
	void Update()
	{
		if (blood <= 0)
		{
//			Debug.Log("cube died!!!");
			createBuff ();
			distroy();
		}
	}

	void createBuff(){

		string[] buffList = {"Heal","GhostForm","BombPowerUp","BombNumberUp","FireLifeTimeUp","SpeedUp","SlowDown" };
		//string[] buffList = {"FireLifeTimeUp"};
		int buffIndex=UnityEngine.Random.Range (0, buffList.Length);

		GameObject buff = Resources.Load(buffList[buffIndex]) as GameObject;
		GameObject obj = (GameObject)Instantiate(buff,this.gameObject.transform.position,this.gameObject.transform.rotation);
		Buff script = (Buff)obj.GetComponent("Buff");
		if (script == null) {
			Debug.Log ("not script");
		} else {
			if (script is Locatable) {
//				Debug.Log ("script is Locatable");
				((Locatable)script).pos = new Position (this.pos.x,this.pos.y);
			}
		}
	}

	void OnDestroy(){
		this.distroy ();
	}

}
