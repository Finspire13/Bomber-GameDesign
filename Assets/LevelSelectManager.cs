using UnityEngine;
using System.Collections;

public class LevelSelectManager : MonoBehaviour {

	public ListViewController CustomMapList;
	public ListViewController PresetMapList;

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

}
