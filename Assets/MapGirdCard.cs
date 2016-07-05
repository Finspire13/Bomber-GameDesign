using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapGirdCard : MonoBehaviour {

	public int MapDataCode{ get; set;}

	public void setGridColor(){
		Debug.Log ("set color in grid"+MapEditorHelper.TargetColor);
		GetComponent<Image> ().color = MapEditorHelper.TargetColor;
		MapDataCode = MapEditorHelper.MapDataCode;
	}
}
