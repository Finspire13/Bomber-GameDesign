using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelEndCanvasAction : MonoBehaviour {

	public Text gameStateText;
	public string gameLoseText = "Oh... You Lose";
	public string gameWinText = "Wow! You win";
	public Button endButton;
	public Button playAgainButton;
	public Button selectLevelButton;
	// Use this for initialization
	void Start () {
		endButton.onClick.AddListener (() => clickEndButton ());
		playAgainButton.onClick.AddListener (() => clickPlayAgainButton ());
		selectLevelButton.onClick.AddListener (clickSelectLevelButton);
		switch (GameManager.instance.levelState) {
			case LevelState.lose:
				gameStateText.text = gameLoseText;
				break;
			case LevelState.win:
				gameStateText.text = gameWinText;
				break;
			default:
				break;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void clickEndButton(){
		GameManager.instance.changeGameState (GameState.end);
	}

	void clickPlayAgainButton(){
		GameManager.instance.changeGameState (GameState.playing);
	}

	void clickSelectLevelButton(){
		GameManager.instance.changeGameState (GameState.levelSelect);
	}
}
