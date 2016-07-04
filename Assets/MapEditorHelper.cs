using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapEditorHelper : MonoBehaviour {

	public static Color TargetColor = new Color(255, 255, 255);

	public static int MapDataCode{ get; set;}

	MapDataHelper.MapData mMapData;

	public RectTransform MapGirdLayout;

	void Awake(){
		mMapData.row = 15;
		mMapData.column = 15;
		mMapData.data = new int[15, 15];
		mMapData.isCreatable = false;
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

	void OnDisable(){
		getMapData ();
	}

}
