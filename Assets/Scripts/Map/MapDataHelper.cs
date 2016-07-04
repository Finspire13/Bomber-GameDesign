using UnityEngine;
using System.Collections;

public class MapDataHelper : MonoBehaviour {

    public TextAsset DEFAULT_MAP_DATA;

    public GameObject NORMAL_CUBE;
    public GameObject WALL_CUBE;
    public GameObject PLANE;
    public GameObject PLAYER;
    public GameObject ENEMY;

    [HideInInspector]
	public static MapDataHelper instance = null;
    //public static string MAP_COMPONET_TAG = "MapComponent";

    //Const Char represent MapData componet
    private const int C_NORMAL_CUBE = 3;
    private const int C_WALL_CUBE = 4;
    private const int C_EMPTY = 0;
    private const int C_PLAYER = 1;
    private const int C_ENEMY = 2;

    public struct MapData
    {
        public int row;
        public int column;
        public int[,] data;
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

	public void setMapData(MapData mapData){
		this.mMapData = mapData;
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

            int[,] mapdata = new int[mMapData.row, mMapData.column];

            for (int lineNum = 1; lineNum <= mMapData.row; lineNum++)
            {
                string[] data = lines[lineNum].Split(spliter, option);

                for (int col = 0; col < mMapData.column; col++)
                {
                    mapdata[lineNum-1, col] = int.Parse(data[col]);
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

                //Assign mapSize to GameDataProcessor 
                if (GameDataProcessor.instance != null)
                {
                    //Debug.Log(mMapData.row + ", " + mMapData.column);
                    GameDataProcessor.instance.mapSizeX = mMapData.row;
                    GameDataProcessor.instance.mapSizeY = mMapData.column;
                }

                for (int x = 0; x < mMapData.row; x++)
                {
                    for (int z = 0; z < mMapData.column; z++)
                    {
                        createMapComponent(mMapData.data[x, z], new Vector3(x, 0.5f, z));
                    }
                }
                //Rotate map to fix x, z axis, but player will be rotated too
                mMapModel.transform.Rotate(0, 90, 0);
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

    void createMapComponent(int componetChar, Vector3 position)
    {
        GameObject mapComponent;
        switch (componetChar)
        {
		case C_EMPTY:
			break;
		case C_NORMAL_CUBE:
			mapComponent = (GameObject)Instantiate (NORMAL_CUBE, position, Quaternion.identity);
			mapComponent.transform.parent = mMapModel.transform;
			GameDataProcessor.instance.addObject (mapComponent.GetComponent<NormalCube>());
            break;
		case C_WALL_CUBE:
			mapComponent = (GameObject)Instantiate (WALL_CUBE, position, Quaternion.identity);
			mapComponent.transform.parent = mMapModel.transform;
			GameDataProcessor.instance.addObject (mapComponent.GetComponent<WallCube>());
			break;
		case C_PLAYER:
			mapComponent = (GameObject)Instantiate (PLAYER, position, Quaternion.identity);
			mapComponent.transform.parent = mMapModel.transform;
			mapComponent.transform.Rotate (0, -90, 0);
			GameDataProcessor.instance.addObject (mapComponent.GetComponentInChildren<PlayerConrol>());
			break;
		case C_ENEMY:
			mapComponent = (GameObject)Instantiate (ENEMY, position, Quaternion.identity);
			mapComponent.transform.parent = mMapModel.transform;
			mapComponent.transform.Rotate (0, -90, 0);
			//leave blank
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
		if (mMapModel != null) {
			Destroy (mMapModel,0);
		}
    }

    public int[,] getMapData()
    {
        if (mMapData.isCreatable)
        {
            return mMapData.data;
        }
        return null;
    }
}
