using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    private Level test;
    public int roomSpacingX = 14;
    public int roomSpacingY = 12;
    

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
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {

        

        
        
        
    }

	private void Awake()
	{
        // We pass playerManager references to the entrance and exit rooms so that we can spawn players there.
        // GameManager must be first because it spawns the playerManager!!!.
        gameManager = GameManager.GetInstance();
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
                        gameManager.currentEntranceRoom = outline;
                    }
                    else if (test.grid[i, j].type == RoomType.Exit)
                    {
                        GameObject health = Instantiate(testHealth);
                        health.transform.position = new Vector3(i * roomSpacingX, 0, j * roomSpacingY);
                        gameManager.currentExitRoom = outline;
                    }

                    room.transform.position = new Vector3(i * roomSpacingX, 0, j * roomSpacingY);
                    outline.transform.parent = room.transform;
                    outline.transform.localPosition = Vector3.zero;

                    // Giving the actual "room" in memory a reference to the room game object so it can access the RoomController.
                    test.grid[i, j].roomObj = room;

                    // Giving the RoomController a reference to the outline game object.
                    RoomController controller = room.GetComponent<RoomController>();
                    controller.outline = outline;

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

                    int randomNum = Random.Range(0, gameManager.numberOfLayouts);
                    string floorVariation = "Rooms/Layouts/" + randomNum.ToString();

                    //GameObject floor = Instantiate(Resources.Load<GameObject>(floorVariation));

                    GameObject roomReference = test.grid[i, j].roomObj;
                    //floor.transform.parent = roomReference.transform;
                    //floor.transform.localPosition = Vector3.zero;

                    // Passing the room controller a reference to the floor variation.
                    RoomController controller = roomReference.GetComponent<RoomController>();
                    //controller.layout = floor;

                    // If infiniteLoopPrevention hits 100, we stop trying to find a match. I picked 100 because its way above the amount of variations we will have for a while.
                    int infiniteLoopPrevention = 0;
                    OutlineData outlineData = controller.outline.GetComponent<OutlineData>();

                    bool foundMatch = false;

                    if (test.grid[i, j].type == RoomType.Entrance)
                    { 
                        GameObject floor = Instantiate(Resources.Load<GameObject>("Rooms/Layouts/0"));
                        floor.transform.parent = roomReference.transform;
                        floor.transform.localPosition = Vector3.zero;
                        floor.transform.rotation = controller.outline.transform.rotation;

                        controller.layout = floor;

                        continue;
                    }

                    else if (test.grid[i, j].type == RoomType.Exit)
                    {
                        int index = 0;
                        while (!foundMatch)
                        {
                            floorVariation = "Rooms/EndLayouts" + index.ToString();
                            GameObject floorToCheck = Resources.Load<GameObject>(floorVariation);
                            LayoutData layoutData = floorToCheck.GetComponent<LayoutData>();

                            if (CheckOutlineLayoutCompatability(outlineData, layoutData))
                            {
                                Debug.Log("Found match for exit floor and outline.");
                                GameObject floor = Instantiate(floorToCheck);
                                floor.transform.parent = roomReference.transform;
                                floor.transform.localPosition = Vector3.zero;
                                floor.transform.rotation = controller.outline.transform.rotation;

                                controller.layout = floor;
                                break;
                            }

                            else
                            {
                                Debug.Log("Exit floor and outline did not match. Re-generating new exit floor variation.");
                                // When we hit the end of the list of variations, we want to go back to the start incase we missed any.
                                index += 1;
                            }

                        }
                        // We don't want to execute whats left in this function.
                        continue;
                    }




                    while (!foundMatch)
                    {
                        infiniteLoopPrevention += 1;
                        if (infiniteLoopPrevention == 100)
                        {
                            Debug.Log("ERROR OCCURED: No variations match any outlines!!!");
                            break;
                        }

                        // Updating floorVariation to have the new rolled number.
                        floorVariation = "Rooms/Layouts/" + randomNum.ToString();

                        GameObject floorToCheck = Resources.Load<GameObject>(floorVariation);
                        LayoutData layoutData = floorToCheck.GetComponent<LayoutData>();

                        if (CheckOutlineLayoutCompatability(outlineData, layoutData))
                        {
                            Debug.Log("Found match for floor and outline.");
                            GameObject floor = Instantiate(floorToCheck);
                            floor.transform.parent = roomReference.transform;
                            floor.transform.localPosition = Vector3.zero;
                            floor.transform.rotation = controller.outline.transform.rotation;

                            controller.layout = floor;
                            break;
                        }

                        else
                        {
                            Debug.Log("Floor and outline did not match. Re-generating new floor variation.");
                            // When we hit the end of the list of variations, we want to go back to the start incase we missed any.
                            randomNum += 1;
                            if (randomNum == gameManager.numberOfLayouts + 1)
                                randomNum = 0;
                        }
                    }

                    //floor.transform.position = new Vector3(i * roomSpacingX, 0, j * roomSpacingY);
                }
            }
        }
    }

    bool CheckOutlineLayoutCompatability(OutlineData outline, LayoutData layout)
    {
        if ((outline.up == layout.up && outline.down == layout.down && outline.right == layout.right && outline.left == layout.left))
        {
            return true;
        }
        else if (layout.CompatibleWithEverything)
        {
            return true;
        }

        return false;
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
                leftRoom.transform.Rotate(new Vector3(0, 0, 0));
                return leftRoom;

            case RoomPattern.Right:
                GameObject rightRoom;
                rightRoom = Instantiate(roomRight);
                rightRoom.transform.Rotate(new Vector3(0, 0, 0));
                return rightRoom;

            case RoomPattern.Up:
                GameObject upFacingRoom = Instantiate(roomUp);
                upFacingRoom.transform.Rotate(new Vector3(0, 180, 0));
                return upFacingRoom;

            case RoomPattern.RightDown:
                GameObject rightDownRoom = Instantiate(roomLeftDown);
                rightDownRoom.transform.Rotate(new Vector3(0, 180, 0));
                return rightDownRoom;

            case RoomPattern.LeftDown:
                GameObject leftDownRoom = Instantiate(roomRightDown);
                leftDownRoom.transform.Rotate(new Vector3(0, 180, 0));
                return leftDownRoom;

            case RoomPattern.LeftUp:
                GameObject leftUpRoom = Instantiate(roomRightUp);
                leftUpRoom.transform.Rotate(new Vector3(0, 180, 0));
                return leftUpRoom;

            case RoomPattern.RightUp:
                GameObject rightUpRoom = Instantiate(roomLeftUp);
                rightUpRoom.transform.Rotate(new Vector3(0, 180, 0));
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
                upLeftDownRoom.transform.Rotate(new Vector3(0, 0, 0));
                return upLeftDownRoom;
            
            case RoomPattern.LeftRightUp:
                GameObject leftRightUpRoom = Instantiate(outlineLeftRightUp);
                leftRightUpRoom.transform.Rotate(new Vector3(0, 180, 0));
                return leftRightUpRoom;

            case RoomPattern.LeftRightDown:
                GameObject leftRightDownRoom = Instantiate(outlineLeftRightDown);
                leftRightDownRoom.transform.Rotate(new Vector3(0, 180, 0));
                return leftRightDownRoom;
            
            case RoomPattern.UpRightDown:
                GameObject upRightDownRoom = Instantiate(outlineUpRightDown);
                upRightDownRoom.transform.Rotate(new Vector3(0, 0, 0));
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
