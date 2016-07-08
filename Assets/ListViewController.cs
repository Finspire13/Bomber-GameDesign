using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ListViewController : MonoBehaviour {

	public GameObject ListView;
	public GameObject ListContentPrefab;

	public void addListContents(string[] contents, PassStringEvent onClick){
		GameObject listContent;
		foreach (string content in contents) {
			listContent = (GameObject)Instantiate (ListContentPrefab);
			listContent.GetComponentInChildren<Text> ().text = content;
			listContent.GetComponent<ListContentController> ().OnClick = onClick;
			listContent.transform.SetParent(ListView.transform, false);
		}
	}

	public void addListContents(string[] contents){
		GameObject listContent;
		foreach (string content in contents) {
			listContent = (GameObject)Instantiate (ListContentPrefab);
			listContent.GetComponentInChildren<Text> ().text = content;
			listContent.transform.SetParent(ListView.transform, false);
		}
	}


}
