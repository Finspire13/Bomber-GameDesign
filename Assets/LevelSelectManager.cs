using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelectManager : MonoBehaviour {

	public ListViewController CustomMapList;
	public ListViewController PresetMapList;
	public Toggle[] LevelToggles;
	public int DifficulityLevel = 1;

	private string[] presetMaps;

	// Use this for initialization
	void Start () {
		PassStringEvent onCustomCLick = new PassStringEvent ();
		PassStringEvent onPresetClick = new PassStringEvent ();
		onCustomCLick.AddListener (startWithCustomMap);
		onPresetClick.AddListener (startWithPresetMap);
		CustomMapList.addListContents (MapDataHelper.instance.getCustomMapList(), onCustomCLick);
		presetMaps = MapDataHelper.instance.getPresetMapList ();
		PresetMapList.addListContents (presetMaps, onPresetClick);
		DifficulityLevel = GameManager.instance.level;
		initToggles ();
	}

	public void startWithPresetMap(string mapName){
		GameManager.instance.playMode = PlayMode.presetMap;
		GameManager.instance.presetMap = System.Array.IndexOf(presetMaps, mapName) + 1;
		GameManager.instance.changeGameState (GameState.playing);
	}

	public void startWithCustomMap(string mapName){
		GameManager.instance.playMode = PlayMode.customMap;
		GameManager.instance.customMap = mapName;
		GameManager.instance.changeGameState (GameState.playing);
	}

	public void backToMenu(){
		GameManager.instance.changeGameState (GameState.start);
	}

	void initToggles(){
		foreach (Toggle tg in LevelToggles) {
			tg.isOn = false;
		}
		LevelToggles [DifficulityLevel - 1].isOn = true;
	}

}
