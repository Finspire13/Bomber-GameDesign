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
	private int level;

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
			MapDataHelper.instance.loadMap(1);
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
}
