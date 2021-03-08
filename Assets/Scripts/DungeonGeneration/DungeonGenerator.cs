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

    public GameObject outlineLeftRightDown;
    public GameObject outlineLeftRightUp;
    public GameObject outlineUpRightDown;
    public GameObject outlineUpLeftDown;

   

    public GameObject roomObj;




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

        test.GenerateDeadEnds();

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
                GameObject outline = GetRoomPrefab(test.grid[i, j].pattern);
                if (outline != null)
                {
                    GameObject room = Instantiate(roomObj);
                    if (test.grid[i, j].type == RoomType.Entrance)
                    {
                        //GameObject player = Instantiate(testPlayer);
                        //player.transform.position = new Vector3(i * roomSpacingX, 1, j * roomSpacingY);
                        playerManager.currentEntranceRoom = outline;
                    }
                    else if (test.grid[i, j].type == RoomType.Exit)
                    {
                        GameObject health = Instantiate(testHealth);
                        health.transform.position = new Vector3(i * roomSpacingX, 0, j * roomSpacingY);
                        playerManager.currentExitRoom = outline;
                    }

                    room.transform.position = new Vector3(i * roomSpacingX, 0, j * roomSpacingY);
                    outline.transform.parent = room.transform;
                    outline.transform.localPosition = Vector3.zero;

                    // Giving the actual "room" in memory a reference to the room game object so it can access the RoomController.
                    test.grid[i, j].roomObj = room;

                    //outline.transform.position = new Vector3(i * roomSpacingX, 0, j * roomSpacingY);
                    
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
                    int randomNum = Random.Range(0, 11);
                    string floorVariation = "Rooms/Layouts/" + randomNum.ToString();
                    //GameObject room = Resources.Load(roomPath) as GameObject;
                    GameObject floor = Instantiate(Resources.Load<GameObject>(floorVariation));

                    GameObject roomReference = test.grid[i, j].roomObj;
                    floor.transform.parent = roomReference.transform;
                    floor.transform.localPosition = Vector3.zero;

                    // Passing the room controller a reference to the floor variation.
                    RoomController controller = roomReference.GetComponent<RoomController>();
                    controller.layout = floor;

                    //floor.transform.position = new Vector3(i * roomSpacingX, 0, j * roomSpacingY);
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

            case RoomPattern.UpLeftDown:
                GameObject upLeftDownRoom = Instantiate(outlineUpLeftDown);
                upLeftDownRoom.transform.Rotate(new Vector3(0, 180, 0));
                return upLeftDownRoom;
            
            case RoomPattern.LeftRightUp:
                GameObject leftRightUpRoom = Instantiate(outlineLeftRightUp);
                return leftRightUpRoom;

            case RoomPattern.LeftRightDown:
                GameObject leftRightDownRoom = Instantiate(outlineLeftRightDown);
                return leftRightDownRoom;
            
            case RoomPattern.UpRightDown:
                GameObject upRightDownRoom = Instantiate(outlineUpRightDown);
                return upRightDownRoom;


            case RoomPattern.UpDownLeftRight:
                GameObject upDownLeftRightRoom = Instantiate(roomLeftRightUpDown);
                return upDownLeftRightRoom;

            default:
                //GameObject cantBeBotherRoom = Instantiate(roomClosed);
                //return cantBeBotherRoom;
                return null;

        }
        
    }
}
