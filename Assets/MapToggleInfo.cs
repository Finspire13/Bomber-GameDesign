using UnityEngine;
using System.Collections;

public class MapToggleInfo : MonoBehaviour {

	public Color TargetColor;
	public MapDataEncoder.MapDataCodeEnum DataCode;


	public void setColorAndCode(bool isOn){
		if (isOn) {
			MapEditorHelper.TargetColor = TargetColor;
			MapEditorHelper.MapDataCode = (int)DataCode;
			Debug.Log ("Set Color to editor");
		}
	}
}
