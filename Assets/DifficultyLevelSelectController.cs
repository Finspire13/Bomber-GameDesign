using UnityEngine;
using System.Collections;

public class DifficultyLevelSelectController : MonoBehaviour {

	public int level;
	public LevelSelectManager manager;

	public void DifficultyLevelSelect(bool isSelect){
		if (isSelect) {
			GameManager.instance.level = this.level;
			manager.DifficulityLevel = this.level;
			Debug.Log ("set Level: " + level);
		}
	}
}
