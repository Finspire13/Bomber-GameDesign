using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ListViewController : MonoBehaviour {

	public GameObject ListView;
	public GameObject ListContentPrefab;

	public void addListContents(string[] contents){
		GameObject listContent;
		foreach (string content in contents) {
			listContent = (GameObject)Instantiate (ListContentPrefab);
			listContent.GetComponentInChildren<Text> ().text = content;
			//TODO: add onCLick method to list content here
			//...
			listContent.transform.SetParent(ListView.transform, false);
		}
	}

	void OnEnable(){
		addListContents (getCustomMapList());
	}

	public string[] getCustomMapList(){
		string filter = "*.cmap";
		string[] maps = Directory.GetFiles(Application.persistentDataPath, filter); 

		for (int i = 0, len = maps.Length; i < len; i++) {
			maps [i] = Path.GetFileName (maps[i]);
		}
		return maps;
	}
}
