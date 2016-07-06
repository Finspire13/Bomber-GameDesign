using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class PassStringEvent : UnityEvent<string>{}

public class ListContentController : MonoBehaviour {

	public PassStringEvent OnClick;

	void Start(){
	}

	public void ListContentClickTrigger(){
		OnClick.Invoke (GetComponentInChildren<Text>().text);
	}
}
