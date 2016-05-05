using UnityEngine;
using System.Collections;

public class MapUnityEditor : MonoBehaviour {

    public TextAsset DEFAULT_DATA;

    public void createMapModel()
    {
		MapDataHelper mHelper = MapDataHelper.instance;
        mHelper.loadFromAsset(DEFAULT_DATA);
        mHelper.createMap();
    }

    public void deleteMapModel()
    {

    }
}
