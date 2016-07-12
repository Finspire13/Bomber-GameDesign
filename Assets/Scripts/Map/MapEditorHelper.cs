using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class MapEditorHelper : MonoBehaviour {

	public static Color TargetColor = new Color(255, 255, 255);

	public static int MapDataCode{ get; set;}

	static int playerNum = 1;

	MapDataHelper.MapData mMapData;

	public RectTransform MapGirdLayout;
	public Color DefaultColor;
	public GameObject DialogPanel;

	string fileName = "";

	public static bool checkCodeConstraint(int gridCode, bool editable){
		if (!editable) {
			return false;
		}
		if (MapDataCode == MapDataEncoder.C_PLAYER) {
			if (playerNum != 0) {
				playerNum--;
				return true;
			} else {
				return false;
			}
		} else {
			if (gridCode == MapDataEncoder.C_PLAYER) {
				playerNum++;
			}
			return true;
		}
	}

	void Awake(){
		mMapData.row = 15;
		mMapData.column = 15;
		mMapData.data = new int[15, 15];
		mMapData.isCreatable = false;
		initMapGridCard ();
	}
		
	void getMapData(){
		int row;
		int column;
		int num = 0;
		foreach (RectTransform child in MapGirdLayout) {
			column = num % 15;
			row = num / 15;
//			Debug.Log (child.gameObject.GetComponent<MapGirdCard> ().MapDataCode + child.name);
			mMapData.data[row, column] = child.gameObject.GetComponent<MapGirdCard> ().MapDataCode;
			num++;
		}
	}

	void initMapGridCard(){
		int row;
		int column;
		int num = 0;
		foreach (RectTransform child in MapGirdLayout) {
			column = num % 15;
			row = num / 15;
			if (row == 0 || row == 14 || column == 0 || column == 14) {
				MapGirdCard card = child.GetComponent<MapGirdCard> ();
				card.MapDataCode = MapDataEncoder.C_WALL_CUBE;
				child.GetComponent<Image> ().color = DefaultColor;
				card.Editable = false;
			}
			num++;
		}
	}

	public void saveMapDatatoFile(){
		if (fileName != "") {
			getMapData ();
			using (StreamWriter sw = new StreamWriter (Application.persistentDataPath + "/" + fileName + ".cmap")) {
				sw.WriteLine (mMapData.row + "," + mMapData.column);
				string s;
				for (int r = 0; r < mMapData.row; r++) {
					s = "";
					for (int c = 0; c < mMapData.column; c++) {
						if (c == 0)
							s = s + mMapData.data [r, c];
						else
							s = s + "," + mMapData.data [r, c];
					}
					sw.WriteLine (s);
				}
			}
			Debug.Log (Application.persistentDataPath);
			fileName = "";
			playerNum  = 1;
		}
	}

	public void showDialogPanel(){
		InputField inputFiled = DialogPanel.GetComponentInChildren<InputField> ();
		DialogPanel.SetActive (true);
		if (inputFiled.text != "") {
			inputFiled.ActivateInputField ();
		}
	}

	public void hideDialogPanel(){
		DialogPanel.SetActive (false);
		fileName = "";
	}

	public void setSavedFileName(string name){
		fileName = name;
	}

	public void exitMapEditor(){
		playerNum  = 1;
		GameManager.instance.changeGameState (GameState.start);
	}
}
