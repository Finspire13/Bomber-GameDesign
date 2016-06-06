using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndCanvasAction : MonoBehaviour {

	public Button tryAgainButton;
	// Use this for initialization
	void Start () {
		tryAgainButton.onClick.AddListener (() => clickTryAgainButton ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void clickTryAgainButton(){
		GameManager.instance.changeGameState (GameState.start);
	}
}
