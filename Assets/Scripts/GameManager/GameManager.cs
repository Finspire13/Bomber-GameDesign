using UnityEngine;
using System.Collections;

public enum GameState{start,playing,levelEnd,end};

public class GameManager : MonoBehaviour {


	public static GameManager instance = null;

	public GameObject startCanvas;
	public GameObject playingCanvas;
	public GameObject levelEndCanvas;
	public GameObject endCanvas;
	private GameObject currentCanvas;
	private int level;


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
