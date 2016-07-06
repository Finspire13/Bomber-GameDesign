using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartCanvasAction : MonoBehaviour {

	public Button playButton;
	public Button editMapButton;
	public Button exitButton;
	// Use this for initialization
	void Start () {
		playButton.onClick.AddListener (() => clickPlayButton ());
		editMapButton.onClick.AddListener (clickEditMapButton);
		exitButton.onClick.AddListener (clickExitButton);
	}

	// Update is called once per frame
	void Update () {

	}

	void clickPlayButton(){
		//GameManager.instance.changeGameState (GameState.playing);
		GameManager.instance.changeGameState (GameState.levelSelect);
	}

	void clickEditMapButton(){
		GameManager.instance.changeGameState (GameState.mapEdit);
	}

	void clickExitButton(){
		Application.Quit ();
	}
}
