using UnityEngine;
using System.Collections;

public class DifficultyLevelSelectController : MonoBehaviour {

	public int level;

	public void DifficultyLevelSelect(bool isSelect){
		if (isSelect) {
			GameManager.instance.level = this.level;
		}
	}
}
