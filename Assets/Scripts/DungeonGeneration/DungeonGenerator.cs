using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    private Level test;
    public int roomSpacingX = 20;
    public int roomSpacingY = 20;
    

    public GameObject roomLeftRightUpDown;
    public GameObject roomLeftRight;
    public GameObject roomLeftUp;
    public GameObject roomLeft;
    public GameObject roomRight;
    public GameObject roomUpDown;
    public GameObject roomClosed;
    public GameObject roomRightDown;
    public GameObject roomLeftDown;
    public GameObject roomRightUp;
    public GameObject roomUp;
    public GameObject roomDown;


    public GameObject testPlayer;
    public GameObject testHealth;

    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {

        

        
        
        
    }

	private void Awake()
	{
        // We pass playerManager references to the entrance and exit rooms so that we can spawn players there.
        playerManager = PlayerManager.GetInstance();

        test = new Level(4, 4);
        test.InitRooms();
        test.GenerateMainPath();
        GeneratePrefabs();
        GenerateFloorVariations();
    }

	// Update is called once per frame
	void Update()
    {
        
    }

    void GeneratePrefabs()
    {
        for (int i = 0; i < test.levelWidth; i++)
        {
            for (int j = 0; j < test.levelHeight; j++)
            {
                GameObject room = GetRoomPrefab(test.grid[i, j].pattern);
                if (room != null)
                {
                    if (test.grid[i, j].type == RoomType.Entrance)
                    {
                        //GameObject player = Instantiate(testPlayer);
                        //player.transform.position = new Vector3(i * roomSpacingX, 1, j * roomSpacingY);
                        playerManager.currentEntranceRoom = room;
                    }
                    else if (test.grid[i, j].type == RoomType.Exit)
                    {
                        GameObject health = Instantiate(testHealth);
                        health.transform.position = new Vector3(i * roomSpacingX, 0, j * roomSpacingY);
                        playerManager.currentExitRoom = room;
                    }
                    room.transform.position = new Vector3(i * roomSpacingX, 0, j * roomSpacingY);
                    
                }
            }
        }
    }

    void GenerateFloorVariations()
    {
        for (int i = 0; i < test.levelWidth; i++)
        {
            for (int j = 0; j < test.levelHeight; j++)
            {
                if (test.grid[i, j].pattern != RoomPattern.Closed)
                { 
                    int randomNum = Random.Range(0, 12);
                    string roomPath = "Rooms/Layouts/" + randomNum.ToString();
                    //GameObject room = Resources.Load(roomPath) as GameObject;
                    GameObject roomReal = Instantiate(Resources.Load<GameObject>(roomPath));
                    roomReal.transform.position = new Vector3(i * roomSpacingX, 0, j * roomSpacingY);
                }
            }
        }
    }

    GameObject GetRoomPrefab(RoomPattern pattern)
    {
        switch (pattern)
        {
            case RoomPattern.Down:
                GameObject downFacingRoom = Instantiate(roomDown);
                downFacingRoom.transform.Rotate(new Vector3(0, 180, 0));
                return downFacingRoom;

            case RoomPattern.Left:
                GameObject leftRoom;
                leftRoom = Instantiate(roomLeft);
                leftRoom.transform.Rotate(new Vector3(0, 180, 0));
                return leftRoom;

            case RoomPattern.Right:
                GameObject rightRoom;
                rightRoom = Instantiate(roomRight);
                rightRoom.transform.Rotate(new Vector3(0, 180, 0));
                return rightRoom;

            case RoomPattern.Up:
                GameObject upFacingRoom = Instantiate(roomUp);
                upFacingRoom.transform.Rotate(new Vector3(0, 180, 0));
                return upFacingRoom;

            case RoomPattern.RightDown:
                GameObject rightDownRoom = Instantiate(roomLeftDown);
                //rightDownRoom.transform.Rotate(new Vector3(0, 90, 0));
                return rightDownRoom;

            case RoomPattern.LeftDown:
                GameObject leftDownRoom = Instantiate(roomRightDown);
                //leftDownRoom.transform.Rotate(new Vector3(0, 90, 0));
                return leftDownRoom;

            case RoomPattern.LeftUp:
                GameObject leftUpRoom = Instantiate(roomRightUp);
                //leftUpRoom.transform.Rotate(new Vector3(0, -180, 0));
                return leftUpRoom;

            case RoomPattern.RightUp:
                GameObject rightUpRoom = Instantiate(roomLeftUp);
                //rightUpRoom.transform.Rotate(new Vector3(0, 0, 0));
                return rightUpRoom;

            case RoomPattern.UpDown:
                GameObject upDownRoom = Instantiate(roomUpDown);
                upDownRoom.transform.Rotate(new Vector3(0, 0, 0));
                return upDownRoom;

            case RoomPattern.LeftRight:
                GameObject leftRightRoom = Instantiate(roomLeftRight);
                leftRightRoom.transform.Rotate(new Vector3(0, 0, 0));
                return leftRightRoom;

            default:
                //GameObject cantBeBotherRoom = Instantiate(roomClosed);
                //return cantBeBotherRoom;
                return null;

        }
        
    }
}
