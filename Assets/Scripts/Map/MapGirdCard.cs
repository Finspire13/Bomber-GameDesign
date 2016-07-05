using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapGirdCard : MonoBehaviour {

	public int MapDataCode{ get; set;}
	public bool Editable = true;

	public void setGridColor(){
		if (MapEditorHelper.checkCodeConstraint (MapDataCode, Editable)) {
			Debug.Log ("set color in grid"+MapEditorHelper.TargetColor);
			GetComponent<Image> ().color = MapEditorHelper.TargetColor;
			MapDataCode = MapEditorHelper.MapDataCode;
		}
	}
}
