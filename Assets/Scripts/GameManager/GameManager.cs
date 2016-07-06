using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState{start,levelSelect,mapEdit,playing,levelEnd,end};

public enum PlayMode{presetMap, customMap};

public class GameManager : MonoBehaviour {


	public static GameManager instance = null;

	public GameObject startCanvas;
	public GameObject playingCanvas;
	public GameObject levelSelectCanvas;
	public GameObject mapEditCanvas;
	public GameObject levelEndCanvas;
	public GameObject endCanvas;

	private int enemyNum;
	public int EnemyNum{
		get{return enemyNum;}
		set{enemyNum = value;}
	}

	private GameObject currentCanvas;
	private int playerBlood;
	public int level = 4;
	private bool canPlayerBuffed = true;

	public PlayMode playMode;
	public int presetMap = 1;
	public string customMap = "";

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
		case GameState.levelSelect:
			Destroy (currentCanvas);
			currentCanvas = Instantiate (levelSelectCanvas) as GameObject;
			break;
		case GameState.mapEdit:
			Destroy (currentCanvas);
			currentCanvas = Instantiate (mapEditCanvas) as GameObject;
			break;
		case GameState.playing:
			Destroy (currentCanvas);
			//coupling
			GameDataProcessor.instance.gameObjects.Clear ();
			currentCanvas = Instantiate (playingCanvas) as GameObject;
			if (playMode == PlayMode.presetMap)
				MapDataHelper.instance.loadMap (presetMap);
			else if (playMode ==PlayMode.customMap)
				MapDataHelper.instance.loadMap (customMap);
			this.resetGame ();
			break;
		case GameState.levelEnd:
			Destroy (currentCanvas);
			currentCanvas = Instantiate (levelEndCanvas) as GameObject;
			MapDataHelper.instance.deleteMap ();
			break;
		case GameState.end:
			Destroy (currentCanvas);
			currentCanvas = Instantiate (endCanvas) as GameObject;
			break;
		default:
			break;
		}
	}

	public void resetGame(){
		GameDataProcessor.instance.resetGame ();
		RhythmRecorder.instance.resetGame ();
	}
}
