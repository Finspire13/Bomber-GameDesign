using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState{start,playing,levelEnd,end};

public class GameManager : MonoBehaviour {


	public static GameManager instance = null;

	public GameObject startCanvas;
	public GameObject playingCanvas;
	public GameObject levelEndCanvas;
	public GameObject endCanvas;
	private GameObject currentCanvas;
	private int playerBlood;
	public int level = 4;
	private bool canPlayerBuffed = true;

//	private float playerScore;
	public ArrayList playerScoreList = new ArrayList();
	public void addToPlayerScoreList(ScoreCount item){
		playerScoreList.Add (item);
	}
	public int playerBombCount = 0;
	public void increaseBombCount(SetBomb setter){
		if (setter is PlayerConrol) {
			++playerBombCount;
		}
	}

	public Dictionary<string,float> computeScore(){
		Dictionary<string,float> scoreMap = new Dictionary<string,float> ();
		foreach (ScoreCount sc in playerScoreList) {
			if (scoreMap.ContainsKey (sc.getName ())) {
				float temp = scoreMap [sc.getName ()];
				temp += sc.getValue ();
				scoreMap [sc.getName ()] = temp;
			} else {
				scoreMap.Add (sc.getName(),sc.getValue());
			}
		}
		return scoreMap;
	}
	public void levelSetting(){
		GameObject[] enemys = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		switch (level) {
		case 1:
			break;
		case 2:
			break;
		case 3:
			for (int i = 0; i < enemys.GetLength (0); ++i) {
				EnemyBomber enemy = enemys [i].GetComponent (typeof(EnemyBomber)) as EnemyBomber;
				enemy.Blood = 2 * enemy.Blood;
				enemy.MaxNum = 9999;
			}
			break;
		case 4:
			for (int i = 0;i < enemys.GetLength (0); ++i) {
				EnemyBomber enemy = enemys [i].GetComponent (typeof(EnemyBomber)) as EnemyBomber;
				enemy.Blood = 10 * enemy.Blood;
				enemy.MaxNum = 9999;
				enemy.Speed += 1;
			}
			for(int i = 0;i < players.GetLength(0);++i){
				PlayerConrol player = players [i].GetComponent (typeof(PlayerConrol)) as PlayerConrol;
				player.Blood = 1;
			}
			canPlayerBuffed = false;
			break;		
		default:
			break;
		}
	}

	public bool isBuffValid(System.Object gatherer){
		if (gatherer is PlayerConrol) {
			Debug.Log ("canPlayerBuffed:"+canPlayerBuffed);
			return canPlayerBuffed;
		}
		return true;
	}

	public int PlayerBlood
	{
		get{ return playerBlood; }
		set{ playerBlood = value; }
	}

	// Use this for initialization
	void Awake(){
		if (instance == null)
		    instance = this;
		else if (instance != this)
			Destroy (gameObject);    

		DontDestroyOnLoad (gameObject);
		currentCanvas = null;

		this.level = 3;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeGameState(GameState gameState){
		switch (gameState) {
		case GameState.start:
			Destroy (currentCanvas);
			currentCanvas = Instantiate (startCanvas) as GameObject;
			break;
		case GameState.playing:
			Destroy (currentCanvas);
			currentCanvas = Instantiate (playingCanvas) as GameObject;
			MapDataHelper.instance.createMapModel();
			break;
		case GameState.levelEnd:
			Destroy (currentCanvas);
			currentCanvas = Instantiate (levelEndCanvas) as GameObject;
			MapDataHelper.instance.deleteMapModel ();
			break;
		case GameState.end:
			Destroy (currentCanvas);
			currentCanvas = Instantiate (endCanvas) as GameObject;
			break;
		default:
			break;
		}
	}
}
