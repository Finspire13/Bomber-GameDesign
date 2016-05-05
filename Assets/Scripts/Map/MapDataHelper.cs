using UnityEngine;
using System.Collections;

public class MapDataHelper : MonoBehaviour {

    public TextAsset DEFAULT_MAP_DATA;

    public GameObject NORMAL_CUBE;
    public GameObject WALL_CUBE;
    public GameObject PLANE;
    public GameObject PLAYER_1;
    public GameObject PLAYER_2;

    [HideInInspector]
	public static MapDataHelper instance = null;
    public static string MAP_COMPONET_TAG = "MapComponent";

    //Const Char represent MapData componet
    private const char C_NORMAL_CUBE = '*';
    private const char C_WALL_CUBE = '#';
    private const char C_EMPTY = '-';
    private const char C_PLAYER_1 = '1';
    private const char C_PLAYER_2 = '2';

    struct MapData
    {
        public int row;
        public int column;
        public char[,] data;
        public bool isCreatable;
    };
    private MapData mMapData;

    private GameObject mMapModel;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);    

		DontDestroyOnLoad (gameObject);


		mMapData.isCreatable = false;
	}

    void Start()
    {
        
    }

	/*
    public static MapDataHelper GetInstance()
    {
        return mHelper;
    }*/

    public void loadFromAsset(TextAsset textAsset)
    {

        if (textAsset != null)
        {

            string txtMapData = textAsset.text;

            // Remove whiteSpace
            System.StringSplitOptions option = System.StringSplitOptions.RemoveEmptyEntries;

            // Split to lines
            string[] lines = txtMapData.Split(new char[] { '\r', '\n' }, option);

            // Split with ','
            char[] spliter = new char[1] { ',' };

            // Get row and length from first line
            string[] sizewh = lines[0].Split(spliter, option);
            mMapData.row = int.Parse(sizewh[0]);
            mMapData.column = int.Parse(sizewh[1]);

            char[,] mapdata = new char[mMapData.row, mMapData.column];

            for (int lineNum = 1; lineNum <= mMapData.row; lineNum++)
            {
                string[] data = lines[lineNum].Split(spliter, option);

                for (int col = 0; col < mMapData.column; col++)
                {
                    mapdata[lineNum-1, col] = data[col][0];
                }
            }
            mMapData.data = mapdata;
            mMapData.isCreatable = true;
        }
        else
        {
            mMapData.isCreatable = false;
            Debug.LogWarning("Map data asset is null");
        }

    }

    public void createMap()
    {
        if (mMapData.isCreatable)
        {
            if (mMapModel == null)
            {
                mMapModel = new GameObject("MapModel");
                mMapModel.transform.position = new Vector3(0, 0, 0);

                for (int x = 0; x < mMapData.row; x++)
                {
                    for (int z = 0; z < mMapData.column; z++)
                    {
                        createMapComponent(mMapData.data[x, z], new Vector3(x, 0.5f, z));
                    }
                }
                //Rotate map to fix x, z axis, but player will be rotated too
                //mMapModel.transform.Rotate(0, 90, 0);
            }
            else
            {
                Debug.LogWarning("Map had been created. Please delete Map first.");
            }   
        }
        else
        {
            Debug.LogWarning("Map can't be created. Please load MapData first.");
        }

    }

    public void createMapComponent(char componetChar, Vector3 position)
    {
        GameObject mapComponent;
        switch (componetChar)
        {
            case C_EMPTY:
                break;
            case C_NORMAL_CUBE:
                mapComponent = (GameObject)Instantiate(NORMAL_CUBE, position, Quaternion.identity);
                mapComponent.transform.parent = mMapModel.transform;
                break;
            case C_WALL_CUBE:
                mapComponent = (GameObject)Instantiate(WALL_CUBE, position, Quaternion.identity);
                mapComponent.transform.parent = mMapModel.transform;
                break;
            case C_PLAYER_1:
                mapComponent = (GameObject)Instantiate(PLAYER_1, position, Quaternion.identity);
                mapComponent.transform.parent = mMapModel.transform;
                break;
            case C_PLAYER_2:
                mapComponent = (GameObject)Instantiate(PLAYER_2, position, Quaternion.identity);
                mapComponent.transform.parent = mMapModel.transform;
                break;
            default:
                break;
        }
        
        position.y = 0f;
        mapComponent = (GameObject)Instantiate(PLANE, position, Quaternion.identity);
        mapComponent.transform.parent = mMapModel.transform;
    }


    public void createMapModel()
    {
        loadFromAsset(DEFAULT_MAP_DATA);
        createMap();
    }

    public void deleteMapModel()
    {
        if (mMapModel != null) { }
        DestroyImmediate(mMapModel);
    }
}
