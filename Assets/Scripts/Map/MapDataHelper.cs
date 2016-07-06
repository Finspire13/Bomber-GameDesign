using UnityEngine;
using System.Collections;
using System.IO;

public class MapDataHelper : MonoBehaviour {

	public TextAsset[] LEVEL_MAP_DATA;

    public GameObject NORMAL_CUBE;
    public GameObject WALL_CUBE;
    public GameObject PLANE;
    public GameObject PLAYER;
    public GameObject ENEMY;
	public GameObject[] Objects;

    [HideInInspector]
	public static MapDataHelper instance = null;

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
		
	public void setMapData(MapData mapData){
		this.mMapData = mapData;
	}
		

	public void loadMap(int levelNum){
		deleteMap ();
		loadFromAsset (LEVEL_MAP_DATA[levelNum-1]);
		createMap ();
	}

	public void loadMap(string fileName){
		deleteMap ();
		loadFromFile (fileName);
		createMap ();
	}

	public string[] getCustomMapList(){
		string filter = "*.cmap";
		string[] maps = Directory.GetFiles(Application.persistentDataPath, filter); 

		for (int i = 0, len = maps.Length; i < len; i++) {
			maps [i] = Path.GetFileName (maps[i]);
		}
		return maps;
	}

	public string[] getPresetMapList(){
		string[] maps = new string[LEVEL_MAP_DATA.Length];
		for (int i = 0, len = maps.Length; i < len; i++) {
			maps [i] = "Level " + (i+1);
		}
		return maps;
	}

    void loadFromAsset(TextAsset textAsset)
    {
        if (textAsset != null)
        {

            string txtMapData = textAsset.text;

            // Split to lines
			string[] lines = txtMapData.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            // Split with ','
            char[] spliter = new char[1] { ',' };

            // Get row and length from first line
			string[] sizewh = lines[0].Split(spliter, System.StringSplitOptions.RemoveEmptyEntries);
            mMapData.row = int.Parse(sizewh[0]);
            mMapData.column = int.Parse(sizewh[1]);

            int[,] mapdata = new int[mMapData.row, mMapData.column];

            for (int lineNum = 1; lineNum <= mMapData.row; lineNum++)
            {
				string[] data = lines[lineNum].Split(spliter, System.StringSplitOptions.RemoveEmptyEntries);

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

	void loadFromFile(string fileName){
		if (fileName != "")
		{
			string[] lines = File.ReadAllLines(Application.persistentDataPath+"/"+fileName);

			// Split with ','
			char[] spliter = new char[1] { ',' };

			// Get row and length from first line
			string[] sizewh = lines[0].Split(spliter,  System.StringSplitOptions.RemoveEmptyEntries);
			mMapData.row = int.Parse(sizewh[0]);
			mMapData.column = int.Parse(sizewh[1]);

			int[,] mapdata = new int[mMapData.row, mMapData.column];

			for (int lineNum = 1; lineNum <= mMapData.row; lineNum++)
			{
				string[] data = lines[lineNum].Split(spliter, System.StringSplitOptions.RemoveEmptyEntries);

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

    void createMap()
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

		//add game level info here
		GameManager.instance.levelSetting ();

    }

    void createMapComponent(int componetChar, Vector3 position)
    {
        GameObject mapComponent;
        switch (componetChar)
        {
		case (int)MapDataEncoder.MapDataCodeEnum.EMPTY:
			break;
		case (int)MapDataEncoder.MapDataCodeEnum.NORMAL_CUBE:
			mapComponent = (GameObject)Instantiate (Objects[3], position, Quaternion.identity);
			mapComponent.transform.parent = mMapModel.transform;
			GameDataProcessor.instance.addObject (mapComponent.GetComponent<NormalCube>());
            break;
		case (int)MapDataEncoder.MapDataCodeEnum.WALL_CUBE:
			mapComponent = (GameObject)Instantiate (Objects[4], position, Quaternion.identity);
			mapComponent.transform.parent = mMapModel.transform;
			GameDataProcessor.instance.addObject (mapComponent.GetComponent<WallCube>());
			break;
		case (int)MapDataEncoder.MapDataCodeEnum.PLAYER:
			mapComponent = (GameObject)Instantiate (Objects[1], position, Quaternion.identity);
			mapComponent.transform.parent = mMapModel.transform;
			mapComponent.transform.Rotate (0, -90, 0);
			GameDataProcessor.instance.addObject (mapComponent.GetComponentInChildren<PlayerConrol> ());
			mapComponent.tag = "Player";
			break;
		case (int)MapDataEncoder.MapDataCodeEnum.ENEMY:
			mapComponent = (GameObject)Instantiate (Objects[2], position, Quaternion.identity);
			mapComponent.transform.parent = mMapModel.transform;
			mapComponent.transform.Rotate (0, -90, 0);
			mapComponent.tag = "Enemy";
			//leave blank
			break;
		case (int)MapDataEncoder.MapDataCodeEnum.MONSTER:
			mapComponent = (GameObject)Instantiate (Objects[5], position, Quaternion.identity);
			mapComponent.transform.parent = mMapModel.transform;
			mapComponent.transform.Rotate (0, -90, 0);
			mapComponent.tag = "Monster";
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
		loadMap (1);
//		loadMap("ss.txt");
    }

    public void deleteMap()
    {
		if (mMapModel != null) {
			Destroy (mMapModel,0);
			mMapModel = null;
		}
    }
}
