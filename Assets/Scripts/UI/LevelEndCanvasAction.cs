using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelEndCanvasAction : MonoBehaviour {

	public Button endButton;
	public Button nextLevelButton;
	// Use this for initialization
	void Start () {
		endButton.onClick.AddListener (() => clickEndButton ());
		nextLevelButton.onClick.AddListener (() => clickNextLevelButton ());
	}

	// Update is called once per frame
	void Update () {

	}

	void clickEndButton(){
		GameManager.instance.changeGameState (GameState.end);
	}

	void clickNextLevelButton(){
		GameManager.instance.changeGameState (GameState.playing);
	}
}
