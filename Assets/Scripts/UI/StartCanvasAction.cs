using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartCanvasAction : MonoBehaviour {

	public Button playButton;
	// Use this for initialization
	void Start () {
		playButton.onClick.AddListener (() => clickPlayButton ());
	}

	// Update is called once per frame
	void Update () {

	}

	void clickPlayButton(){
		GameManager.instance.changeGameState (GameState.playing);
	}
}
